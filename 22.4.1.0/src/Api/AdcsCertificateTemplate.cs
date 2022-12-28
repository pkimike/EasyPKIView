using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.DirectoryServices;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView {
    public class AdcsCertificateTemplate : AdcsDirectoryEntry {
        static readonly Dictionary<String, EnhancedKeyUsageFlags> _enhancedKeyUsageMap = new Dictionary<String, EnhancedKeyUsageFlags>() {
            {"1.3.6.1.4.1.311.10.12.1"  , EnhancedKeyUsageFlags.Any},
            {"1.3.6.1.4.1.311.20.1"     , EnhancedKeyUsageFlags.CTLAutoenrollment},
            {"1.3.6.1.4.1.311.10.5.1"   , EnhancedKeyUsageFlags.DRMSigning},
            {"1.3.6.1.4.1.311.21.19"    , EnhancedKeyUsageFlags.DSEmailReplication},
            {"1.3.6.1.4.1.311.10.3.4.1" , EnhancedKeyUsageFlags.EFSDataRecovery},
            {"1.3.6.1.4.1.311.10.3.8"   , EnhancedKeyUsageFlags.WindowsNTEmbeddedCryptography},
            {"1.3.6.1.4.1.311.20.2.1"   , EnhancedKeyUsageFlags.CertificateEnrollmentAgent},
            {"1.3.6.1.5.5.8.2.2"        , EnhancedKeyUsageFlags.IKEIntermediate},
            {"1.3.6.1.4.1.311.21.5"     , EnhancedKeyUsageFlags.CAExchange},
            {"1.3.6.1.4.1.311.10.3.1"   , EnhancedKeyUsageFlags.CTLSigning},
            {"1.3.6.1.4.1.311.10.3.12"  , EnhancedKeyUsageFlags.DocumentSigning},
            {"1.3.6.1.4.1.311.10.3.4"   , EnhancedKeyUsageFlags.EFS},
            {"1.3.6.1.4.1.311.10.3.11"  , EnhancedKeyUsageFlags.KeyRecovery},
            {"1.3.6.1.4.1.311.21.6"     , EnhancedKeyUsageFlags.KeyRecoveryAgent},
            {"1.3.6.1.4.1.311.10.3.13"  , EnhancedKeyUsageFlags.LifetimeSigning},
            {"1.3.6.1.4.1.311.10.3.10"  , EnhancedKeyUsageFlags.QualifiedSubordination},
            {"1.3.6.1.4.1.311.20.2.2"   , EnhancedKeyUsageFlags.SmartcardLogon},
            {"1.3.6.1.4.1.311.10.3.2"   , EnhancedKeyUsageFlags.TimestampSigning},
            {"1.3.6.1.4.1.311.10.6.2"   , EnhancedKeyUsageFlags.LicenseServerVerification},
            {"1.3.6.1.4.1.311.10.6.1"   , EnhancedKeyUsageFlags.KeyPackLicenses},
            {"1.3.6.1.4.1.311.10.3.7"   , EnhancedKeyUsageFlags.OemWindowsSystemComponentVerification},
            {"1.3.6.1.5.5.7.3.2"        , EnhancedKeyUsageFlags.ClientAuthentication},
            {"1.3.6.1.5.5.7.3.3"        , EnhancedKeyUsageFlags.CodeSigning},
            {"1.3.6.1.5.5.7.3.4"        , EnhancedKeyUsageFlags.SecureEmail},
            {"1.3.6.1.5.5.7.3.5"        , EnhancedKeyUsageFlags.IPSec},
            {"1.3.6.1.5.5.7.3.6"        , EnhancedKeyUsageFlags.IPSecTunnel},
            {"1.3.6.1.5.5.7.3.7"        , EnhancedKeyUsageFlags.IPSecUser},
            {"1.3.6.1.5.5.7.3.9"        , EnhancedKeyUsageFlags.OCSPResponseSigning},
            {"1.3.6.1.5.5.7.3.1"        , EnhancedKeyUsageFlags.ServerAuthentication},
            {"1.3.6.1.5.5.7.3.8"        , EnhancedKeyUsageFlags.PKITimestampSigning},
            {"1.3.6.1.4.1.311.10.3.9"   , EnhancedKeyUsageFlags.CertificateRootListSigning},
            {"1.3.6.1.4.1.311.10.3.5"   , EnhancedKeyUsageFlags.WindowsHardwareDriverVerification}

        };

        /// <summary>
        /// Default constructor
        /// </summary>
        public AdcsCertificateTemplate() { }

        public AdcsCertificateTemplate(DirectoryEntry dEntry) : base(AdcsObjectType.CertificateTemplate, dEntry) {
            if (ObjectType == AdcsObjectType.None) {
                throw new CertificateTemplateNotFoundException();
            }
        }
        public AdcsCertificateTemplate(String name) : base(AdcsObjectType.CertificateTemplate, LdapUrls.GetCertificateTemplateLdapUrl(name)) {
            if (ObjectType == AdcsObjectType.CertificateTemplate) {
                throw new CertificateTemplateNotFoundException(name);
            }
        }
        /// <summary>
        /// The object ID of the certificate template
        /// </summary>
        public String Oid { get; set; }
        /// <summary>
        /// The major of this certificate template
        /// </summary>
        public Int32 MajorRevision { get; set; }
        /// <summary>
        /// The minor of this certificate template
        /// </summary>
        public Int32 MinorRevision { get; set; }
        /// <summary>
        /// The certificate template version. Version 1 = Compatible with Windows 2000 and up, Version 2 = WS2003 and up, and so on.
        /// </summary>
        public Int32 SchemaVersion { get; set; }
        /// <summary>
        /// The list of key usages asserted by certificates issued using this certificate template
        /// </summary>
        public X509KeyUsageFlags KeyUsages { get; set; }
        /// <summary>
        /// The enhanced key usages asserted by the certificate template
        /// </summary>
        public EnhancedKeyUsageFlags EnhancedKeyUsages { get; set; } = EnhancedKeyUsageFlags.None;
        /// <summary>
        /// Critical extensions asserted by the template
        /// </summary>
        public List<Oid> CriticalExtensions { get; set; }
        /// <summary>
        /// Enhanced key usages which are custom to the current Active Directory Forest
        /// </summary>
        public List<String> CustomEnahncedKeyUsages { get; set; } = new List<String>();
        /// <summary>
        /// The number of certificate request agents a certificate request must be signed by in order to obtain a certificate using this template
        /// </summary>
        public Int32 RASignaturesRequired { get; set; }
        /// <summary>
        /// The minimum key size of the public key enforced by this certificate template
        /// </summary>
        public Int32 MinimumPublicKeyLength { get; set; }
        /// <summary>
        /// The amount of time for which certificates issued from this template are valid
        /// </summary>
        public TimeSpan ValidityPeriod { get; set; }
        /// <summary>
        /// Private key flags
        /// </summary>
        public PrivateKeyFlags PrivateKeyFlags { get; set; }
        /// <summary>
        /// General certificate template flags
        /// </summary>
        public CertificateTemplateFlags GeneralFlags { get; set; }
        /// <summary>
        /// Certificate template subject name flags
        /// </summary>
        public CertificateTemplateNameFlags SubjectNameFlags { get; set; }

        public CertificateTemplateEnrollmentFlags EnrollmentFlags { get; set; }
        /// <summary>
        /// Key attestation methods
        /// </summary>
        public KeyAttestationMethodFlags KeyAttestationMethods { get; set; }
        /// <summary>
        /// Indicates whether the certificate template requires the CA to archive the subject's private key
        /// </summary>
        public Boolean KeyArchivalRequired { get; set; }
        /// <summary>
        /// Indicates whether the private key associated with certificates issued from this template
        /// should be exportable from the host on which they were created (only meaningful on Windows clients)
        /// </summary>
        public Boolean ExportablePrivateKey { get; set; }
        /// <summary>
        /// Indicates whether this certificate template should enforce strong private key protection.
        /// See https://tinyurl.com/y25l2h8p for more details.
        /// </summary>
        public Boolean RequiresStrongKeyProtection { get; set; }

        public KeyAttestationEnforcement KeyAttestationEnforcement { get; set; }
        /// <summary>
        /// Indicates whether this certificate template requires the CA to assert a TPM Key Attestation issuance policy OID on issued certificates
        /// </summary>
        public Boolean AssertsKeyAttestationPolicy { get; set; }
        /// <summary>
        /// Minimum Client OS that can request against this template
        /// </summary>
        public String MinimumSupportedClient { get; set; }
        /// <summary>
        /// Minimum Server OS where ADCS can issue against this template
        /// </summary>
        public String MinimumSupportedServer { get; set; }
        /// <summary>
        /// The transient Ids of the issuing CAs to which this template is assigned.
        /// </summary>
        public List<Guid> AssignedEnrollmentServices;

        /// <summary>
        /// The collection of AD identity principals that have rights on the certificate template along with the specification of what those rights are
        /// </summary>
        public Dictionary<String, CertificateTemplateAccessFlags> AccessRules { get; set; } = new Dictionary<String, CertificateTemplateAccessFlags>();

        void setEnhancedKeyUsageFlags() {
            Object[] ekus = (Object[])DirEntry.Properties[DsPropertyIndex.EKU]?.Value;
            if (ekus is null) {
                return;
            }

            foreach (Object eku in ekus) {
                var oid = (String)eku;
                if (_enhancedKeyUsageMap.ContainsKey(oid)) {
                    EnhancedKeyUsages |= _enhancedKeyUsageMap[oid];
                } else {
                    CustomEnahncedKeyUsages.Add(oid);
                }
            }
        }

        void setPropertiesFromDirectoryObject() {
            SchemaVersion = Convert.ToInt32(DirEntry.Properties[DsPropertyIndex.Version].Value);
            KeyUsages = (X509KeyUsageFlags)DirEntry.Properties[DsPropertyIndex.KeyUsage].Value;
            RASignaturesRequired = Convert.ToInt32(DirEntry.Properties[DsPropertyIndex.RASignaturesRequired].Value);
            MinimumPublicKeyLength = Convert.ToInt32(DirEntry.Properties[DsPropertyIndex.MinimumKeySize].Value);
            Oid = DirEntry.Properties[DsPropertyIndex.OID].Value.ToString();
            ValidityPeriod = ((Byte[])DirEntry.Properties[DsPropertyIndex.ValidityPeriod].Value).ToTimeSpan();
            GeneralFlags = ((CertificateTemplateFlags)DirEntry.Properties[DsPropertyIndex.CertTemplateGeneralFlags].Value);
            SubjectNameFlags = ((CertificateTemplateNameFlags)DirEntry.Properties[DsPropertyIndex.CertTemplateSubjectNameFlags].Value);
            EnrollmentFlags = ((CertificateTemplateEnrollmentFlags)DirEntry.Properties[DsPropertyIndex.CertTemplateEnrollmentFlags].Value);
        }

        void setCalculatedProperties() {
            MajorRevision = Convert.ToInt32(DirEntry.Properties[DsPropertyIndex.CertTemplateMajorRevision].Value);
            MinorRevision = Convert.ToInt32(DirEntry.Properties[DsPropertyIndex.CertTemplateMinorRevision].Value);
            KeyArchivalRequired = (PrivateKeyFlags & PrivateKeyFlags.RequireKeyArchival) > 0;
            ExportablePrivateKey = (PrivateKeyFlags & PrivateKeyFlags.AllowKeyExport) > 0;
            RequiresStrongKeyProtection = (PrivateKeyFlags & PrivateKeyFlags.RequireStrongProtection) > 0;
            if ((PrivateKeyFlags & PrivateKeyFlags.AttestationRequired) > 0) {
                KeyAttestationEnforcement = KeyAttestationEnforcement.Required;
            } else if ((PrivateKeyFlags & PrivateKeyFlags.AttestationPreferred) > 0) {
                KeyAttestationEnforcement = KeyAttestationEnforcement.Preferred;
            }

            if (KeyAttestationEnforcement > 0) {
                if ((PrivateKeyFlags & PrivateKeyFlags.TrustOnUse) > 0) {
                    KeyAttestationMethods |= KeyAttestationMethodFlags.TrustOnUse;
                }
                if ((PrivateKeyFlags & PrivateKeyFlags.ValidateCert) > 0) {
                    KeyAttestationMethods |= KeyAttestationMethodFlags.ValidateCert;
                }
                if ((PrivateKeyFlags & PrivateKeyFlags.ValidateKey) > 0) {
                    KeyAttestationMethods |= KeyAttestationMethodFlags.ValidateKey;
                }

                AssertsKeyAttestationPolicy = (PrivateKeyFlags & PrivateKeyFlags.AttestationWithoutPolicy) == 0;
            }
            setMinimumSupportedClient();
            setMinimumSupportedServer();
        }
        void setMinimumSupportedClient() {
            const Int32 mask = 0x0F000000;

            PrivateKeyFlags result = PrivateKeyFlags & (PrivateKeyFlags)mask;
            MinimumSupportedClient = result switch {
                PrivateKeyFlags.None        => getLegacyClientSupport(),
                PrivateKeyFlags.ClientXP    => "Windows XP / Windows Server 2003",
                PrivateKeyFlags.ClientVista => "Windows Vista / Windows Server 2008",
                PrivateKeyFlags.ClientWin7  => "Windows 7 / Windows Server 2008 R2",
                PrivateKeyFlags.ClientWin8  => "Windows 8 / Windows Server 2012",
                PrivateKeyFlags.ClientWin81 => "Windows 8.1 / Windows Server 2012 R2",
                PrivateKeyFlags.ClientWin10 => "Windows 10 / Windows Server 2016",
                _                           => "Unknown"
            };
        }
        String getLegacyClientSupport() {
            return SchemaVersion switch {
                1 => "Windows 2000",
                2 => "Windows XP",
                3 => "Windows Vista",
                4 => "Windows 8",
                _ => "Unknown"
            };
        }
        void setMinimumSupportedServer() {
            const Int32 mask = 0x000F0000;
            PrivateKeyFlags result = PrivateKeyFlags & (PrivateKeyFlags)mask;

            MinimumSupportedServer = result switch {
                PrivateKeyFlags.None         => getLegacyServerSupport(),
                PrivateKeyFlags.Server2003   => "Windows Server 2003",
                PrivateKeyFlags.Server2008   => "Windows Server 2008",
                PrivateKeyFlags.Server2008R2 => "Windows Server 2008 R2",
                PrivateKeyFlags.Server2012   => "Windows Server 2012",
                PrivateKeyFlags.Server2012R2 => "Windows Server 2012 R2",
                PrivateKeyFlags.Server2016   => "Windows Server 2016",
                                           _ => "Unknown"
            };
        }
        String getLegacyServerSupport() {
            return SchemaVersion switch {
                1 => "Windows 2000 Server",
                2 => "Windows Server 2003",
                3 => "Windows Server 2008",
                4 => "Windows Serve 2012",
                _ => "Unknown"
            };
        }
        void setAccessRules() {
            try {
                ActiveDirectorySecurity sd = DirEntry.ObjectSecurity;
                foreach (ActiveDirectoryAccessRule rule in sd.GetAccessRules(true, true, typeof(NTAccount))) {
                    String identity = rule.IdentityReference?.ToString();
                    if (identity is null) {
                        continue;
                    }
                    CertificateTemplateAccessFlags accessMask = getAccessFlag(rule);
                    if (AccessRules.ContainsKey(identity)) {
                        AccessRules[identity] |= accessMask;
                    } else {
                        AccessRules.Add(identity, accessMask);
                    }
                }
            } catch (Exception ex) {
                throw new CertificateTemplateAccessRuleException(this, ex);
            }
        }
        CertificateTemplateAccessFlags getAccessFlag(ActiveDirectoryAccessRule accessRule) {
            ActiveDirectoryRights rights = accessRule.ActiveDirectoryRights;
            if ((rights & ActiveDirectoryRights.GenericAll) > 0) {
                return CertificateTemplateAccessFlags.FullControl;
            }

            var retValue = CertificateTemplateAccessFlags.None;
            if ((rights & ActiveDirectoryRights.GenericRead) > 0) {
                retValue |= CertificateTemplateAccessFlags.Read;
            }

            if ((rights & ActiveDirectoryRights.ExtendedRight) > 0) {
                switch (accessRule.ObjectType.ToString()) {
                    case AdcsExtendedRightId.Enroll:
                        retValue |= CertificateTemplateAccessFlags.Enroll;
                        break;
                    case AdcsExtendedRightId.AutoEnroll:
                        retValue |= CertificateTemplateAccessFlags.Autoenroll;
                        break;
                }
            }

            if ((rights & ActiveDirectoryRights.GenericWrite) > 0) {
                retValue |= CertificateTemplateAccessFlags.Write;
            }

            return retValue;
        }
    }
}
