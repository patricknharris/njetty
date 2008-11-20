using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NJetty.Util.Util;
using NJetty.Util.Logging;

namespace NJetty.Util.Test.Util
{
    [TestFixture]
    public class ArrayQueueTest
    {
        
    
        [Test]
        public void TestWrap()
        {
            ArrayQueue<String> queue = new ArrayQueue<String>(3);
            
            
            Assert.AreEqual(0,queue.Count);

            for (int i=0;i<3;i++)
            {
                queue.Offer("one");
                Assert.AreEqual(1,queue.Count);

                queue.Offer("two");
                Assert.AreEqual(2,queue.Count);

                queue.Offer("three");
                Assert.AreEqual(3,queue.Count);

                Assert.AreEqual("one",queue[0]);
                Assert.AreEqual("two",queue[1]);
                Assert.AreEqual("three",queue[2]);

                


                Assert.AreEqual("[one, two, three]",queue.ToString());

                Assert.AreEqual("one",queue.Dequeue());
                Assert.AreEqual(2,queue.Count);

                Assert.AreEqual("two", queue.Dequeue());
                Assert.AreEqual(1,queue.Count);

                Assert.AreEqual("three", queue.Dequeue());
                Assert.AreEqual(0,queue.Count);


                queue.Offer("xxx");
                Assert.AreEqual(1,queue.Count);
                Assert.AreEqual("xxx", queue.Dequeue());
                Assert.AreEqual(0,queue.Count);



            }

        }
        [Test]
        public void TestRemove()
        {
            ArrayQueue<String> queue = new ArrayQueue<String>(3,3);
           
            queue.Enqueue("0");
            queue.Enqueue("x");
            
            for (int i=1;i<100;i++)
            {
                queue.Enqueue(""+i);
                queue.Enqueue("x");
                queue.RemoveAt(queue.Count-3);
                queue[queue.Count-3] = queue[queue.Count-3]+"!";
            }

            for (int i = 0; i < 99; i++)
            {
                Assert.AreEqual(i + "!", queue[i]);
            }
        }

        public void TestGrow()
        {
            ArrayQueue<String> queue = new ArrayQueue<String>(3, 5);
            Assert.AreEqual(3, queue.Capacity);

            queue.Enqueue("a");
            queue.Enqueue("b");
            Assert.AreEqual(3, queue.Capacity);
            queue.Enqueue("c");
            Assert.AreEqual(8, queue.Capacity);

            for (int i = 0; i < 4; i++)
                queue.Enqueue("" + ('d' + i));
            Assert.AreEqual(8, queue.Capacity);
            for (int i = 0; i < 4; i++)
                queue.Dequeue();
            Assert.AreEqual(8, queue.Capacity);
            for (int i = 0; i < 4; i++)
                queue.Enqueue("" + ('d' + i));
            Assert.AreEqual(8, queue.Capacity);
            for (int i = 0; i < 4; i++)
                queue.Dequeue();
            Assert.AreEqual(8, queue.Capacity);
            for (int i = 0; i < 4; i++)
                queue.Enqueue("" + ('d' + i));
            Assert.AreEqual(8, queue.Capacity);

            queue.Enqueue("z");
            Assert.AreEqual(13, queue.Capacity);

            queue.Clear();
            Assert.AreEqual(13, queue.Capacity);
            for (int i = 0; i < 12; i++)
                queue.Enqueue("" + ('a' + i));
            Assert.AreEqual(13, queue.Capacity);
            queue.Clear();
            Assert.AreEqual(13, queue.Capacity);
            for (int i = 0; i < 12; i++)
                queue.Enqueue("" + ('a' + i));
            Assert.AreEqual(13, queue.Capacity);

        }
    }
}
