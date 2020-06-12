using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Cryptography.X509Certificates;

namespace EasyPKIView
{
    /// <summary>
    /// Describes a Microsoft Enterprise Certification Authority as stored in the "Enrollment Services" container in Active Directory
    /// </summary>
    public class ADCertificationAuthority : ADCSDirectoryEntry
    {
        /// <summary>
        /// An object containing the CA's public certificate
        /// </summary>
        public X509Certificate2 CACertificate { get; private set; }

        /// <summary>
        /// Indicates whether this CA is an Enterprise or Standalone CA.
        /// </summary>
        public bool IsEnterpriseCA { get; private set; }

        /// <summary>
        /// Indicates the DNS name of the server where the CA is installed.
        /// </summary>
        public string DNSHostName { get; private set; }

        /// <summary>
        /// Indicates the Distinguished name of the CA's Certificate
        /// </summary>
        public string CACertificateDN { get; private set; }

        /// <summary>
        /// The list of ADCertificateTemplates advertised as being available for enrollment on this CA.
        /// </summary>
        public List<ADCertificateTemplate> Templates { get; private set; } = new List<ADCertificateTemplate>();


        /// <summary>
        /// Indicates whether this CA advertises any certificate templates.
        /// </summary>
        public bool HasTemplates
        {
            get
            {
                return Templates.Count > 0;
            }
        }

        /// <summary>
        /// ADCertificationAuthority Constructor 1
        /// </summary>
        /// <param name="name">The common name of the CA</param>
        public ADCertificationAuthority(string name)
            : base(LdapUrls.EnrollmentService(name), ObjectClass.PKIEnrollmentService)
        {
            if (!Usable)
            {
                throw new CertificationAuthorityNotFoundException(name);
            }
            SetFieldsFromDirectoryObject();
        }

        /// <summary>
        /// ADCertificationAuthority Constructor 2
        /// </summary>
        /// <param name="CAEntry">The Active Directory entry pointing to this CA Enrollment Services object</param>
        public ADCertificationAuthority(DirectoryEntry CAEntry)
            : base(CAEntry, ObjectClass.PKIEnrollmentService)
        {
            if (!Usable)
            {
                throw new CertificationAuthorityNotFoundException();
            }
            SetFieldsFromDirectoryObject();
        }

        /// <summary>
        /// ADCertificationAuthority Constructor 3
        /// </summary>
        /// <param name="CACert">The CA's public certificate</param>
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

        /// <summary>
        /// Loads all CAs in the current Active Directory forest
        /// </summary>
        /// <returns>A list of ADCertificationAuthority objects</returns>
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
