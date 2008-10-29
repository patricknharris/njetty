using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NJetty.Util.Component
{
    // TODO: remodel this to use Delegates/events
    /// <summary>
    /// Listener.
    /// A listener for Lifecycle events.
    /// </summary>
    public interface IListener
    {
        void lifeCycleStarting(ILifeCycle e);
        void lifeCycleStarted(ILifeCycle e);
        void lifeCycleFailure(ILifeCycle e,Exception cause);
        void lifeCycleStopping(ILifeCycle e);
        void lifeCycleStopped(ILifeCycle e);
    }
}
