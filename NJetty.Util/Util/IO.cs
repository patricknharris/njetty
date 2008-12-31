#region Apache License
//
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership. 
// The ASF licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except input compliance with 
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to input writing, software
// distributed under the License inputStream distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NJetty.Util.Threading;
using NJetty.Util.Logging;
using System.IO;

namespace NJetty.Util.Util
{

    /// <summary>
    /// IO Utilities.
    /// Provides stream handling utilities input
    ///  singleton Threadpool implementation accessed by static members.
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    public class IO
    {
        #region Constants

        public const string CRLF = "\015\012";
        public static readonly byte[] CRLF_BYTES = new byte[] { 13, 10 };
        public const int bufferSize = 2 * 8192;

        #endregion

        #region Singleton Thread Pool

        // TODO get rid of this singleton!
        static class Singleton
        {
            public static readonly QueuedThreadPool __pool = new QueuedThreadPool();
            static Singleton()
            {
                try { __pool.Start(); }
                catch (Exception e) { Log.Warn(e); Environment.Exit(1); }
            }
        }

        #endregion
        
        #region Job Task Class

        class Job
        {
            Stream input;
            Stream output;
            StreamReader read;
            StreamWriter write;

            public Job(Stream input, Stream output)
            {
                this.input = input;
                this.output = output;
                this.read = null;
                this.write = null;
            }
            public Job(StreamReader read, StreamWriter write)
            {
                this.input = null;
                this.output = null;
                this.read = read;
                this.write = write;
            }

            public void TaskDelegate()
            {
                try
                {
                    if (input != null)
                        Copy(input, output, -1);
                    else
                        Copy(read, write, -1);
                }
                catch (IOException e)
                {
                    Log.Ignore(e);
                    try
                    {
                        if (output != null)
                            output.Close();
                        if (write != null)
                            write.Close();
                    }
                    catch (IOException e2)
                    {
                        Log.Ignore(e2);
                    }
                }
            }
        }

        #endregion

        #region Copy Methods

        /// <summary>
        /// Copy Stream input to Stream output until EOF or exception.
        /// </summary>
        /// <param name="input">own thread</param>
        /// <param name="output"></param>
        public static void CopyThread(Stream input, Stream output)
        {
            try
            {
                Job job = new Job(input, output);
                if (!Singleton.__pool.Dispatch(job.TaskDelegate))
                    job.TaskDelegate();
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

        /// <summary>
        /// Copy Stream input to Stream output until EOF or exception.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public static void Copy(Stream input, Stream output)
        {
            Copy(input, output, -1);
        }

        /// <summary>
        /// Copy Stream input to Stream output until EOF or exception
        /// </summary>
        /// <param name="input">own thread</param>
        /// <param name="output"></param>
        public static void CopyThread(StreamReader input, StreamWriter output)
        {
            try
            {
                Job job = new Job(input, output);
                if (!Singleton.__pool.Dispatch(job.TaskDelegate))
                    job.TaskDelegate();
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

        /// <summary>
        /// Copy Reader to Writer output until EOF or exception.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public static void Copy(TextReader input, TextWriter output)
        {
            Copy(input, output, -1);
        }

        /// <summary>
        /// Copy Stream input to Stream for byteCount bytes or until EOF or exception.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="byteCount"></param>
        public static void Copy(Stream input,
                                Stream output,
                                long byteCount)
        {
            byte[] buffer = new byte[bufferSize];
            int len = bufferSize;

            if (byteCount >= 0)
            {
                while (byteCount > 0)
                {
                    int max = byteCount < bufferSize ? (int)byteCount : bufferSize;
                    len = input.Read(buffer, 0, max);

                    if (len == -1)
                        break;

                    byteCount -= len;
                    output.Write(buffer, 0, len);
                }
            }
            else
            {
                while (true)
                {
                    len = input.Read(buffer, 0, bufferSize);
                    if (len < 0)
                        break;
                    output.Write(buffer, 0, len);
                }
            }
        }

        
        /// <summary>
        /// Copy Reader to Writer for byteCount bytes or until EOF or exception.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="byteCount"></param>
        public static void Copy(TextReader input,
                                TextWriter output,
                                long byteCount)
        {
            char[] buffer = new char[bufferSize];
            int len = bufferSize;

            if (byteCount >= 0)
            {
                while (byteCount > 0)
                {
                    if (byteCount < bufferSize)
                        len = input.Read(buffer, 0, (int)byteCount);
                    else
                        len = input.Read(buffer, 0, bufferSize);

                    if (len == -1)
                        break;

                    byteCount -= len;
                    output.Write(buffer, 0, len);
                }
            }
            else
            {
                while (true)
                {
                    len = input.Read(buffer, 0, bufferSize);
                    if (len == -1)
                        break;
                    output.Write(buffer, 0, len);
                }
            }
        }

        /// <summary>
        /// Copy files or directories
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <exception cref="IOException"></exception>
        public static void Copy(FileInfo from, FileInfo to)
        {
            if (from.IsDirectory())
                CopyDir(from, to);
            else
                CopyFile(from, to);
        }

        public static void CopyDir(FileInfo from, FileInfo to)
        {
            DirectoryInfo toDir;
            if (to.Exists)
            {
                if (!to.IsDirectory())
                {
                    throw new ArgumentException(to.ToString());
                }
                toDir = new DirectoryInfo(to.FullName);
            }
            else
            {
                toDir = new DirectoryInfo(to.FullName);
                toDir.Create();
            }



            FileInfo[] files = new DirectoryInfo(from.FullName).GetFiles();
            if (files != null)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    string name = files[i].Name;
                    if (".".Equals(name) || "..".Equals(name))
                        continue;
                    Copy(files[i], new FileInfo(Path.Combine(to.FullName, name)));
                }
            }
        }

        public static void CopyFile(FileInfo from, FileInfo to)
        {
            using (Stream input = from.OpenRead())
            {
                using (Stream output = to.OpenWrite())
                {
                    Copy(input, output);
                }
            }

        }

        #endregion

        #region ToString Methods

        /// <summary>
        /// Read input stream to string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToString(Stream input)
        {
            return ToString(input, null);
        }

        /// <summary>
        /// Read input stream to string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ToString(Stream input, string encoding)
        {
            StringWriter writer = new StringWriter();
            StreamReader reader = encoding == null ? new StreamReader(input) : new StreamReader(input, Encoding.GetEncoding(encoding));
            Copy(reader, writer);
            return writer.ToString();
        }

        /// <summary>
        /// Read input stream to string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToString(StreamReader input)
        {
            StringWriter writer = new StringWriter();
            Copy(input, writer);
            return writer.ToString();
        }

        #endregion
        
        #region Delete Method

        /// <summary>
        /// Delete File.
        /// This delete will recursively delete directories - BE CAREFULL
        /// </summary>
        /// <param name="file">The file to be deleted.</param>
        /// <returns></returns>
        public static bool Delete(FileInfo file)
        {
            if (!file.Exists)
                return false;
            if (file.IsDirectory())
            {
                FileInfo[] files = new DirectoryInfo(file.FullName).GetFiles();
                for (int i = 0; files != null && i < files.Length; i++)
                    Delete(files[i]);
            }

            try
            {
                file.Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Close Method

        /// <summary>
        /// closes an input or output stream, and logs exceptions 
        /// </summary>
        /// <param name="inputStream">the input or output stream to close</param>
        public static void Close(Stream stream)
        {
            try
            {
                if (stream != null)
                    stream.Close();
            }
            catch (IOException e)
            {
                Log.Ignore(e);
            }
        }

        #endregion

        #region Read Bytes Method

        public static byte[] ReadBytes(Stream input)
        {
            MemoryStream bout = new MemoryStream();
            Copy(input, bout);
            return bout.GetBuffer();
        }

        #endregion

        #region Nowhere Stream Properties

        static NullOS __nullStream = new NullOS();

        /// <summary>
        /// Gets an OutputStream to nowhere or so called devnull
        /// </summary>
        public static Stream NullStream
        {
            get { return __nullStream; }
        }


        static ClosedIS __closedStream = new ClosedIS();

        /// <summary>
        /// Gets an OutputStream to nowhere or so called devnull
        /// </summary>
        public static Stream ClosedStream
        {
            get { return __closedStream; }
        }


        static NullWrite __nullWriter = new NullWrite();

        /// <summary>
        /// Gets A Writer to nowhere
        /// </summary>
        public static TextWriter NullWriter
        {
            get { return __nullWriter; }
        }

        #endregion

        #region Null OutputStream Class

        class NullOS : Stream
        {
            public override void Close() { }
            public void Write(byte[] b) { }
            public void Write(int b) { }

            public override bool CanRead
            {
                get { return false; }
            }

            public override bool CanSeek
            {
                get { return false; }
            }

            public override bool CanWrite
            {
                get { return true; }
            }

            public override void Flush()
            { }

            public override long Length
            {
                get { return 0; }
            }

            public override long Position
            {
                get
                {
                    return 0;
                }
                set
                {

                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            { }
        }
        
        #endregion
        
        #region Close InputStream Class


        class ClosedIS : Stream
        {
            public override bool CanRead
            {
                get { return false; }
            }

            public override bool CanSeek
            {
                get { return false; }
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override void Flush()
            { }

            public override long Length
            {
                get { return -1; }
            }

            public override long Position
            {
                get
                {
                    return -1;
                }
                set
                {

                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return -1;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return -1;
            }

            public override void SetLength(long value)
            {

            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }
        }
        

        #endregion

        # region Null Writer Class

        class NullWrite : TextWriter
        {

            public override void Close() { }
            protected override void Dispose(bool disposing) { }
            public override void Flush() { }
            public override void Write(char value) { }
            public override void Write(char[] buffer) { }
            public override void Write(string value) { }
            public override void Write(char[] buffer, int index, int count) { }


            public override Encoding Encoding
            {
                get { return null; }
            }
        }
        
        #endregion
    }
}
