#region Apache License
//
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership. 
// The ASF licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with 
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NJetty.Util.Util
{

    /// <summary>
    /// Date Format Cache.
    /// Computes string representations of Dates and caches
    /// the results so that subsequent requests within the same minute
    /// will be fast.
    /// 
    /// Only format strings that contain either "ss" or "ss.SSS" are handled.
    /// 
    /// The timezone of the date may be included as an ID with the "zzz"
    /// format string or as an offset with the "ZZZ" format string.
    /// 
    /// If consecutive calls are frequently very different, then this
    /// may be a little slower than a normal DateFormat.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    public class DateCache
    {

        public static string DEFAULT_FORMAT = "EEE MMM dd HH:mm:ss zzz yyyy";
        static long __hitWindow = 60 * 60;

        string _formatString;
        string _tzFormatString;
        string _tzFormat;

        string _minFormatString;
        string _minFormat;

        string _secFormatString;
        string _secFormatString0;
        string _secFormatString1;

        long _lastMinutes = -1;
        long _lastSeconds = -1;
        int _lastMs = -1;
        string _lastResult = null;

        CultureInfo _locale = null;
        DateTimeFormatInfo _dfs = null;

        object _thisLock = new object();
        static object _classLock = new object();
        TimeZoneInfo _timeZone;

        #region Constructors

        /// <summary>
        /// Make a DateCache that will use a default format. The default format 
        /// generates the same results as Date.ToString().
        /// </summary>
        public DateCache()
            : this(DEFAULT_FORMAT)
        {
            this.TimeZone = TimeZoneInfo.Local;
        }

        /// <summary>
        /// Make a DateCache that will use the given format
        /// </summary>
        /// <param name="format"></param>
        public DateCache(string format)
        {
            _formatString = format;
            this.TimeZone = TimeZoneInfo.Local;

        }

        public DateCache(string format, CultureInfo l)
        {
            _formatString = format;
            _locale = l;
            this.TimeZone = TimeZoneInfo.Local;
        }

        public DateCache(string format, DateTimeFormatInfo s)
        {
            _formatString = format;
            _dfs = s;
            this.TimeZone = TimeZoneInfo.Local;
        }

        #endregion

        /// <summary>
        /// Gets or Sets the timezone.
        /// </summary>
        public TimeZoneInfo TimeZone
        {
            set
            {
                setTzFormatString(value);
                if (_locale != null)
                {

                    //_tzFormat = new SimpleDateFormat(_tzFormatString, _locale);
                    _tzFormat = _tzFormatString;
                    //_minFormat = new SimpleDateFormat(_minFormatString, _locale);
                    _minFormat = _minFormatString;
                }
                else if (_dfs != null)
                {

                    //_tzFormat = new SimpleDateFormat(_tzFormatString, _dfs);
                    _tzFormat = _tzFormatString;
                    //_minFormat = new SimpleDateFormat(_minFormatString, _dfs);
                    _minFormat = _minFormatString;

                }
                else
                {
                    //_tzFormat = new SimpleDateFormat(_tzFormatString);
                    _tzFormat = _tzFormatString;
                    //_minFormat = new SimpleDateFormat(_minFormatString);
                    _minFormat = _minFormatString;

                }

                //_tzFormat.setTimeZone(value);
                //_minFormat.setTimeZone(value);
                _timeZone = value;
                _lastSeconds = -1;
                _lastMinutes = -1;
            }

            get
            {
                //return _tzFormat.getTimeZone();
                return _timeZone;
            }
        }


        /// <summary>
        /// Set the timezone. by using Timezone ID
        /// </summary>
        public string TimeZoneID
        {
            set
            {
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(value);
                this.TimeZone = tzi;

            }
        }

        /* ------------------------------------------------------------ */
        void setTzFormatString(TimeZoneInfo tz)
        {
            int zIndex = _formatString.IndexOf("ZZZ");
            if (zIndex >= 0)
            {
                string ss1 = _formatString.Substring(0, zIndex);
                string ss2 = _formatString.Substring(zIndex + 3);
                int tzOffset = tz.BaseUtcOffset.Milliseconds;


                StringBuilder sb = new StringBuilder(_formatString.Length + 10);
                sb.Append(ss1);
                sb.Append("'");
                if (tzOffset >= 0)
                    sb.Append('+');
                else
                {
                    tzOffset = -tzOffset;
                    sb.Append('-');
                }

                int raw = tzOffset / (1000 * 60);		// Convert to seconds
                int hr = raw / 60;
                int min = raw % 60;

                if (hr < 10)
                    sb.Append('0');
                sb.Append(hr);
                if (min < 10)
                    sb.Append('0');
                sb.Append(min);
                sb.Append('\'');

                sb.Append(ss2);
                _tzFormatString = sb.ToString();
            }
            else
                _tzFormatString = _formatString;
            setMinFormatString();
        }


        /* ------------------------------------------------------------ */
        void setMinFormatString()
        {
            int i = _tzFormatString.IndexOf("ss.SSS");
            int l = 6;
            if (i >= 0)
                throw new ArgumentException("ms not supported");
            i = _tzFormatString.IndexOf("ss");
            l = 2;

            // Build a formatter that formats a second format string
            string ss1 = _tzFormatString.Substring(0, i);
            string ss2 = _tzFormatString.Substring(i + l);
            _minFormatString = ss1 + "'ss'" + ss2;
        }


        /// <summary>
        /// Format a date according to our stored formatter.
        /// </summary>
        /// <param name="inDate">date to format</param>
        /// <returns>returns a formatted date</returns>
        public string Format(DateTime inDate)
        {
            lock (_thisLock)
            {
                return Format(inDate.Ticks);
            }
        }

        /* ------------------------------------------------------------ */
        /** Format a date according to our stored formatter.
         * @param inDate 
         * @return Formatted date
         */
        public string Format(long inDate)
        {
            lock (_thisLock)
            {
                long seconds = inDate / 1000000;

                // Is it not suitable to cache?
                if (seconds < _lastSeconds ||
                    _lastSeconds > 0 && seconds > _lastSeconds + __hitWindow)
                {
                    // It's a cache miss
                    DateTime d1 = new DateTime(inDate);
                    return d1.ToString(_tzFormat);

                }

                // Check if we are in the same second
                // and don't care about millis
                if (_lastSeconds == seconds)
                    return _lastResult;

                DateTime d = new DateTime(inDate);

                // Check if we need a new format string
                long minutes = seconds / 60;
                if (_lastMinutes != minutes)
                {
                    _lastMinutes = minutes;
                    _secFormatString = d.ToString(_minFormat);

                    int i = _secFormatString.IndexOf("ss");
                    int l = 2;
                    _secFormatString0 = _secFormatString.Substring(0, i);
                    _secFormatString1 = _secFormatString.Substring(i + l);
                }

                // Always format if we get here
                _lastSeconds = seconds;
                StringBuilder sb = new StringBuilder(_secFormatString.Length);
                sb.Append(_secFormatString0);
                int s = (int)(seconds % 60);
                if (s < 10)
                    sb.Append('0');
                sb.Append(s);
                sb.Append(_secFormatString1);
                _lastResult = sb.ToString();


                return _lastResult;
            }
        }

        /* ------------------------------------------------------------ */
        /** Format to string buffer. 
         * @param inDate Date the format
         * @param buffer StringBuilder
         */
        public void Format(long inDate, StringBuilder buffer)
        {
            buffer.Append(Format(inDate));
        }

        /* ------------------------------------------------------------ */
        /** Get the format.
         */
        public string DateTimeFormat
        {
            get { return _minFormat; }
        }

        public string FormatString
        {
            get
            {
                return _formatString;
            }
        }

        public string Now
        {
            get
            {
                long now = DateTime.Now.Ticks;
                _lastMs = (int)(now % 1000000);
                return Format(now);
            }
        }

        public int LastMs
        {
            get
            {
                return _lastMs;
            }
        }

    }
}
