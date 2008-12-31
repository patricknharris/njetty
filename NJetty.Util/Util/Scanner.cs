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
using System.IO;
using NJetty.Util.Component;
using System.Timers;
using NJetty.Util.Logging;

namespace NJetty.Util.Util
{

    /// <summary>
    /// Scanner, Utility for scanning a directory for added, removed and changed
    /// files and reporting these events via registered Listeners.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    public class Scanner : AbstractLifeCycle
    {
        int _scanInterval;
        new List<Listener> _listeners = new List<Listener>();
        Dictionary<string, long> _prevScan = new Dictionary<string, long>();
        Dictionary<string, long> _currentScan = new Dictionary<string, long>();
        IFilenameFilter _filter;
        List<FileInfo> _scanDirs;
        bool _reportExisting = true;
        Timer _timer;
        bool _recursive=true;

        object _thisLock = new object();

        /// <summary>
        /// Listener 
        /// Marker for notifications re file changes.
        /// </summary>
        public interface Listener
        {
        }

        
        public interface DiscreteListener : Listener
        {
            void FileChanged (string filename);
            void FileAdded (string filename);
            void FileRemoved (string filename);
        }
        
        
        public interface BulkListener : Listener
        {
            void FilesChanged (List<string> filenames);
        }


        
        public Scanner ()
        {       
        }

        /// <summary>
        /// Get or Set Scan Interval
        /// </summary>
        public int ScanInterval
        {
            get{return _scanInterval;}
            set
            {
                lock(_thisLock)
                {
                    _scanInterval = value;
                    Schedule();
                }
            }
        }

        

        /// <summary>
        /// Set the location of the directory to scan.
        /// </summary>
        [Obsolete("Use ScanDirs instead")]
        public DirectoryInfo ScanDir
        {
            get
            {
                return (_scanDirs==null?null:new DirectoryInfo(_scanDirs[0].FullName));
            }

            set
            {
                _scanDirs = new List<FileInfo>();
                _scanDirs.Add(new FileInfo(value.FullName));
            }
        }

        public List<FileInfo> ScanDirs
        {
            get { return _scanDirs;  }
            set { _scanDirs = value;}
        }
        
        public bool Recursive
        {
            get
            { 
                return _recursive;
            }
            set
            {
                _recursive = value;
            }
        }
        
        /// <summary>
        /// Gets or Sets a filter to files found in the scan directory.
        /// Only files matching the filter will be reported as added/changed/removed.
        /// </summary>

        public IFilenameFilter FilenameFilter
        {
            get{ return _filter; }
            set{ _filter = value; }
        }


        /// <summary>
        /// Whether or not an initial scan will report all files as being added.
        /// if true, all files found on initial scan will be  reported as being added, otherwise not
        /// </summary>
        public bool ReportExistingFilesOnStartup
        {
            set{this._reportExisting = value;}
        }

        /// <summary>
        /// Add an added/removed/changed listener 
        /// </summary>
        /// <param name="listener"></param>
        public void AddListener (Listener listener)
        {
            lock(_thisLock)
            {
                if (listener == null)
                    return;
                _listeners.Add(listener);   
            }
        }


        /// <summary>
        /// Remove a registered listener 
        /// </summary>
        /// <param name="listener">the Listener to be removed</param>
        public void RemoveListener (Listener listener)
        {
            lock(_thisLock)
            {
                if (listener == null)
                    return;
                _listeners.Remove(listener);    
            }
        }


        /// <summary>
        /// Start the scanning action. 
        /// </summary>
        protected override void DoStart ()
        {
            lock (_thisLock)
            {
                if (_reportExisting)
                {
                    // if files exist at startup, report them
                    Scan();
                }
                else
                {
                    //just register the list of existing files and only report changes
                    ScanFiles();
                    foreach (string item in _currentScan.Keys)
	                {
                        _prevScan.Add(item, _currentScan[item]);
	                } 
                }
                Schedule();
            }
        }

        public void TimerTaskDelegate (object sender, ElapsedEventArgs e)
        {
            Scan();
        }

       
        
        public void Schedule()
        {  
            if (IsRunning)
            {
                if (_timer != null)
                {
                    _timer.Stop();
                }

                if (ScanInterval > 0)
                {
                    _timer = new Timer();
                    _timer.Elapsed += TimerTaskDelegate;
                    _timer.Interval = ScanInterval * 1000;
                    _timer.Start();
                }
            }
        }

        /// <summary>
        /// Stop the scanning. 
        /// </summary>
        protected override void DoStop()
        {
            lock(_thisLock)
            {
                if (_timer!=null)
                    _timer.Stop();
                _timer=null;
            }
            
        }

        /// <summary>
        /// Perform a pass of the scanner and report changes 
        /// </summary>
        public void Scan ()
        {
            ScanFiles();
            ReportDifferences(_currentScan, _prevScan);
            _prevScan.Clear();

            if (_currentScan != null)
            {
                foreach (string key in _currentScan.Keys)
                {
                    if (IsStopping || IsStopped)
                    {
                        // stop if no longer running
                        return;
                    }
                    _prevScan.Add(key, _currentScan[key]);
                }
            }
        }

        
        /// <summary>
        /// Recursively scan all files in the designated directories. 
        /// returns the Map of name of file to last modified time
        /// </summary>
        public void ScanFiles ()
        {
            if (_scanDirs==null)
                return;
            
            _currentScan.Clear();

            foreach (FileInfo dir in _scanDirs)
            {

                if (IsStopping || IsStopped)
                {
                    // stop if no longer running
                    return;
                }

                if (dir != null && dir.Exists)
                {
                    ScanFile(dir, _currentScan);
                }
            }
        }


        /// <summary>
        /// Report the adds/changes/removes to the registered listeners
        /// </summary>
        /// <param name="currentScan">the info from the most recent pass</param>
        /// <param name="oldScan">info from the previous pass</param>
        public void ReportDifferences(Dictionary<string, long> currentScan, Dictionary<string, long> oldScan) 
        {
            List<string> bulkChanges = new List<string>();

            HashSet<string> oldScanKeys = new HashSet<string>(oldScan.Keys);

            foreach (string key in currentScan.Keys)
            {

                if (IsStopping || IsStopped)
                {
                    // stop if no longer running
                    return;
                }


                if (!oldScanKeys.Contains(key))
                {
                    Log.Debug("File added: " + key);
                    ReportAddition(key);
                    bulkChanges.Add(key);
                }
                else if (!oldScan[key].Equals(currentScan[key]))
                {
                    Log.Debug("File changed: " + key);
                    ReportChange(key);
                    oldScanKeys.Remove(key);
                    bulkChanges.Add(key);
                }
                else
                {
                    oldScanKeys.Remove(key);
                }
            }

            if (oldScanKeys.Count > 0)
            {
                foreach (string filename in oldScanKeys)
                {
                    Log.Debug("File removed: " + filename);
                    ReportRemoval(filename);
                    bulkChanges.Add(filename);

                }
            }
            
            if (bulkChanges.Count > 0)
                ReportBulkChanges(bulkChanges);
        }


        /// <summary>
        /// Get last modified time on a single file or recurse if
        /// the file is a directory. 
        /// </summary>
        /// <param name="f">file or directory</param>
        /// <param name="scanInfoMap">map of filenames to last modified times</param>
        void ScanFile(FileInfo f, Dictionary<string, long> scanInfoMap)
        {

            if (IsStopping || IsStopped)
            {
                // stop if no longer running
                return;
            }

            try
            {
                if (!f.Exists)
                    return;

                if (!f.IsDirectory())
                {
                    if ((_filter == null) || ((_filter != null) && _filter.Accept(f.Directory, f.Name)))
                    {
                        string name = f.FullName;
                        long lastModified = f.LastWriteTimeUtc.Ticks;
                        scanInfoMap.Add(name, lastModified);
                    }
                }
                else if (f.IsDirectory() && (_recursive || _scanDirs.Contains(f)))
                {
                    FileInfo[] files = (new DirectoryInfo(f.FullName)).GetFiles();
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (IsStopping || IsStopped)
                        {
                            // stop if no longer running
                            return;
                        }
                        ScanFile(files[i], scanInfoMap);
                    }
                }
            }
            catch (IOException e)
            {
                Log.Warn("Error scanning watched files", e);
            }
        }

