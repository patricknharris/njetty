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
using System.Timers;
using System.IO;
using System.Globalization;
using System.Configuration;
using NJetty.Util.Logging;

namespace NJetty.Util.Util
{

    /// <summary>
    /// RolloverFileOutputStream
    /// 
    /// This output stream puts content in a file that is rolled over every 24 hours.
    /// The filename must include the string "yyyy_mm_dd", which is replaced with the 
    /// 
    /// actual date when creating and rolling over the file.
    /// Old files are retained for a number of days before being deleted.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    public class RolloverFileOutputStream : FilterOutputStream
    {

        static Timer __rollover;

        const string YYYY_MM_DD = "yyyy_mm_dd";

        string _fileBackupFormat;
        string _fileDateFormat;
        TimeZoneInfo _timeZoneInfo;

        string _filename;
        FileInfo _file;
        bool _append;
        int _retainDays;

        object _thisLock = new object();
        static object _classLock = new object();

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">The filename must include the string "yyyy_mm_dd", which is replaced with the actual date when creating and rolling over the file.</param>
        public RolloverFileOutputStream(string filename)
            : this(filename, true, int.Parse(ConfigurationManager.AppSettings["ROLLOVERFILE_RETAIN_DAYS"] ?? "31"))
        {
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">The filename must include the string "yyyy_mm_dd", which is replaced with the actual date when creating and rolling over the file.</param>
        /// <param name="append">If true, existing files will be appended to.</param>
        /// <exception cref="IOException"></exception>
        public RolloverFileOutputStream(string filename, bool append)
            : this(filename, append, int.Parse(ConfigurationManager.AppSettings["ROLLOVERFILE_RETAIN_DAYS"] ?? "31"))
        {

        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">The filename must include the string "yyyy_mm_dd", which is replaced with the actual date when creating and rolling over the file.</param>
        /// <param name="append">If true, existing files will be appended to.</param>
        /// <param name="retainDays">The number of days to retain files before deleting them.  0 to retain forever.</param>
        /// <exception cref="IOException"></exception>
        public RolloverFileOutputStream(string filename,
                                        bool append,
                                        int retainDays)
            : this(filename, append, retainDays, TimeZoneInfo.Local)
        {

        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">The filename must include the string "yyyy_mm_dd", which is replaced with the actual date when creating and rolling over the file.</param>
        /// <param name="append">If true, existing files will be appended to.</param>
        /// <param name="retainDays">The number of days to retain files before deleting them. 0 to retain forever.</param>
        /// <param name="zone"></param>
        /// <exception cref="IOException"></exception>
        public RolloverFileOutputStream(string filename,
                                        bool append,
                                        int retainDays,
                                        TimeZoneInfo zone)
            : this(filename, append, retainDays, zone, null, null)
        {


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">The filename must include the string "yyyy_mm_dd", which is replaced with the actual date when creating and rolling over the file.</param>
        /// <param name="append">If true, existing files will be appended to.</param>
        /// <param name="retainDays">The number of days to retain files before deleting them. 0 to retain forever.</param>
        /// <param name="zone"></param>
        /// <param name="dateFormat">The format for the date file substitution. If null the system property ROLLOVERFILE_DATE_FORMAT is used and if that is null, then default is "yyyy_MM_dd".</param>
        /// <param name="backupFormat">The format for the file extension of backup files. If null the system property ROLLOVERFILE_BACKUP_FORMAT is used and if that is null, then default is "HHmmssSSS".</param>
        /// <exception cref="IOException">s</exception>
        public RolloverFileOutputStream(string filename,
                                        bool append,
                                        int retainDays,
                                        TimeZoneInfo zone,
                                        string dateFormat,
                                        string backupFormat)
            : base(null)
        {
            if (dateFormat == null)
                dateFormat = ConfigurationManager.AppSettings["ROLLOVERFILE_DATE_FORMAT"] ?? "yyyy_MM_dd";
            _fileDateFormat = dateFormat;
            if (backupFormat == null)
                backupFormat = ConfigurationManager.AppSettings["ROLLOVERFILE_BACKUP_FORMAT"] ?? "HHmmssSSS";
            _fileBackupFormat = backupFormat;

            _timeZoneInfo = zone;

            if (filename != null)
            {
                filename = filename.Trim();
                if (filename.Length == 0)
                    filename = null;
            }
            if (filename == null)
                throw new ArgumentException("Invalid filename");

            _filename = filename;
            _append = append;
            _retainDays = retainDays;
            SetFile();

            lock (_classLock)
            {
                if (__rollover == null)
                    __rollover = new Timer();

                __rollover.Elapsed += RollTaskDelegate;
                DateTime now = DateTime.Now;
                if (_timeZoneInfo != null)
                {
                    now = TimeZoneInfo.ConvertTime(now, _timeZoneInfo);
                }
                DateTime midnight = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
                __rollover.Interval = midnight.Ticks - now.Ticks;
                __rollover.Start();
            }
        }

        #endregion

        public string FullFilename
        {
            get { return _filename; }
        }

        public string DatedFilename
        {
            get
            {
                if (_file == null)
                    return null;
                return _file.ToString();
            }
        }

        public int RetainDays
        {
            get
            {
                return _retainDays;
            }
        }

        void SetFile()
        {
            lock (_thisLock)
            {
                // Check directory
                FileInfo file = new FileInfo(_filename);
                _filename = file.FullName;
                file = new FileInfo(_filename);
                FileInfo dir = new FileInfo(file.Directory.FullName);
                if (!dir.IsDirectory() || !dir.CanWrite())
                    throw new IOException("Cannot write log directory " + dir);

                DateTime now = DateTime.Now;
                if (_timeZoneInfo != null)
                {
                    now = TimeZoneInfo.ConvertTime(now, _timeZoneInfo);
                }

                // Is this a rollover file?
                string filename = file.Name;
                int i = filename.ToLower().IndexOf(YYYY_MM_DD);
                if (i >= 0)
                {
                    file = new FileInfo(Path.Combine(dir.FullName,
                                  filename.Substring(0, i) +
                                  now.ToString(_fileDateFormat) +
                                  filename.Substring(i + YYYY_MM_DD.Length)));
                }

                if (file.Exists && !file.CanWrite())
                    throw new IOException("Cannot write log file " + file);

                // Do we need to change the output stream?
                if (output == null || !file.Equals(_file))
                {
                    // Yep
                    _file = file;
                    if (!_append && file.Exists)
                    {
                        file.MoveTo(new FileInfo(file.ToString() + "." + now.ToString(_fileBackupFormat)).FullName);
                    }
                    Stream oldOut = output;
                    output = new FileStream(
                        file.FullName, _append
                        ? (FileMode.Append | FileMode.CreateNew)
                        : (FileMode.Truncate | FileMode.CreateNew),
                        FileAccess.Write);
                    if (oldOut != null)
                        oldOut.Close();
                    //if(log.isDebugEnabled())log.debug("Opened "+_file);
                }
            }
        }

        void RemoveOldFiles()
        {
            if (_retainDays > 0)
            {
                DateTime nowDT = DateTime.Now;
                if (_timeZoneInfo != null)
                {
                    nowDT = TimeZoneInfo.ConvertTime(nowDT, _timeZoneInfo);
                }

                long now = nowDT.Ticks;

                FileInfo file = new FileInfo(_filename);
                DirectoryInfo dir = file.Directory;
                string fn = file.Name;
                int s = fn.ToLower().IndexOf(YYYY_MM_DD);
                if (s < 0)
                    return;
                string prefix = fn.Substring(0, s);
                string suffix = fn.Substring(s + YYYY_MM_DD.Length);

                FileInfo[] logList = dir.GetFiles();
                for (int i = 0; i < logList.Length; i++)
                {
                    fn = logList[i].Name;
                    if (fn.StartsWith(prefix) && fn.IndexOf(suffix, prefix.Length) >= 0)
                    {
                        FileInfo f = new FileInfo(Path.Combine(dir.FullName, fn));
                        long date = f.LastWriteTime.Ticks;
                        if (((now - date) / (1000 * 60 * 60 * 24) * 1000) > _retainDays)
                            f.Delete();
                    }
                }
            }
        }

       
        public override void Close()
        {
            lock (_classLock)
            {
                try { base.Close(); }
                finally
                {
                    output = null;
                    _file = null;
                }

                //_rollTask.cancel(); 
                __rollover.Stop();
            }
        }

        public void RollTaskDelegate(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (__rollover.Interval != ((1000L * 60 * 60 * 24) * 1000))
                {
                    __rollover.Interval = (1000L * 60 * 60 * 24) * 1000;
                }
                SetFile();
                RemoveOldFiles();
            }
            catch (IOException ioe)
            {
                Log.Warn(ioe);
            }
        }
    }
}
