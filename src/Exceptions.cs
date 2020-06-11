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

    public class CertificateTemplateNotFoundException : Exception
    {
        internal CertificateTemplateNotFoundException(string name)
            : base($"A certificate template with the name \"{name}\" could not be found in the current Active Directory forest")
        { }

        internal CertificateTemplateNotFoundException()
            : base(@"The specified certificate template directory entry does not exist in the current Active Directory forest")
        { }
    }

    public class CertificationAuthorityNotFoundException : Exception
    {
        internal CertificationAuthorityNotFoundException(string name)
            : base($"A certification authority enrollment services object with the name \"{name}\" could not be found in the current Active Directory forest")
        { }

        internal CertificationAuthorityNotFoundException()
            : base(@"The specified certification authority enrollment services directory entry does not exist in the current Active Directory forest")
        { }
    }
}