        void Warn(object listener,string filename,Exception th)
        {
            Log.Warn(th);
            Log.Warn(listener+" failed on '"+filename);
        }

        /// <summary>
        /// Report a file addition to the registered FileAddedListeners 
        /// </summary>
        /// <param name="filename"></param>
        void ReportAddition (string filename)
        {
            foreach (object l in _listeners)
            {
                try
                {
                    if (l is DiscreteListener)
                        ((DiscreteListener)l).FileAdded(filename);
                }
                catch (SystemException e)
                {
                    Warn(l, filename, e);
                }
                catch (Exception e)
                {
                    Warn(l, filename, e);
                }

            }
        }


        /// <summary>
        /// Report a file removal to the FileRemovedListeners 
        /// </summary>
        /// <param name="filename"></param>
        void ReportRemoval (string filename)
        {

            foreach (object l in _listeners)
            {
                try
                {
                    if (l is DiscreteListener)
                        ((DiscreteListener)l).FileRemoved(filename);
                }
                catch (SystemException e)
                {
                    Warn(l, filename, e);
                }
                catch (Exception e)
                {
                    Warn(l, filename, e);
                }

            }
        }


        /// <summary>
        /// Report a file change to the FileChangedListeners
        /// </summary>
        /// <param name="filename"></param>
        void ReportChange (string filename)
        {

            foreach (object l in _listeners)
            {
                try
                {
                    if (l is DiscreteListener)
                        ((DiscreteListener)l).FileChanged(filename);
                }
                catch (SystemException e)
                {
                    Warn(l, filename, e);
                }
                catch (Exception e)
                {
                    Warn(l, filename, e);
                }

            }
        }
        
        void ReportBulkChanges (List<string> filenames)
        {
            
            foreach (object l in _listeners)
            {
                try
                {
                    if (l is BulkListener)
                        ((BulkListener)l).FilesChanged(filenames);
                }
                catch (SystemException e)
                {
                    Warn(l, filenames.ToString(), e);
                }
                catch (Exception e)
                {
                    Warn(l, filenames.ToString(), e);
                }

            }


        }
    }
}
