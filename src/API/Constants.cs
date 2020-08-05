using System.ComponentModel;

namespace EasyPKIView
{
    /// <summary>
    /// Key Attestation Type Enumeration.  
    /// See https://tinyurl.com/y9c6oxnp for more information
    /// </summary>
    public enum KeyAttestationType
    {
        /// <summary>
        /// None. Key Attestation not supported by this certificate template
        /// </summary>
        [Description(@"None")]
        None = 0x0,

        /// <summary>
        /// An authenticating user is allowed to vouch for a valid TPM by specifying their domain credentials.
        /// </summary>
        [Description(@"Account Credentials")]
        AccountCredentials = 0x200,

        /// <summary>
        /// The EKCert of the device must validate through administrator-managed TPM intermediate CA certificates to an administrator-managed root CA certificate.
        /// </summary>
        [Description(@"Manufacturer Signing Certificate")]
        SigningCertificate = 0x400,

        /// <summary>
        /// The EKPub of the device must appear in the PKI administrator-managed list.
        /// </summary>
        [Description(@"Manufacturer Pre-Shared Key")]
        PreSharedKey = 0x800
    }

    internal static class PropertyIndex
    {
        internal const string OID = @"msPKI-Cert-Template-OID";
        internal const string Version = @"msPKI-Template-Schema-Version";
        internal const string EKU = @"pKIExtendedKeyUsage";
        internal const string KeyUsage = @"pKIKeyUsage";
        internal const string Name = @"name";
        internal const string DisplayName = @"displayName";
        internal const string WhenCreated = @"whenCreated";
        internal const string WhenChanged = @"whenChanged";
        internal const string ObjectGUID = @"objectGUID";
        internal const string MinimumKeySize = @"msPKI-Minimal-Key-Size";
        internal const string RASignaturesRequired = @"msPKI-RA-Signature";
        internal const string ObjectClass = @"objectClass";
        internal const string CACertificate = @"cACertificate";
        internal const string DistinguishedName = @"distinguishedName";
        internal const string Flags = @"flags";
        internal const string DNSHostName = @"dNSHostName";
        internal const string CACertificateDN = @"cACertificateDN";
        internal const string CertificateTemplates = @"certificateTemplates";
        internal const string ValidityPeriod = @"pKIExpirationPeriod";
        internal const string PrivateKeyFlags = @"msPKI-Private-Key-Flag";
    }

    internal static class ObjectClass
    {
        internal const string PKIEnrollmentService = @"pKIEnrollmentService";
        internal const string PKICertificateTemplate = @"pKICertificateTemplate";
    }

    internal static class CertificateTemplateFlag
    {
        internal const int RequirePrivateKeyArchival = 0x1;
        internal const int ExportablePrivateKey = 0x10;
        internal const int StrongKeyProtectionRequired = 0x20;
        internal const int RequireAlternateSignatureAlgorithm = 0x40;
        internal const int RequireSameKeyRenewal = 0x80;
        internal const int UseLegacyProvider = 0x100;
        internal const int KeyAttestationRequired = 0x2000;
        internal const int KeyAttestationPreferred = 0x1000;
        internal const int AllowKeyAttestationWithoutPolicyAssertion = 0x4000;
    }

    internal static class Constants
    {
        internal const string UnrecognizedExtension = @"Unrecognized Extension";
        internal const string CertificateTemplateExtensionOid = @"1.3.6.1.4.1.311.21.7";
    }
}
