using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NJetty.Util.Component
{
    public interface ILifeCycle
    {

        
        /// <summary>
        /// Starts the component.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the component.
        /// The component may wait for current activities to complete
        /// normally, but it can be interrupted.
        /// </summary>
        void Stop();

        /// <summary>
        /// true if the component is starting or has been started.
        /// </summary>
        bool IsRunning
        {
            get;
        }

        /// <summary>
        /// true if the component has been started.
        /// </summary>
        bool IsStarted
        {
            get;
        }

        /// <summary>
        /// true if the component is starting.
        /// </summary>
        bool IsStarting
        {
            get;
        }

        /// <summary>
        /// true if the component is stopping.
        /// </summary>
        bool IsStopping
        {
            get;
        }

        /// <summary>
        /// true if the component has been stopped.
        /// </summary>
        bool IsStopped
        {
            get;
        }

        /// <summary>
        /// true if the component has failed to start or has failed to stop.
        /// </summary>
        bool IsFailed
        {
            get;
        }

        void addLifeCycleListener(IListener listener);

        void removeLifeCycleListener(IListener listener);

    }
}
