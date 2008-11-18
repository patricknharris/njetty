using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NJetty.Util.Logger;

namespace NJetty.Util.Util
{
    public class Queue<T> : List<T>
    {
        object _thisLock = new object();
        
        public Queue()
            : base()
        {

        }


        public Queue(int capacity) : base(capacity) 
        {
            
        }

        public T Dequeue()
        {
            lock (_thisLock)
            {
                int c = Count;
                T item = this[c - 1];
                this.Remove(item);
                return item;
            }

        }

        public void Enqueue(T item)
        {
            lock (_thisLock)
            {
                this.Add(item);
            }
        }

    }
}
