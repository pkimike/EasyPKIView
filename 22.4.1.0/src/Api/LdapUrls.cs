using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView {
    static class LdapUrls {
        static readonly Exception _exception;

        internal static String CertificateTemplatesContainer;
        internal static String CertificateTemplatesContainerUrl;
        internal static String EnrollmentServicesContainer;
        internal static String EnrollmentServicesContainerUrl;

        static String forestRootDn;
        static String domainController;
        static String publicKeyServicesContainer;

        static LdapUrls() {
            try {
                setDomainController();
                setForestRootDN();
                publicKeyServicesContainer = $"CN=Public Key Services,CN=Services,CN=Configuration,{forestRootDn}";
                CertificateTemplatesContainer = $"CN=Certificate Templates,{publicKeyServicesContainer}";
                CertificateTemplatesContainerUrl = $"LDAP://{domainController}/{CertificateTemplatesContainer}";
                EnrollmentServicesContainerUrl = $"CN=Enrollment Services,{publicKeyServicesContainer}";
                EnrollmentServicesContainerUrl = $"LDAP://{domainController}/{EnrollmentServicesContainer}";
            } catch (Exception e) {
                _exception = e;
            }
        }

        public static String GetCertificateTemplateLdapUrl(String name) {
            if (_exception != null) {
                throw new ApplicationException("Failed to connect to the Active Directory", _exception);
            }

            return $"LDAP://{domainController}/CN={name},{CertificateTemplatesContainer}";
        }

        public static String GetEnrollmentServiceLdapUrl(String name) {
            if (_exception != null) {
                throw new ApplicationException("Failed to connect to the Active Directory", _exception);
            }

            return $"LDAP://{domainController}/CN={name},{EnrollmentServicesContainer}";
        }

        static void setDomainController() {
            DirectoryContext domainContext = new DirectoryContext(DirectoryContextType.Domain);
            var domain = Domain.GetDomain(domainContext);
            domainController = domain.FindDomainController().Name;
        }
        static void setForestRootDN() {
            forestRootDn = Forest.GetCurrentForest()
                .RootDomain
                .GetDirectoryEntry()
                .Properties[DsPropertyIndex.DistinguishedName].Value.ToString();
        }
    }
}
