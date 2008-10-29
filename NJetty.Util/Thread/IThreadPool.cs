using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NJetty.Util.Thread
{
    public interface IThreadPool
    {
        // TODO: remove this note, equivalent to public abstract boolean dispatch(Runnable job);
        bool Dispatch(ThreadStart job);

        /// <summary>
        /// Blocks until the thread pool is stopped.
        /// </summary>
        void Join();

        /// <summary>
        /// The total number of threads currently in the pool
        /// </summary>
        // TODO: remove this note: equivalent to int getThreads()
        int Threads
        {
            get;
        }

        /// <summary>
        /// The number of idle threads in the pool
        /// </summary>
        int IdleThreads
        {
            get;
        }

        /// <summary>
        /// True if the pool is low on threads
        /// </summary>
        bool IsLowOnThreads
        {
            get;
        }
    }
}
