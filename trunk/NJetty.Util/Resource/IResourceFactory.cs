using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NJetty.Util.Resource
{
    public interface IResourceFactory
    {
        Resource GetResource(string path);
    }
}
