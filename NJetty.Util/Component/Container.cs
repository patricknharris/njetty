using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NJetty.Util.Util;
using System.Runtime.Remoting.Contexts;

namespace NJetty.Util.Component
{
    
    public class Container
    {
        object _listeners;
        object _lock = new object();
        

        public void AddEventListener(IContainerListener listener)
        {
            lock (_lock)
            {
                _listeners = LazyList.Add(_listeners, listener);
            }
        }


        public void RemoveEventListener(IContainerListener listener)
        {
            lock (_lock)
            {
                _listeners = LazyList.Remove(_listeners, listener);
            }
            
        }





    }
}
