using System.Collections.Generic;
using System.Linq;
using System.DirectoryServices;

namespace EasyPKIView
{
    /// <summary>
    /// Describes an X509 Extended Key Usage.
    /// See https://tinyurl.com/y34dpt24 for more details
    /// </summary>
    public class ExtendedKeyUsage
    {
        /// <summary>
        /// The OID (Object Identifier) of the EKU
        /// </summary>
        public string OID { get; private set; }

        /// <summary>
        /// The name of the EKU
        /// </summary>
        public string Name { get; private set; }

        private ExtendedKeyUsage(string oid, string name)
        {
            OID = oid;
            Name = name;
        }

        internal ExtendedKeyUsage(string oid)
        {
            OID = oid;
            Name = Constants.UnrecognizedExtension;
        }

        #region Supported EKUs

        /// <summary>
        /// Any - Certificate contains no EKUs and is unrestricted.
        /// </summary>
        public static readonly ExtendedKeyUsage Any = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.12.1", @"Any Application Policy");

        /// <summary>
        /// The certificate can be used to sign a request for automatic enrollment in a certificate trust list (CTL). 
        /// </summary>
        public static readonly ExtendedKeyUsage CTLAutoenrollment = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.20.1", @"Certificate Trust List Autoenrollment");

        /// <summary>
        /// The certificate can be used for digital rights management applications.
        /// </summary>
        public static readonly ExtendedKeyUsage DRMSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.5.1", @"Digital Rights Management Signing");

        /// <summary>
        /// The certificate can be used for Directory Service email replication.
        /// </summary>
        public static readonly ExtendedKeyUsage DSEmailReplication = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.21.19", @"Directory Services Email Replication");

        /// <summary>
        /// The certificate can be used for recovery of documents protected by using Encrypting File System (EFS).
        /// </summary>
        public static readonly ExtendedKeyUsage EFSDataRecovery = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.4.1", @"EFS Data Recovery");

        /// <summary>
        /// The certificate can be used for Windows NT Embedded cryptography.
        /// </summary>
        public static readonly ExtendedKeyUsage WindowsNTEmbeddedCryptography = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.8", @"Windows NT Embedded Cryptography");

        /// <summary>
        /// The certificate can be used by an enrollment agent.
        /// </summary>
        public static readonly ExtendedKeyUsage CertificateEnrollmentAgent = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.20.2.1", @"Certificate Enrollment Agent");

        /// <summary>
        /// The certificate can be used for Internet Key Exchange (IKE).
        /// </summary>
        public static readonly ExtendedKeyUsage IKEIntermediate = new ExtendedKeyUsage(@"1.3.6.1.5.5.8.2.2", @"IKE Intermediate");

        /// <summary>
        /// 	The certificate can be used for archiving a private key on a certification authority.
        /// </summary>
        public static readonly ExtendedKeyUsage CAExchange = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.21.5", @"CA Exchange");

        /// <summary>
        /// The certificate can be used to sign a Certificate Trust List (CTL).
        /// </summary>
        public static readonly ExtendedKeyUsage CTLSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.1", @"Microsoft Trust List Signing");

        /// <summary>
        /// The certificate can be used for signing documents.
        /// </summary>
        public static readonly ExtendedKeyUsage DocumentSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.12", @"Document Signing");

        /// <summary>
        /// The certificate can be used to encrypt files by using the Encrypting File System (EFS).
        /// </summary>
        public static readonly ExtendedKeyUsage EFS = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.4", @"Encrypting File System");

        /// <summary>
        /// 	The certificate can be used to encrypt and recover escrowed keys.
        /// </summary>
        public static readonly ExtendedKeyUsage KeyRecovery = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.11", @"Key Recovery");

        /// <summary>
        /// The certificate is used to identify a key recovery agent.
        /// </summary>
        public static readonly ExtendedKeyUsage KeyRecoveryAgent = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.21.6", @"Key Recovery Agent");

        /// <summary>
        /// Limits the validity period of a signature to the validity period of the certificate. This restriction is typically used with the XCN_OID_PKIX_KP_CODE_SIGNING OID value to indicate that new time stamp semantics should be used.
        /// </summary>
        public static readonly ExtendedKeyUsage LifetimeSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.13", @"Lifetime Signing");

        /// <summary>
        /// The certificate can be used to sign cross certificate and subordinate certification authority certificate requests. Qualified subordination is implemented by applying basic constraints, certificate policies, and application policies. Cross certification typically requires policy mapping.
        /// </summary>
        public static readonly ExtendedKeyUsage QualifiedSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.10", @"Qualified Signing");

        /// <summary>
        /// The certificate enables an individual to log on to a computer by using a smart card.
        /// </summary>
        public static readonly ExtendedKeyUsage SmartcardLogon = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.20.2.2", @"Smartcard Logon");

        /// <summary>
        /// The certificate can be used to sign a time stamp to be added to a document. Time stamp signing is typically part of a time stamping service.
        /// </summary>
        public static readonly ExtendedKeyUsage TimestampSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.2", @"Timestamp Signing");

        /// <summary>
        /// The certificate can be used by a license server when transacting with Microsoft to receive licenses for Terminal Services clients
        /// </summary>
        public static readonly ExtendedKeyUsage LicenseServer = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.6.2", @"License Server");

        /// <summary>
        /// The certificate can be used for key pack licenses.
        /// </summary>
        public static readonly ExtendedKeyUsage KeyPackLicenses = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.6.1", @"Key Pack Licenses");

