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
                if (c == 0)
                {
                    return default(T);
                }

                T item = this[c - 1];
                this.Remove(item);
                return item;
            }

        }

        public int Enqueue(T item)
        {
            lock (_thisLock)
            {
                this.Add(item);
                return this.Count;
            }
        }

        public void Remove(T item)
        {
            lock (_thisLock)
            {
                base.Remove(item);
            }
        }

        public int Count
        {
            get
            {
                lock (_thisLock)
                {
                    return base.Count;
                }
            }
        }

    }
}
