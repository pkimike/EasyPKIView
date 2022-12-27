using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView;
class LdapUrls {
    internal static String CertificateTemplatesContainer;
    internal static String CertificateTemplatesContainerUrl;
    internal static String EnrollmentServicesContainer;
    internal static String EnrollmentServicesContainerUrl;

    static String forestRootDn;
    static String domainController;
    static String publicKeyServicesContainer;

    static LdapUrls() {
        setDomainController();
        setForestRootDN();
        publicKeyServicesContainer = $"CN=Public Key Services,CN=Services,CN=Configuration,{forestRootDn}";
        CertificateTemplatesContainer = $"CN=Certificate Templates,{publicKeyServicesContainer}";
        CertificateTemplatesContainerUrl = $"LDAP://{domainController}/{CertificateTemplatesContainer}";
        EnrollmentServicesContainerUrl = $"CN=Enrollment Services,{publicKeyServicesContainer}";
        EnrollmentServicesContainerUrl = $"LDAP://{domainController}/{EnrollmentServicesContainer}";
    }

    internal static String getCertificateTemplateLdapUrl(String name) {
        return $"LDAP://{domainController}/CN={name},{CertificateTemplatesContainer}";
    }

    internal static String getEnrollmentServiceLdapUrl(String name) {
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
