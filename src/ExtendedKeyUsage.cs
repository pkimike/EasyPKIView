using System.Collections.Generic;
using System.Linq;
using System.DirectoryServices;

namespace EasyPKIView
{
    public class ExtendedKeyUsage
    {
        public string OID { get; private set; }
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

        public static readonly ExtendedKeyUsage Any = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.12.1", @"Any Application Policy");
        public static readonly ExtendedKeyUsage CTLAutoenrollment = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.20.1", @"Certificate Trust List Autoenrollment");
        public static readonly ExtendedKeyUsage DRMSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.5.1", @"Digital Rights Management Signing");
        public static readonly ExtendedKeyUsage DSEmailReplication = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.21.19", @"Directory Services Email Replication");
        public static readonly ExtendedKeyUsage EFSDataRecovery = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.4.1", @"EFS Data Recovery");
        public static readonly ExtendedKeyUsage WindowsNTEmbeddedCryptography = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.8", @"Windows NT Embedded Cryptography");
        public static readonly ExtendedKeyUsage CertificateEnrollmentAgent = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.20.2.1", @"Certificate Enrollment Agent");
        public static readonly ExtendedKeyUsage IKEIntermediate = new ExtendedKeyUsage(@"1.3.6.1.5.5.8.2.2", @"IKE Intermediate");
        public static readonly ExtendedKeyUsage CAExchange = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.21.5", @"CA Exchange");
        public static readonly ExtendedKeyUsage CTLSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.1", @"Microsoft Trust List Signing");
        public static readonly ExtendedKeyUsage DocumentSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.12", @"Document Signing");
        public static readonly ExtendedKeyUsage EFS = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.4", @"Encrypting File System");
        public static readonly ExtendedKeyUsage KeyRecovery = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.11", @"Key Recovery");
        public static readonly ExtendedKeyUsage KeyRecoveryAgent = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.21.6", @"Key Recovery Agent");
        public static readonly ExtendedKeyUsage LifetimeSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.13", @"Lifetime Signing");
        public static readonly ExtendedKeyUsage QualifiedSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.10", @"Qualified Signing");
        public static readonly ExtendedKeyUsage SmartcardLogon = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.20.2.2", @"Smartcard Logon");
        public static readonly ExtendedKeyUsage TimestampSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.2", @"Timestamp Signing");
        public static readonly ExtendedKeyUsage LicenseServer = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.6.2", @"License Server");
        public static readonly ExtendedKeyUsage KeyPackLicenses = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.6.1", @"Key Pack Licenses");
        public static readonly ExtendedKeyUsage LegacyWindowsCryptography = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.7", @"Legacy Windows Cryptography");
        public static readonly ExtendedKeyUsage OEMandWHQLCryptography = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.7", @"OEM and WHQL Cryptography");
        public static readonly ExtendedKeyUsage ClientAuthentication = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.2", @"Client Authentication");
        public static readonly ExtendedKeyUsage CodeSigning = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.3", @"Code Signing");
        public static readonly ExtendedKeyUsage SecureEmail = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.4", @"Secure Email");
        public static readonly ExtendedKeyUsage IPSec = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.5", @"IPSec (End-to-End)");
        public static readonly ExtendedKeyUsage IPSecTunnel = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.6", @"IPSec (tunnel mode)");
        public static readonly ExtendedKeyUsage IPSecUser = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.7", @"IPSec User");
        public static readonly ExtendedKeyUsage OCSPResponseSigning = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.9", @"OCSP Response Signing");
        public static readonly ExtendedKeyUsage ServerAuthentication = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.1", @"Server Authentication");
        public static readonly ExtendedKeyUsage PKITimestampSigning = new ExtendedKeyUsage(@"1.3.6.1.5.5.7.3.8", @"PKI Timestamp Signing");
        public static readonly ExtendedKeyUsage CertificateRootListSigning = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.9", @"Certificate Root List Signing");
        public static readonly ExtendedKeyUsage WHQLCryptography = new ExtendedKeyUsage(@"1.3.6.1.4.1.311.10.3.5", @"WHQL Cryptography");

        public static List<ExtendedKeyUsage> Supported { get; private set; } = new List<ExtendedKeyUsage> { Any, CTLAutoenrollment, DRMSigning, DSEmailReplication, EFSDataRecovery, WindowsNTEmbeddedCryptography, CertificateEnrollmentAgent, IKEIntermediate, CAExchange, CTLSigning, DocumentSigning, EFS, KeyRecovery, KeyRecoveryAgent, LifetimeSigning, QualifiedSigning, SmartcardLogon, TimestampSigning, LicenseServer, KeyPackLicenses, LegacyWindowsCryptography, OEMandWHQLCryptography, ClientAuthentication, CodeSigning, SecureEmail, IPSec, IPSecTunnel, IPSecUser, OCSPResponseSigning, ServerAuthentication, PKITimestampSigning, CertificateRootListSigning, WHQLCryptography };

        #endregion

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
