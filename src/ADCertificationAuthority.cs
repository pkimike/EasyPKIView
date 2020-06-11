using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Cryptography.X509Certificates;

namespace EasyPKIView
{
    public class ADCertificationAuthority : ADCSDirectoryEntry
    {
        public X509Certificate2 CACertificate { get; private set; }
        public bool IsEnterpriseCA { get; private set; }
        public string DNSHostName { get; private set; }
        public string CACertificateDN { get; private set; }
        public List<ADCertificateTemplate> Templates { get; private set; } = new List<ADCertificateTemplate>();

        public bool HasTemplates
        {
            get
            {
                return Templates.Count > 0;
            }
        }

        public ADCertificationAuthority(string name)
            : base(LdapUrls.EnrollmentService(name), ObjectClass.PKIEnrollmentService)
        {
            if (!Usable)
            {
                throw new CertificationAuthorityNotFoundException(name);
            }
            SetFieldsFromDirectoryObject();
        }

        public ADCertificationAuthority(DirectoryEntry CAEntry)
            : base(CAEntry, ObjectClass.PKIEnrollmentService)
        {
            if (!Usable)
            {
                throw new CertificationAuthorityNotFoundException();
            }
            SetFieldsFromDirectoryObject();
        }

        public ADCertificationAuthority(X509Certificate2 CACert)
            : this(CACert.Subject.Replace(@"CN=", string.Empty).Split(',')[0])
        { }

        private void SetFieldsFromDirectoryObject()
        {
            CACertificate = new X509Certificate2((byte[])DirEntry.Properties[PropertyIndex.CACertificate].Value);
            IsEnterpriseCA = (int)DirEntry.Properties[PropertyIndex.Flags].Value == 10;
            DNSHostName = DirEntry.Properties[PropertyIndex.DNSHostName].Value.ToString();
            CACertificateDN = DirEntry.Properties[PropertyIndex.CACertificateDN].Value.ToString();
            GetTemplates(DirEntry);
        }

        private void GetTemplates(DirectoryEntry CAEntry)
        {
            CAEntry.RefreshCache(new string[] { PropertyIndex.CertificateTemplates });
            object[] TemplateNames;

            try
            {
                TemplateNames = (object[])CAEntry.Properties[PropertyIndex.CertificateTemplates].Value;
            }
            catch
            {
                //If there's only a single template, it won't cast as an array automatically.
                TemplateNames = new object[] { CAEntry.Properties[PropertyIndex.CertificateTemplates].Value };
            }

            if (TemplateNames != null)
            {
                for (int x=0; x<TemplateNames.Length; x++)
                {
                    try
                    {
                        Templates.Add(new ADCertificateTemplate(Templates[x].ToString()));
                    }
                    catch (CertificateTemplateNotFoundException)
                    {
                        //Won't include any templates that don't resolve to a valid certificate template directory entry
                    }
                }
            }
        }

        public static List<ADCertificationAuthority> GetAll()
        {
            List<ADCertificationAuthority> Collection = new List<ADCertificationAuthority>();
            ADCertificationAuthority CA;

            using (DirectoryEntry EnrollmentServicesContainer = new DirectoryEntry(LdapUrls.EnrollmentServicesContainer))
            {
                foreach (DirectoryEntry CAEntry in EnrollmentServicesContainer.Children)
                {
                    try
                    {
                        CA = new ADCertificationAuthority(CAEntry);
                        Collection.Add(CA);
                    }
                    catch (CertificationAuthorityNotFoundException)
                    {
                        //This directory entry is not a valid enrollment services object.
                    }
                }
            }

            return Collection;
        }
    }
}
