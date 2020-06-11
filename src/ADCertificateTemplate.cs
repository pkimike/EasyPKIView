using System;
using System.Collections.Generic;
using System.DirectoryServices;

namespace EasyPKIView
{
    public class ADCertificateTemplate : ADCSDirectoryEntry
    {
        public string Oid { get; private set; }
        public int Version { get; private set; }
        public List<KeyUsage> KeyUsages { get; private set; }
        public List<ExtendedKeyUsage> ExtendedKeyUsages { get; private set; }
        public int RASignaturesRequired { get; private set; }
        public int MinimumKeySize { get; private set; }

        public ADCertificateTemplate(string name)
            : base(LdapUrls.CertificateTemplate(name), ObjectClass.PKICertificateTemplate)
        {
            if (!Usable)
            {
                throw new CertificateTemplateNotFoundException(name);
            }
            SetFieldsFromDirectoryObject();
        }

        public ADCertificateTemplate(DirectoryEntry TemplateEntry)
            : base(TemplateEntry, ObjectClass.PKICertificateTemplate)
        {
            if (!Usable)
            {
                throw new CertificateTemplateNotFoundException();
            }
            SetFieldsFromDirectoryObject();
        }

        private void SetFieldsFromDirectoryObject()
        {
            Version = Convert.ToInt32(DirEntry.Properties[PropertyIndex.Version].Value);
            ExtendedKeyUsages = ExtendedKeyUsage.GetEKUs(DirEntry);
            KeyUsages = KeyUsage.GetKeyUsages((byte[])DirEntry.Properties[PropertyIndex.KeyUsage].Value);
            RASignaturesRequired = (int)DirEntry.Properties[PropertyIndex.RASignaturesRequired].Value;
            MinimumKeySize = (int)DirEntry.Properties[PropertyIndex.MinimumKeySize].Value;
        }

        public static List<ADCertificateTemplate> GetAll()
        {
            List<ADCertificateTemplate> Collection = new List<ADCertificateTemplate>();
            ADCertificateTemplate Template;

            using (DirectoryEntry TemplatesContainer = new DirectoryEntry(LdapUrls.CertificateTemplatesContainer))
            {
                foreach(DirectoryEntry TemplateEntry in TemplatesContainer.Children)
                {
                    try
                    {
                        Template = new ADCertificateTemplate(TemplateEntry);
                        Collection.Add(Template);
                    }
                    catch (CertificateTemplateNotFoundException)
                    {
                        //This directory entry is not a certificate template object.
                    }
                }
            }

            return Collection;
        }
    }
}
