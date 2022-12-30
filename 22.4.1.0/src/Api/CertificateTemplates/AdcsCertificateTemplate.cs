using System.Data;
using System.DirectoryServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using EasyPKIView.EnrollmentServices;

namespace EasyPKIView.CertificateTemplates {
    /// <summary>
    /// ADCS Certificate Template
    /// </summary>
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

            setProperties();
        }
        public AdcsCertificateTemplate(String name) : base(AdcsObjectType.CertificateTemplate, PublicKeyServicesContainerHelper.GetCertificateTemplateLdapUrl(name)) {
            if (ObjectType == AdcsObjectType.CertificateTemplate) {
                throw new CertificateTemplateNotFoundException(name);
            }

            setProperties();
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
        public KeyUsageExtension KeyUsages { get; set; } = new KeyUsageExtension();
        /// <summary>
        /// The enhanced key usages asserted by the certificate template
        /// </summary>
        public EnhancedKeyUsageExtension EnhancedKeyUsages { get; set; } = new EnhancedKeyUsageExtension();
        /// <summary>
        /// Critical extensions asserted by the template
        /// </summary>
        public List<Oid> CriticalExtensions { get; set; } = new List<Oid>();
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
        /// <summary>
        /// Certificate Template Enrollment flags
        /// </summary>
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
        /// <summary>
        /// If true, the template requires that the enrollee supply the subject information in the certificate request
        /// </summary>
        public Boolean IsOffline { get; set; }
        /// <summary>
        /// Indicates whether or how key attestation is enforced by the template
        /// </summary>
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
        public List<Guid> AssignedEnrollmentServices = new();
        /// <summary>
        /// The collection of AD identity principals that have rights on the certificate template along with the specification of what those rights are
        /// </summary>
        public Dictionary<String, CertificateTemplateAccessFlags> AccessRules { get; set; } = new Dictionary<String, CertificateTemplateAccessFlags>();
        /// <summary>
        /// Sets the list of ids of enrollment services to which this template is assigned.
        /// </summary>
        /// <param name="enrollmentServices"></param>
        public void SetAssignedEnrollmentServices(List<AdcsEnrollmentService> enrollmentServices) {
            if (enrollmentServices is null) {
                return;
            }

            AssignedEnrollmentServices = enrollmentServices.Where(e => e.PublishedCertificateTemplates.Contains(Name))
                .Select(e => e.TransientId).ToList();
        }
        /// <summary>
        /// Gets the list of enrollment services to which this template is assigned.
        /// </summary>
        /// <param name="enrollmentServices">The list of enrollment services published in Active Directory.</param>
        /// <returns>The list of enrollment services to which this template is assigned.</returns>
        public List<AdcsEnrollmentService> GetAssignedEnrollmentServices(List<AdcsEnrollmentService> enrollmentServices) {
            return enrollmentServices.Where(e => AssignedEnrollmentServices.Contains(e.TransientId)).ToList();
        }

        void setProperties() {
            if (ObjectType == AdcsObjectType.None) {
                return;
            }
            setPropertiesFromDirectoryObject();
            setAccessRules();
            setCalculatedProperties();
        }
        void setPropertiesFromDirectoryObject() {
            SchemaVersion = Convert.ToInt32(DirEntry.Properties[DsPropertyName.Version].Value);
            RASignaturesRequired = Convert.ToInt32(DirEntry.Properties[DsPropertyName.RASignaturesRequired].Value);
            MinimumPublicKeyLength = Convert.ToInt32(DirEntry.Properties[DsPropertyName.MinimumKeySize].Value);
            Oid = DirEntry.Properties[DsPropertyName.OID].Value.ToString();
            ValidityPeriod = ((Byte[])DirEntry.Properties[DsPropertyName.ValidityPeriod].Value).ToTimeSpan();
            GeneralFlags = (CertificateTemplateFlags)Convert.ToInt32(DirEntry.Properties[DsPropertyName.CertTemplateGeneralFlags].Value);
            SubjectNameFlags = (CertificateTemplateNameFlags)Convert.ToInt32(DirEntry.Properties[DsPropertyName.CertTemplateSubjectNameFlags].Value);
            EnrollmentFlags = (CertificateTemplateEnrollmentFlags)Convert.ToInt32(DirEntry.Properties[DsPropertyName.CertTemplateEnrollmentFlags].Value);
        }
        void setCalculatedProperties() {
            MajorRevision = Convert.ToInt32(DirEntry.Properties[DsPropertyName.CertTemplateMajorRevision].Value);
            MinorRevision = Convert.ToInt32(DirEntry.Properties[DsPropertyName.CertTemplateMinorRevision].Value);
            KeyArchivalRequired = (PrivateKeyFlags & PrivateKeyFlags.RequireKeyArchival) > 0;
            ExportablePrivateKey = (PrivateKeyFlags & PrivateKeyFlags.AllowKeyExport) > 0;
            RequiresStrongKeyProtection = (PrivateKeyFlags & PrivateKeyFlags.RequireStrongProtection) > 0;
            IsOffline = (SubjectNameFlags & CertificateTemplateNameFlags.EnrolleeSuppliesSubject) > 0;
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
            setCriticalExtensions();
            setKeyUsagesExtension();
            setEnhancedKeyUsageExtension();
            setMinimumSupportedClient();
            setMinimumSupportedServer();
        }
        void setCriticalExtensions() {
            var criticalExtensionOids = GetMultiStringAttribute(DsPropertyName.CertTemplateCriticalExtensions);
            if (criticalExtensionOids is null) {
                return;
            }

            foreach (String criticalExtensionOid in criticalExtensionOids) {
                CriticalExtensions.Add(new Oid(criticalExtensionOid));
            }
        }
        void setKeyUsagesExtension() {
            var keyUsageBytes = (Byte[])DirEntry.Properties[DsPropertyName.KeyUsage].Value;
            if (keyUsageBytes is null) {
                return;
            }
            var keyUsageInt = keyUsageBytes.ToInt32();
            var keyUsageFlags = (X509KeyUsageFlags)keyUsageInt;
            KeyUsages = new KeyUsageExtension(keyUsageFlags) {
                IsCritical = CriticalExtensions.Select(p => p.Value).Contains(X509ExtensionOid.KeyUsage)
            };
        }
        void setEnhancedKeyUsageExtension() {
            List<String> ekus = GetMultiStringAttribute(DsPropertyName.EKU);
            if (ekus is null) {
                return;
            }

            var ekuFlags = EnhancedKeyUsageFlags.None;
            var customEkus = new List<String>();

            foreach (String oid in ekus) {
                if (_enhancedKeyUsageMap.ContainsKey(oid)) {
                    ekuFlags |= _enhancedKeyUsageMap[oid];
                } else {
                    customEkus.Add(oid);
                }
            }

            EnhancedKeyUsages = new EnhancedKeyUsageExtension(ekuFlags) {
                Custom = customEkus,
                IsCritical = CriticalExtensions.Select(p => p.Value).Contains(X509ExtensionOid.EnhancedKeyUsage)
            };
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
                2 => "Windows XP / Windows Server 2003",
                3 => "Windows Vista / Windows Server 2008",
                4 => "Windows 8 / Windows Server 2012",
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
                Console.WriteLine($"Current Template: {Name}");
                ActiveDirectorySecurity sd = DirEntry.ObjectSecurity;
                var denyRules = new List<(String, ActiveDirectoryAccessRule)>();
                foreach (ActiveDirectoryAccessRule rule in sd.GetAccessRules(true, true, typeof(NTAccount))) {
                    String identity = rule.IdentityReference?.ToString();
                    if (identity is null) {
                        continue;
                    }
                    if (rule.AccessControlType == AccessControlType.Deny) {
                        denyRules.Add(new ValueTuple<String, ActiveDirectoryAccessRule>(identity, rule));
                        continue;
                    }
                    CertificateTemplateAccessFlags accessMask = getAccessFlag(rule);
                    if (AccessRules.ContainsKey(identity)) {
                        AccessRules[identity] |= accessMask;
                    } else {
                        AccessRules.Add(identity, accessMask);
                    }
                }
                //Handle any deny rules
                foreach ((String, ActiveDirectoryAccessRule) denyRule in denyRules.Where(denyRule => AccessRules.ContainsKey(denyRule.Item1))) {
                    AccessRules[denyRule.Item1] &= getAccessFlag(denyRule.Item2);
                }
            } catch (Exception ex) {
                throw new CertificateTemplateAccessRuleException(this, ex);
            }
        }
        CertificateTemplateAccessFlags getAccessFlag(ActiveDirectoryAccessRule accessRule) {
            ActiveDirectoryRights rights = accessRule.ActiveDirectoryRights;
            if ((rights & ActiveDirectoryRights.GenericAll) == ActiveDirectoryRights.GenericAll) {
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

        /// <summary>
        /// Gets all certificate templates which exist in the current Active Directory forest.
        /// </summary>
        /// <returns>A list of certificate templates</returns>
        public static List<AdcsCertificateTemplate> GetAllFromDirectory() {
            var retValue = new List<AdcsCertificateTemplate>();
            using DirectoryEntry templatesContainer = new DirectoryEntry(PublicKeyServicesContainerHelper.CertificateTemplatesContainerUrl);
            foreach (DirectoryEntry dEntry in templatesContainer.Children) {
                try {
                    var currentTemplate = new AdcsCertificateTemplate(dEntry);
                    retValue.Add(currentTemplate);
                } catch (CertificateTemplateNotFoundException) {
                    //This directory entry is not a certificate template.
                }
            }

            return retValue;
        }
    }
}
