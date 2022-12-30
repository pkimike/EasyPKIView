using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using EasyPKIView.CertificateTemplates;

namespace EasyPKIView.EnrollmentServices;
/// <summary>
/// ADCS Enrollment service.
/// </summary>
public class AdcsEnrollmentService : AdcsDirectoryEntry {
    /// <summary>
    /// Empty constructor.
    /// </summary>
    public AdcsEnrollmentService() { }
    /// <summary>
    /// Accepts a CA common name.
    /// </summary>
    /// <param name="name">A CA common name.</param>
    public AdcsEnrollmentService(String name) : base(AdcsObjectType.EnrollmentService, PublicKeyServicesContainerHelper.GetEnrollmentServiceLdapUrl(name)) {
        if (ObjectType == AdcsObjectType.EnrollmentService) {
            setProperties();
        }
    }
    /// <summary>
    /// Accepts the directory entry of an enrollment service.
    /// </summary>
    /// <param name="dEntry">The directory entry of an enrollment service.</param>
    public AdcsEnrollmentService(DirectoryEntry dEntry) : base(AdcsObjectType.EnrollmentService, dEntry) {
        if (ObjectType == AdcsObjectType.EnrollmentService) {
            setProperties();
        }
    }
    /// <summary>
    /// Accepts the public certificate of a CA
    /// </summary>
    /// <param name="caCertificate">The public certificate of a CA</param>
    public AdcsEnrollmentService(X509Certificate2 caCertificate) : this(caCertificate.Subject.Replace("CN=", String.Empty).Split(',')[0]) { }

    /// <summary>
    /// The CA's public certificate.
    /// </summary>
    public Byte[] Certificate { get; set; }
    /// <summary>
    /// Indicates whether the CA is an enterprise or a standalone CA.
    /// </summary>
    public Boolean IsEnterprise { get; set; }
    /// <summary>
    /// The DNS name of the host where the CA is installed.
    /// </summary>
    public String DnsHostName { get; set; }
    /// <summary>
    /// The common name of the CA.
    /// </summary>
    public String Name { get; set; }
    /// <summary>
    /// The Distinguished name of the CA's certificate
    /// </summary>
    public String CaCertificateDn { get; set; }
    /// <summary>
    /// Enrollment service flags
    /// </summary>
    public EnrollmentServiceFlags Flags { get; set; }
    /// <summary>
    /// The transient IDs of the certificate templates that are assigned to this CA.
    /// </summary>
    public List<String> PublishedCertificateTemplates { get; set; } = new();

    /// <summary>
    /// Gets the CA Config string.
    /// </summary>
    /// <returns>The CA Config string</returns>
    public String GetCAConfigName() => $"{DnsHostName}\\{Name}";

    /// <summary>
    /// Gets the list of certificate templates assigned to this CA.
    /// </summary>
    /// <param name="entries">The collection of certificate templates in the Active Directory Forest.</param>
    /// <returns>The list of certificate templates assigned to this CA.</returns>
    public List<AdcsCertificateTemplate> GetPublishedCertificateTemplates(List<AdcsCertificateTemplate> entries) {
        return entries.Where(e => PublishedCertificateTemplates.Contains(e.Name)).ToList();
    }

    public X509Certificate2 GetCACertificate() {
        if (Certificate is null) {
            return null;
        }

        return new X509Certificate2(Certificate);
    }

    void setProperties() {
        Certificate = (Byte[])DirEntry.Properties[DsPropertyName.CACertificate]?.Value;
        Flags = (EnrollmentServiceFlags)GetInt32(DsPropertyName.Flags, 0);
        DnsHostName = DirEntry.Properties[DsPropertyName.DNSHostName].Value.ToString();
        CaCertificateDn = DirEntry.Properties[DsPropertyName.CACertificateDN].Value.ToString();
        PublishedCertificateTemplates = GetMultiStringAttribute(DsPropertyName.CertificateTemplates);
        IsEnterprise = (Flags & EnrollmentServiceFlags.IsEnterprise) > 0;
    }

    public static List<AdcsEnrollmentService> GetAllFromDirectory() {
        using var container = new DirectoryEntry(PublicKeyServicesContainerHelper.EnrollmentServicesContainerUrl);
        if (container?.Children is null) {
            return new List<AdcsEnrollmentService>();
        }

        return (from DirectoryEntry dEntry 
                in container.Children 
                select new AdcsEnrollmentService(dEntry) 
                into enrollService 
                where enrollService.ObjectType == AdcsObjectType.EnrollmentService 
                select enrollService).ToList();
    }
}
