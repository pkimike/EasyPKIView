namespace EasyPKIView
{
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
    }

    internal static class ObjectClass
    {
        internal const string PKIEnrollmentService = @"pKIEnrollmentService";
        internal const string PKICertificateTemplate = @"pKICertificateTemplate";
    }

    internal static class Constants
    {
        internal static string UnrecognizedExtension = @"Unrecognized Extension";
    }
}