        /// <summary>
        /// The certificate can be used for Windows Server 2003, Windows XP, and Windows 2000 cryptography.
        /// </summary>
        public static readonly ExtendedKeyUsage LegacyWindowsCryptography = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.7", @"Legacy Windows Cryptography");

        /// <summary>
        /// The certificate can be used for used for Original Equipment Manufacturers (OEM) Windows Hardware Quality Labs (WHQL) cryptography.
        /// </summary>
        public static readonly ExtendedKeyUsage OEMandWHQLCryptography = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.7", @"OEM and WHQL Cryptography");

        /// <summary>
        /// The certificate can be used for authenticating a client.
        /// </summary>
        public static readonly ExtendedKeyUsage ClientAuthentication = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.2", @"Client Authentication");

        /// <summary>
        /// The certificate can be used for signing code.
        /// </summary>
        public static readonly ExtendedKeyUsage CodeSigning = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.3", @"Code Signing");

        /// <summary>
        /// The certificate can be used to encrypt email messages.
        /// </summary>
        public static readonly ExtendedKeyUsage SecureEmail = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.4", @"Secure Email");

        /// <summary>
        /// The certificate can be used for signing end-to-end Internet Protocol Security (IPSEC) communication.
        /// </summary>
        public static readonly ExtendedKeyUsage IPSec = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.5", @"IPSec (End-to-End)");

        /// <summary>
        /// The certificate can be used for singing IPSEC communication in tunnel mode.
        /// </summary>
        public static readonly ExtendedKeyUsage IPSecTunnel = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.6", @"IPSec (tunnel mode)");

        /// <summary>
        /// The certificate can be used for an IPSEC user.
        /// </summary>
        public static readonly ExtendedKeyUsage IPSecUser = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.7", @"IPSec User");

        /// <summary>
        /// The certificate can be used for Online Certificate Status Protocol (OCSP) signing.
        /// </summary>
        public static readonly ExtendedKeyUsage OCSPResponseSigning = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.9", @"OCSP Response Signing");

        /// <summary>
        /// The certificate can be used for server authentication.
        /// </summary>
        public static readonly ExtendedKeyUsage ServerAuthentication = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.1", @"Server Authentication");

        /// <summary>
        /// The certificate can be used for signing public key infrastructure timestamps.
        /// </summary>
        public static readonly ExtendedKeyUsage PKITimestampSigning = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.8", @"PKI Timestamp Signing");

        /// <summary>
        /// The certificate can be used sign a certificate root list.
        /// </summary>
        public static readonly ExtendedKeyUsage CertificateRootListSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.9", @"Certificate Root List Signing");

        /// <summary>
        /// The certificate can be used for Windows Hardware Quality Labs (WHQL) cryptography.
        /// </summary>
        public static readonly ExtendedKeyUsage WHQLCryptography = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.5", @"WHQL Cryptography");

        /// <summary>
        /// The list of extended key usages supported by default.
        /// </summary>
        public static List<ExtendedKeyUsage> Supported { get; private set; } = new List<ExtendedKeyUsage> { Any, CTLAutoenrollment, DRMSigning, DSEmailReplication, EFSDataRecovery, WindowsNTEmbeddedCryptography, CertificateEnrollmentAgent, IKEIntermediate, CAExchange, CTLSigning, DocumentSigning, EFS, KeyRecovery, KeyRecoveryAgent, LifetimeSigning, QualifiedSigning, SmartcardLogon, TimestampSigning, LicenseServer, KeyPackLicenses, LegacyWindowsCryptography, OEMandWHQLCryptography, ClientAuthentication, CodeSigning, SecureEmail, IPSec, IPSecTunnel, IPSecUser, OCSPResponseSigning, ServerAuthentication, PKITimestampSigning, CertificateRootListSigning, WHQLCryptography };

        #endregion

        /// <summary>
        /// Allows for the addition of custom EKUs
        /// </summary>
        /// <param name="oid">The OID of the custom EKU to be added to the Supported collection</param>
        /// <param name="name">The name of the custom EKU to be added to the Supported collection</param>
        public static void AddSupported(string oid, string name)
        {
            if (Supported.FirstOrDefault(p => p.OID.Matches(oid)) != null | Supported.FirstOrDefault(p => p.Name.Matches(name)) != null)
            {
                throw new ExtendedKeyUsageAlreadyExistsException(oid, name);
            }

            Supported.Add(new ExtendedKeyUsage(oid, name));
        }

        internal static List<ExtendedKeyUsage> GetEKUs(DirectoryEntry Template)
        {
            switch (Template.Properties[PropertyIndex.EKU].Count)
            {
                case 0:
                    return new List<ExtendedKeyUsage>();
                case 1:
                    string oid = Template.Properties[PropertyIndex.EKU].Value.ToString();
                    return new List<ExtendedKeyUsage> { FindByOid(oid) };
                default:
                    List<ExtendedKeyUsage> Result = new List<ExtendedKeyUsage>();
                    object[] EKUs = (object[])Template.Properties[PropertyIndex.EKU].Value;
                    for (int x=0; x<EKUs.Length; x++)
                    {
                        Result.Add(FindByOid((string)EKUs[x]));
                    }
                    return Result;
            }
        }

        private static ExtendedKeyUsage FindByOid(string oid)
        {
            ExtendedKeyUsage eku = Supported.FirstOrDefault(p => p.OID.Matches(oid));
            if (eku == null)
            {
                return new ExtendedKeyUsage(oid);
            }
            else
            {
                return eku;
            }
        }
    }
}
