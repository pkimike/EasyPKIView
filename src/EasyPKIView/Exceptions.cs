using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView
{
    public class ExtendedKeyUsageAlreadyExistsException : Exception
    {
        internal ExtendedKeyUsageAlreadyExistsException(string oid, string name)
            : base($"An extended key usage with the name {name} or object ID {oid} already exists in the supported collection")
        { }
    }
}
