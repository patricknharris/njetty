using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NJetty.Util.Component
{
    public interface IContainerListener
    {
        void AddBean(object bean);
        void RemoveBean(object bean);
        void Add(ContainerRelationship relationship);
        void Remove(ContainerRelationship relationship);
    }
}
