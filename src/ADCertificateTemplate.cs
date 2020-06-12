using System;
using System.Linq;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace EasyPKIView
{
    /// <summary>
    /// Describes a certificate template as stored in the "Certificate Templates" container in Active Directory
    /// </summary>
    public class ADCertificateTemplate : ADCSDirectoryEntry
    {
        private static bool loadedAllTemplates = false;
        private static List<ADCertificateTemplate> all;

        /// <summary>
        /// The object ID of the certificate template
        /// </summary>
        public string Oid { get; private set; }

        /// <summary>
        /// The certificate template version. Version 1 = Compatible with Windows 2000 and up, Version 2 = WS2003 and up, and so on.
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// The list of key usages asserted by certificates issued using this certificate template
        /// </summary>
        public List<KeyUsage> KeyUsages { get; private set; }

        /// <summary>
        /// The list of extended key usages (also known as application policies) asserted by certificates issued using this certificate template
        /// </summary>
        public List<ExtendedKeyUsage> ExtendedKeyUsages { get; private set; }

        /// <summary>
        /// The number of certificate request agents a certificate request must be signed by in order to obtain a certificate using this template
        /// </summary>
        public int RASignaturesRequired { get; private set; }

        /// <summary>
        /// The minimum key size of the public key enforced by this certificate template
        /// </summary>
        public int MinimumKeySize { get; private set; }

        /// <summary>
        /// ADCertificateTemplate Constructor 1
        /// </summary>
        /// <param name="name">The Name attribute of the certificate template as indicated in Active Directory</param>
        public ADCertificateTemplate(string name)
            : base(LdapUrls.CertificateTemplate(name), ObjectClass.PKICertificateTemplate)
        {
            if (!Usable)
            {
                throw new CertificateTemplateNotFoundException(name);
            }
            SetFieldsFromDirectoryObject();
        }

        /// <summary>
        /// ADCertificateTemplate Constructor 2
        /// </summary>
        /// <param name="TemplateEntry">The Active Directory entry pointing to this certificate template</param>
        public ADCertificateTemplate(DirectoryEntry TemplateEntry)
            : base(TemplateEntry, ObjectClass.PKICertificateTemplate)
        {
            if (!Usable)
            {
                throw new CertificateTemplateNotFoundException();
            }
            SetFieldsFromDirectoryObject();
        }

        /// <summary>
        /// ADCertificateTemplate Constructor 3
        /// </summary>
        /// <param name="Cert">A certificate object which was issued using the desired certificate template</param>
        public ADCertificateTemplate(X509Certificate2 Cert)
        {
            string templateOid = string.Empty;
            ADCertificateTemplate FoundTemplate;

            foreach(X509Extension Extension in Cert.Extensions)
            {
                if (Extension.Oid.Value.Matches(Constants.CertificateTemplateExtensionOid))
                {
                    templateOid = GetTemplateOidFromCertExtension(Extension);
                    FoundTemplate = LoadAll().Where(p => p.Version > 1)
                                             .FirstOrDefault(p => p.Oid.Matches(templateOid));

                    if (FoundTemplate == null)
                    {
                        throw new CertificateTemplateOidNotFoundException(templateOid);
                    }
                    PropertyCopier<ADCertificateTemplate, ADCertificateTemplate>.Copy(FoundTemplate, this);
                    return;
                }
            }

            throw new CertificateTemplateOidNotFoundException();
        }

        private void SetFieldsFromDirectoryObject()
        {
            Version = Convert.ToInt32(DirEntry.Properties[PropertyIndex.Version].Value);
            ExtendedKeyUsages = ExtendedKeyUsage.GetEKUs(DirEntry);
            KeyUsages = KeyUsage.GetKeyUsages((byte[])DirEntry.Properties[PropertyIndex.KeyUsage].Value);
            RASignaturesRequired = (int)DirEntry.Properties[PropertyIndex.RASignaturesRequired].Value;
            MinimumKeySize = (int)DirEntry.Properties[PropertyIndex.MinimumKeySize].Value;
        }

        /// <summary>
        /// Loads all certificate templates contained in the Active Directory forest
        /// </summary>
        /// <param name="refreshIfCached">If true, list will be generated from AD even if there is already a cached list. Default is false</param>
        /// <returns>A list of ADCertificateTemplate objects</returns>
        public static List<ADCertificateTemplate> LoadAll(bool refreshIfCached = false)
        {
            if (!loadedAllTemplates | refreshIfCached)
            {
                LoadAllWorker();
            }

            return all;
        }

        private static void LoadAllWorker()
        {
            all = new List<ADCertificateTemplate>();
            ADCertificateTemplate Template;

            using (DirectoryEntry TemplatesContainer = new DirectoryEntry(LdapUrls.CertificateTemplatesContainer))
            {
                foreach(DirectoryEntry TemplateEntry in TemplatesContainer.Children)
                {
                    try
                    {
                        Template = new ADCertificateTemplate(TemplateEntry);
                        all.Add(Template);
                    }
                    catch (CertificateTemplateNotFoundException)
                    {
                        //This directory entry is not a certificate template object.
                    }
                }
            }

            loadedAllTemplates = true;
        }

        private static string GetTemplateOidFromCertExtension(X509Extension Extention)
        {
            AsnEncodedData asnData = new AsnEncodedData(Extention.Oid, Extention.RawData);
            string templateInfo = asnData.Format(true);
            return templateInfo.Split(new string[] { @"Template=" }, StringSplitOptions.None)[1].Split('\r')[0];
        }
    }
}
