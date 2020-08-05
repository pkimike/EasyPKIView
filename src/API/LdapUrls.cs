using System.DirectoryServices.ActiveDirectory;

namespace EasyPKIView
{
    internal class LdapUrls
    {
        private static string dc = string.Empty;
        private static bool dcSet = false;

        private static string rootDN = string.Empty;
        private static bool rootDNSet = false;

        private static string pksContainer = string.Empty;
        private static bool pksContainerSet = false;

        private static string templatesContainerDN = string.Empty;
        private static bool templatesContainerDNSet = false;

        private static string templatesUrl = string.Empty;
        private static bool templatesUrlSet = false;

        private static string esContainerDN = string.Empty;
        private static bool esContainerDNSet = false;

        private static string esUrl = string.Empty;
        private static bool esUrlSet = false;

        private static string DC
        {
            get
            {
                if (!dcSet)
                {
                    GetDomainController();

                }
                return dc;
            }
        }

        private static string RootDN
        {
            get
            {
                if (!rootDNSet)
                {
                    GetForestRootDN();

                }
                return rootDN;
            }
        }

        private static string PublicKeyServicesContainerDN
        {
            get
            {
                if (!pksContainerSet)
                {
                    pksContainer = $"CN=Public Key Services,CN=Services,CN=Configuration,{RootDN}";
                }
                return pksContainer;
            }
        }

        private static string CertificateTemplatesContainerDN
        {
            get
            {
                if (!templatesContainerDNSet)
                {
                    templatesContainerDN = $"CN=Certificate Templates,{PublicKeyServicesContainerDN}";
                }
                return templatesContainerDN;
            }
        }

        private static string EnrollmentServicesContainerDN
        {
            get
            {
                if (!esContainerDNSet)
                {
                    esContainerDN = $"CN=Enrollment Services,{PublicKeyServicesContainerDN}";
                }
                return esContainerDN;
            }
        }

        internal static string CertificateTemplatesContainer
        {
            get
            {
                if (!templatesUrlSet)
                {
                    templatesUrl = $"LDAP://{DC}/{CertificateTemplatesContainerDN}";
                }
                return templatesUrl;
            }
        }

        internal static string EnrollmentServicesContainer
        {
            get
            {
                if (!esUrlSet)
                {
                    esUrl = $"LDAP://{DC}/{EnrollmentServicesContainerDN}";
                }
                return esUrl;
            }
        }

        internal static string CertificateTemplate(string name)
        {
            return $"LDAP://{DC}/CN={name},{CertificateTemplatesContainerDN}";
        }

        internal static string EnrollmentService(string name)
        {
            return $"LDAP://{DC}/CN={name},{EnrollmentServicesContainerDN}";
        }

        private static void GetDomainController()
        {
            DirectoryContext domainContext = new DirectoryContext(DirectoryContextType.Domain);
            var domain = Domain.GetDomain(domainContext);
            dc = domain.FindDomainController().Name;
            dcSet = true;
        }

        private static void GetForestRootDN()
        {
            rootDN = Forest.GetCurrentForest()
                           .RootDomain
                           .GetDirectoryEntry()
                           .Properties[PropertyIndex.DistinguishedName].Value.ToString();
            rootDNSet = true;
        }
    }
}
