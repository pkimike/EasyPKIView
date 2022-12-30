using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView {
    static class PublicKeyServicesContainerHelper {
        static readonly Exception _exception;
        static readonly String _certificateTemplatesContainer;
        static readonly String _enrollmentServicesContainer;
        static readonly String _oidContainer;
        static readonly String _domainController;


        public static readonly String CertificateTemplatesContainerUrl;
        public static readonly String EnrollmentServicesContainerUrl;
        public static readonly String OidContainerUrl;


        static PublicKeyServicesContainerHelper() {
            try {
                _domainController = getDomainController();
                String forestRootDn = getForestRootDN();
                var publicKeyServicesContainer = $"CN=Public Key Services,CN=Services,CN=Configuration,{forestRootDn}";
                _certificateTemplatesContainer = $"CN=Certificate Templates,{publicKeyServicesContainer}";
                _enrollmentServicesContainer = $"CN=Enrollment Services, {publicKeyServicesContainer}";
                _oidContainer = $"CN=OID, {publicKeyServicesContainer}";
                CertificateTemplatesContainerUrl = $"LDAP://{_domainController}/{_certificateTemplatesContainer}";
                EnrollmentServicesContainerUrl = $"LDAP://{_domainController}/{_enrollmentServicesContainer}";
                OidContainerUrl = $"LDAP://{_domainController}/{_oidContainer}";
            } catch (Exception e) {
                _exception = e;
            }
        }

        public static String GetCertificateTemplateLdapUrl(String name) {
            if (_exception != null) {
                throw new ApplicationException("Failed to connect to the Active Directory", _exception);
            }

            return $"LDAP://{_domainController}/CN={name},{_certificateTemplatesContainer}";
        }
        public static String GetEnrollmentServiceLdapUrl(String name) {
            if (_exception != null) {
                throw new ApplicationException("Failed to connect to the Active Directory", _exception);
            }

            return $"LDAP://{_domainController}/CN={name},{_enrollmentServicesContainer}";
        }
        public static String GetOidLdapUrl(String name) {
            if (_exception != null) {
                throw new ApplicationException("Failed to connect to the Active Directory", _exception);
            }

            return $"LDAP://{_domainController}/CN={name},{_oidContainer}";
        }

        static String getDomainController() {
            DirectoryContext domainContext = new DirectoryContext(DirectoryContextType.Domain);
            var domain = Domain.GetDomain(domainContext);
            return domain.FindDomainController().Name;
        }
        static String getForestRootDN() {
            return Forest.GetCurrentForest()
                .RootDomain
                .GetDirectoryEntry()
                .Properties[DsPropertyName.DistinguishedName].Value.ToString();
        }
    }
}
