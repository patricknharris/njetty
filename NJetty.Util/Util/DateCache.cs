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
        
        public static string DEFAULT_FORMAT="EEE MMM dd HH:mm:ss zzz yyyy";
        private static long __hitWindow=60*60;
        
        private string _formatString;
        private string _tzFormatString;
        private DateTimeFormatInfo _tzFormat;
        
        private string _minFormatString;
        private DateTimeFormatInfo _minFormat;

        private string _secFormatString;
        private string _secFormatString0;
        private string _secFormatString1;

        private long _lastMinutes = -1;
        private long _lastSeconds = -1;
        private int _lastMs = -1;
        private string _lastResult = null;

        private CultureInfo _locale = null;
        private DateTimeFormatInfo _dfs = null;

        private object _thisLock = new object();
        private static object _classLock = new object();

        #region Constructors

        /// <summary>
        /// Make a DateCache that will use a default format. The default format 
        /// generates the same results as Date.ToString().
        /// </summary>
        public DateCache() : this(DEFAULT_FORMAT)
        {
            this.TimeZone = TimeZone.CurrentTimeZone;
        }
        
        /// <summary>
        /// Make a DateCache that will use the given format
        /// </summary>
        /// <param name="format"></param>
        public DateCache(string format)
        {
            _formatString=format;
            this.TimeZone = TimeZone.CurrentTimeZone;
            
        }
        
        public DateCache(string format,CultureInfo l)
        {
            _formatString=format;
            _locale = l;
            this.TimeZone = TimeZone.CurrentTimeZone;
        }

        public DateCache(string format, DateTimeFormatInfo s)
        {
            _formatString=format;
            _dfs = s;
            this.TimeZone = TimeZone.CurrentTimeZone;
        }

        #endregion

        /// <summary>
        /// Gets or Sets the timezone.
        /// </summary>
        public TimeZone TimeZone
        {
            set
            {
                setTzFormatString(value);
                if (_locale != null)
                {
                    
                    _tzFormat = new SimpleDateFormat(_tzFormatString, _locale);
                    _minFormat = new SimpleDateFormat(_minFormatString, _locale);
                }
                else if (_dfs != null)
                {
                    _tzFormat = new SimpleDateFormat(_tzFormatString, _dfs);
                    _minFormat = new SimpleDateFormat(_minFormatString, _dfs);
                }
                else
                {
                    _tzFormat = new SimpleDateFormat(_tzFormatString);
                    _minFormat = new SimpleDateFormat(_minFormatString);
                }
                _tzFormat.setTimeZone(value);
                _minFormat.setTimeZone(value);
                _lastSeconds = -1;
                _lastMinutes = -1;   

            }

            get
            {
                return _tzFormat.getTimeZone();
            }
        }

        
        /// <summary>
        /// Set the timezone. by using Timezone ID
        /// </summary>
        public string TimeZoneID
        {
            set
            {
                this.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(value);
            }
        }
        
        /* ------------------------------------------------------------ */
        private void setTzFormatString(TimeZone tz )
        {
            int zIndex = _formatString.indexOf( "ZZZ" );
            if( zIndex >= 0 )
            {
                string ss1 = _formatString.substring( 0, zIndex );
                string ss2 = _formatString.substring( zIndex+3 );
                int tzOffset = tz.getRawOffset();
                
                StringBuilder sb = new StringBuilder(_formatString.length()+10);
                sb.append(ss1);
                sb.append("'");
                if( tzOffset >= 0 )
                    sb.append( '+' );
                else
                {
                    tzOffset = -tzOffset;
                    sb.append( '-' );
                }
                
                int raw = tzOffset / (1000*60);		// Convert to seconds
                int hr = raw / 60;
                int min = raw % 60;
                
                if( hr < 10 )
                    sb.append( '0' );
                sb.append( hr );
                if( min < 10 )
                    sb.append( '0' );
                sb.append( min );
                sb.append( '\'' );
                
                sb.append(ss2);
                _tzFormatString=sb.toString();            
            }
            else
                _tzFormatString=_formatString;
            setMinFormatString();
        }

        
        /* ------------------------------------------------------------ */
        private void setMinFormatString()
        {
            int i = _tzFormatString.indexOf("ss.SSS");
            int l = 6;
            if (i>=0)
                throw new IllegalStateException("ms not supported");
            i = _tzFormatString.indexOf("ss");
            l=2;
            
            // Build a formatter that formats a second format string
            string ss1=_tzFormatString.substring(0,i);
            string ss2=_tzFormatString.substring(i+l);
            _minFormatString =ss1+"'ss'"+ss2;
        }

        
        /// <summary>
        /// Format a date according to our stored formatter.
        /// </summary>
        /// <param name="inDate">date to format</param>
        /// <returns>returns a formatted date</returns>
        public string Format(DateTime inDate)
        {
            lock(_thisLock)
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
                long seconds = inDate / 1000;

                // Is it not suitable to cache?
                if (seconds < _lastSeconds ||
                    _lastSeconds > 0 && seconds > _lastSeconds + __hitWindow)
                {
                    // It's a cache miss
                    Date d = new Date(inDate);
                    return _tzFormat.format(d);

                }

                // Check if we are in the same second
                // and don't care about millis
                if (_lastSeconds == seconds)
                    return _lastResult;

                Date d = new Date(inDate);

                // Check if we need a new format string
                long minutes = seconds / 60;
                if (_lastMinutes != minutes)
                {
                    _lastMinutes = minutes;
                    _secFormatString = _minFormat.format(d);

                    int i = _secFormatString.indexOf("ss");
                    int l = 2;
                    _secFormatString0 = _secFormatString.substring(0, i);
                    _secFormatString1 = _secFormatString.substring(i + l);
                }

                // Always format if we get here
                _lastSeconds = seconds;
                StringBuilder sb = new StringBuilder(_secFormatString.length());
                sb.append(_secFormatString0);
                int s = (int)(seconds % 60);
                if (s < 10)
                    sb.append('0');
                sb.append(s);
                sb.append(_secFormatString1);
                _lastResult = sb.toString();


                return _lastResult;
            }
        }

        /* ------------------------------------------------------------ */
        /** Format to string buffer. 
         * @param inDate Date the format
         * @param buffer StringBuilder
         */
        public void format(long inDate, StringBuilder buffer)
        {
            buffer.append(format(inDate));
        }
        
        /* ------------------------------------------------------------ */
        /** Get the format.
         */
        public DateTimeFormatInfo DateTimeFormat
        {
            get { return _minFormat; }
        }

        /* ------------------------------------------------------------ */
        public string getFormatString()
        {
            return _formatString;
        }    

        /* ------------------------------------------------------------ */
        public string now()
        {
            long now=System.currentTimeMillis();
            _lastMs=(int)(now%1000);
            return format(now);
        }

        /* ------------------------------------------------------------ */
        public int lastMs()
        {
            return _lastMs;
        }

    }
}
