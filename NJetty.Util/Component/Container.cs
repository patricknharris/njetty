using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NJetty.Util.Util;
using System.Runtime.Remoting.Contexts;
using System.Runtime.CompilerServices;

namespace NJetty.Util.Component
{
    
    public class Container
    {
        object _listeners;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddEventListener(IContainerListener listener)
        {
            _listeners=LazyList.Add(_listeners,listener);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveEventListener(IContainerListener listener)
        {
            _listeners=LazyList.Remove(_listeners,listener);
        }





    }
}
