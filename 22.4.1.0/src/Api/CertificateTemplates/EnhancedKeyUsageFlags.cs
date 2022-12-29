namespace EasyPKIView.CertificateTemplates;

/// <summary>
/// Defines flags that determine how certificate subject is constructed.
/// <para>This enumeration has a <see cref="FlagsAttribute"/> attribute that allows a bitwise combination of its member values.</para>
/// </summary>
[Flags]
public enum EnhancedKeyUsageFlags : Int64 {
    /// <summary>
    /// None.
    /// </summary>
    None = 0,
    /// <summary>
    /// The certificate can be used for authenticating a client.
    /// </summary>
    ClientAuthentication          = 0x1,
    /// <summary>
    /// The certificate can be used for server authentication.
    /// </summary>
    ServerAuthentication          = 0x2,
    /// <summary>
    /// The certificate can be used to encrypt email messages.
    /// </summary>
    SecureEmail                   = 0x4,
    /// <summary>
    /// The certificate can be used to encrypt files by using the Encrypting File System (EFS).
    /// </summary>
    EFS                           = 0x8,
    /// <summary>
    /// The certificate enables an individual to log on to a computer by using a smart card.
    /// </summary>
    SmartcardLogon                = 0x10,
    /// <summary>
    /// The certificate can be used for signing code.
    /// </summary>
    CodeSigning                   = 0x20,
    /// <summary>
    /// The certificate can be used for Online Certificate Status Protocol (OCSP) signing.
    /// </summary>
    OCSPResponseSigning           = 0x40,
    /// <summary>
    /// 	The certificate can be used for archiving a private key on a certification authority.
    /// </summary>
    CAExchange                    = 0x80,
    /// <summary>
    /// The certificate can be used to sign a Certificate Trust List (CTL).
    /// </summary>
    CTLSigning                    = 0x100,
    /// <summary>
    /// The certificate can be used for signing documents.
    /// </summary>
    DocumentSigning               = 0x200,
    /// <summary>
    /// The certificate can be used for recovery of documents protected by using Encrypting File System (EFS).
    /// </summary>
    EFSDataRecovery               = 0x400,
    /// <summary>
    /// 	The certificate can be used to encrypt and recover escrowed keys.
    /// </summary>
    KeyRecovery                   = 0x800,
    /// <summary>
    /// The certificate is used to identify a key recovery agent.
    /// </summary>
    KeyRecoveryAgent              = 0x1000,
    /// <summary>
    /// Limits the validity period of a signature to the validity period of the certificate. This restriction is typically used with the XCN_OID_PKIX_KP_CODE_SIGNING OID value to indicate that new time stamp semantics should be used.
    /// </summary>
    LifetimeSigning               = 0x2000,
    /// <summary>
    /// The certificate can be used to sign cross certificate and subordinate certification authority certificate requests. Qualified subordination is implemented by applying basic constraints, certificate policies, and application policies. Cross certification typically requires policy mapping.
    /// </summary>
    QualifiedSubordination                = 0x4000,
    /// <summary>
    /// The certificate can be used for Windows NT Embedded cryptography.
    /// </summary>
    WindowsNTEmbeddedCryptography         = 0x8000,
    /// <summary>
    /// The certificate can be used to sign a time stamp to be added to a document. Time stamp signing is typically part of a time stamping service.
    /// </summary>
    TimestampSigning                      = 0x10000,
    /// <summary>
    /// The certificate can be used by a license server when transacting with Microsoft to receive licenses for Terminal Services clients
    /// </summary>
    LicenseServerVerification             = 0x20000,
    /// <summary>
    /// The certificate can be used for key pack licenses.
    /// </summary>
    KeyPackLicenses                       = 0x40000,
    /// <summary>
    /// The certificate can be used for used for Original Equipment Manufacturers (OEM) Windows Hardware Quality Labs (WHQL) cryptography.
    /// </summary>
    OemWindowsSystemComponentVerification = 0x80000,
    /// <summary>
    /// The certificate can be used to sign a request for automatic enrollment in a certificate trust list (CTL). 
    /// </summary>
    CTLAutoenrollment                     = 0x100000,
    /// <summary>
    /// The certificate can be used by an enrollment agent.
    /// </summary>
    CertificateEnrollmentAgent            = 0x200000,
    /// <summary>
    /// The certificate can be used for Directory Service email replication.
    /// </summary>
    DSEmailReplication                    = 0x400000,
    /// <summary>
    /// The certificate can be used for signing end-to-end Internet Protocol Security (IPSEC) communication.
    /// </summary>
    IPSec                                 = 0x800000,
    /// <summary>
    /// The certificate can be used for singing IPSEC communication in tunnel mode.
    /// </summary>
    IPSecTunnel                           = 0x1000000,
    /// <summary>
    /// The certificate can be used for an IPSEC user.
    /// </summary>
    IPSecUser                             = 0x2000000,
    /// <summary>
    /// The certificate can be used for Internet Key Exchange (IKE).
    /// </summary>
    IKEIntermediate                       = 0x4000000,
    /// <summary>
    /// The certificate can be used for digital rights management applications.
    /// </summary>
    DRMSigning                            = 0x8000000,
    /// <summary>
    /// The certificate can be used for signing public key infrastructure timestamps.
    /// </summary>
    PKITimestampSigning                   = 0x10000000,
    /// <summary>
    /// The certificate can be used sign a certificate root list.
    /// </summary>
    CertificateRootListSigning            = 0x20000000,
    /// <summary>
    /// The certificate can be used for Windows Hardware Quality Labs (WHQL) cryptography.
    /// </summary>
    WindowsHardwareDriverVerification     = 0x40000000,
    /// <summary>
    /// The certificate can be used for any purpose
    /// </summary>
    Any                                   = 0x80000000
}
