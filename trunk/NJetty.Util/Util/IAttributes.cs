using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NJetty.Util.Util
{
    public interface IAttributes
    {
        void RemoveAttribute(string name);
        void SetAttribute(string name, object attribute);
        object GetAttribute(string name);
        Enumerable GetAttributeNames();
        void ClearAttributes();
    }
}
