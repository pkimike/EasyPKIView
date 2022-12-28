using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView {
    static class DsPropertyIndex {
        internal const String OID                            = "msPKI-Cert-Template-OID";
        internal const String Version                        = "msPKI-Template-Schema-Version";
        internal const String EKU                            = "pKIExtendedKeyUsage";
        internal const String KeyUsage                       = "pKIKeyUsage";
        internal const String Name                           = "name";
        internal const String DisplayName                    = "displayName";
        internal const String WhenCreated                    = "whenCreated";
        internal const String WhenChanged                    = "whenChanged";
        internal const String ObjectGUID                     = "objectGUID";
        internal const String MinimumKeySize                 = "msPKI-Minimal-Key-Size";
        internal const String RASignaturesRequired           = "msPKI-RA-Signature";
        internal const String ObjectClass                    = "objectClass";
        internal const String CACertificate                  = "cACertificate";
        internal const String DistinguishedName              = "distinguishedName";
        internal const String Flags                          = "flags";
        internal const String DNSHostName                    = "dNSHostName";
        internal const String CACertificateDN                = "cACertificateDN";
        internal const String CertificateTemplates           = "certificateTemplates";
        internal const String ValidityPeriod                 = "pKIExpirationPeriod";
        internal const String PrivateKeyFlags                = "msPKI-Private-Key-Flag";
        internal const String CertTemplateGeneralFlags       = "flags";
        internal const String CertTemplateEnrollmentFlags    = "msPKI-Enrollment-Flag";
        internal const String CertTemplateSubjectNameFlags   = "msPKI-Certificate-Name-Flag";
        internal const String CertTemplateCriticalExtensions = "pKICriticalExtensions";
        internal const String CertTemplateApplicationPolicy  = "msPKI-Certificate-Application-Policy";
        internal const String CertTemplateMinorRevision      = "msPKI-Template-Minor-Revision";
        internal const String CertTemplateMajorRevision      = "revision";
    }
}
