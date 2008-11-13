using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NJetty.Util.Util;
using System.Runtime.Remoting.Contexts;
using NJetty.Util.Logger;

namespace NJetty.Util.Component
{
    
    public class Container
    {
        object _listeners;
        object _lock = new object();
        
        /// <summary>
        /// Add Event Listener
        /// (thread safe)
        /// </summary>
        /// <param name="listener"></param>
        public void AddEventListener(IContainerListener listener)
        {
            lock (_lock)
            {
                _listeners = LazyList.Add(_listeners, listener);
            }
        }

        /// <summary>
        /// Remove Event Listener 
        /// (thread safe)
        /// </summary>
        /// <param name="listener"></param>
        public void RemoveEventListener(IContainerListener listener)
        {
            lock (_lock)
            {
                _listeners = LazyList.Remove(_listeners, listener);
            }
            
        }


        /// <summary>
        /// Update single parent to child relationship.
        /// (thread safe)
        /// </summary>
        /// <param name="parent">The parent of the child.</param>
        /// <param name="oldChild">The previous value of the child.  If this is non null and differs from <code>child</code>, then a remove event is generated.</param>
        /// <param name="child">The current child. If this is non null and differs from <code>oldChild</code>, then an add event is generated.</param>
        /// <param name="relationship">The name of the relationship</param>
        public void Update(object parent, object oldChild, object child, string relationship)
        {
            // TODO: make child parameter readonly

            lock (_lock)
            {
                if (oldChild != null && !oldChild.Equals(child))
                    Remove(parent, oldChild, relationship);
                if (child != null && !child.Equals(oldChild))
                    Add(parent, child, relationship);
            }
            
        }
        
        
        /// <summary>
        /// Update single parent to child relationship.
        /// (thread safe)
        /// </summary>
        /// <param name="parent">The parent of the child.</param>
        /// <param name="oldChild">The previous value of the child.  If this is non null and differs from <code>child</code>, then a remove event is generated.</param>
        /// <param name="child">The current child. If this is non null and differs from <code>oldChild</code>, then an add event is generated.</param>
        /// <param name="relationship">The name of the relationship</param>
        /// <param name="addRemove">If true add/remove is called for the new/old children as well as the relationships</param>
        public void Update(object parent, object oldChild, object child, string relationship, bool addRemove)
        {
            // TODO: make child parameter readonly

            lock (_lock)
            {
                if (oldChild != null && !oldChild.Equals(child))
                {
                    Remove(parent, oldChild, relationship);
                    if (addRemove)
                    {
                        RemoveBean(oldChild);
                    }
                }

                if (child != null && !child.Equals(oldChild))
                {
                    if (addRemove)
                    {
                        AddBean(child);
                    }

                    Add(parent, child, relationship);
                }
            }
        }

        /// <summary>
        /// Update multiple parent to child relationship.
        /// (thread safe)
        /// </summary>
        /// <param name="parent">The parent of the child.</param>
        /// <param name="oldChildren">
        ///     The previous array of children.  A remove event is generated for any child in this array but not in the  <code>children</code> array.
        ///     This array is modified and children that remain in the new children array are nulled out of the old children array.
        /// </param>
        /// <param name="children">The current array of children. An add event is generated for any child in this array but not in the <code>oldChildren</code> array.</param>
        /// <param name="relationship">The name of the relationship</param>
        public void Update(object parent, object[] oldChildren, object[] children, string relationship)
        {
            // TODO: make children parameter readonly
            lock (_lock)
            {
                Update(parent, oldChildren, children, relationship, false);
            }
        }
        
        
        /// <summary>
        /// Update multiple parent to child relationship.
        /// (thread safe)
        /// </summary>
        /// <param name="parent">The parent of the child.</param>
        /// <param name="oldChildren">
        ///     The previous array of children.  A remove event is generated for any child in this array but not in the  <code>children</code> array.
        ///     This array is modified and children that remain in the new children array are nulled out of the old children array.
        /// </param>
        /// <param name="children">The current array of children. An add event is generated for any child in this array but not in the <code>oldChildren</code> array.</param>
        /// <param name="relationship">The name of the relationship</param>
        /// <param name="addRemove">If true add/remove is called for the new/old children as well as the relationships</param>
        public void Update(object parent, object[] oldChildren, object[] children, string relationship, bool addRemove)
        {
            // TODO: make children parameter readonly

            lock (_lock)
            {

                object[] newChildren = null;
                if (children != null)
                {
                    newChildren = new object[children.Length];

                    for (int i = children.Length; i-- > 0; )
                    {
                        bool new_child = true;
                        if (oldChildren != null)
                        {
                            for (int j = oldChildren.Length; j-- > 0; )
                            {
                                if (children[i] != null && children[i].Equals(oldChildren[j]))
                                {
                                    oldChildren[j] = null;
                                    new_child = false;
                                }
                            }
                        }

                        if (new_child)
                        {
                            newChildren[i] = children[i];
                        }
                    }
                }

                if (oldChildren != null)
                {
                    for (int i = oldChildren.Length; i-- > 0; )
                    {
                        if (oldChildren[i] != null)
                        {
                            Remove(parent, oldChildren[i], relationship);
                            
                            if (addRemove)
                            {
                                RemoveBean(oldChildren[i]);
                            }
                        }
                    }
                }

                if (newChildren != null)
                {
                    for (int i = 0; i < newChildren.Length; i++)
                        if (newChildren[i] != null)
                        {
                            if (addRemove)
                            {
                                AddBean(newChildren[i]);
                            }
                            
                            Add(parent, newChildren[i], relationship);
                        }
                }
            }
        }

        #region Add/Remove Bean

        public void AddBean(Object obj)
        {
            if (_listeners != null)
            {
                for (int i = 0; i < LazyList.Size(_listeners); i++)
                {
                    LazyList.Get<IContainerListener>(_listeners, i).AddBean(obj);
                }
            }
        }

        public void RemoveBean(object obj)
        {
            if (_listeners != null)
            {
                for (int i = 0; i < LazyList.Size(_listeners); i++)
                {
                    LazyList.Get<IContainerListener>(_listeners, i).RemoveBean(obj);
                }
            }
        }

        #endregion

        /// <summary>
        /// Add a parent child relationship
        /// </summary>
        /// <param name="parent">parent</param>
        /// <param name="child">child</param>
        /// <param name="relationship">relationship</param>
        private void Add(object parent, object child, string relationship)
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug("Container {0} + {1} as {2}", parent, child, relationship);
            }


            if (_listeners!=null)
            {
                ContainerRelationship eventObject =new ContainerRelationship(this,parent,child,relationship);
                for (int i=0; i<LazyList.Size(_listeners); i++)
                {
                    LazyList.Get<IContainerListener>(_listeners, i).Add(eventObject);
                }
            }
        }
        
        /// <summary>
        /// remove a parent child relationship
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="relationship"></param>
        private void Remove(object parent, object child, string relationship)
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug("Container {0} - {1} as {2}", parent, child, relationship);
            }
            
            if (_listeners!=null)
            {
                ContainerRelationship eventObject =new ContainerRelationship(this,parent,child,relationship);
                for (int i=0; i<LazyList.Size(_listeners); i++)
                {
                    LazyList.Get<IContainerListener>(_listeners, i).Remove(eventObject);
                }
            }
        }





    }
}
