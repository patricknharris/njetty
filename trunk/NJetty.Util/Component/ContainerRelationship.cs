using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NJetty.Util.Component
{
    public class ContainerRelationship
    {
        private object _parent;
        private object _child;
        private string _relationship;
        private Container _container;
        
        internal ContainerRelationship(Container container, object parent,object child, string relationship)
        {
            _container=container;
            _parent=parent;
            _child=child;
            _relationship=relationship;
        }
        
        public Container Container
        {
            get { return _container; }
        }
        
        public object Child
        {
            get{return _child;}
        }
        
        public object Parent
        {
            get{return _parent;}
        }
        
        public string Relationship
        {
            get{return _relationship;}
        }
        

        public override string  ToString()
        {
 	         return _parent+"---"+_relationship+"-->"+_child;
        }

        public override int  GetHashCode()
        {
 	         return _parent.GetHashCode()+_child.GetHashCode()+_relationship.GetHashCode();
        }
        
        

        public override bool  Equals(object o)
        {
 	         if (o==null || o.GetType() != typeof(ContainerRelationship))
             {
                return false;
             }
             ContainerRelationship r = (ContainerRelationship)o;
             return r._parent==_parent && r._child==_child && r._relationship.Equals(_relationship);
        }
    }
}
