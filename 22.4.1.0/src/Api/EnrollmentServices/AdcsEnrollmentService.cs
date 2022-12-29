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
        
    }
    /// <summary>
    /// Accepts the directory entry of an enrollment service.
    /// </summary>
    /// <param name="dEntry">The directory entry of an enrollment service.</param>
    public AdcsEnrollmentService(DirectoryEntry dEntry) : base(AdcsObjectType.EnrollmentService, dEntry) {
        
    }
    /// <summary>
    /// Accepts the public certificate of a CA
    /// </summary>
    /// <param name="caCertificate">The public certificate of a CA</param>
    public AdcsEnrollmentService(X509Certificate2 caCertificate) : this(caCertificate.Subject.Replace("CN=", String.Empty).Split(',')[0]) { }

    /// <summary>
    /// The CA's public certificate.
    /// </summary>
    public X509Certificate2 Certificate { get; set; }
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

    void setFieldsFromDirectoryEntry() {
        var certBytes = (Byte[])DirEntry.Properties[DsPropertyName.CACertificate]?.Value;
        if (certBytes != null) {
            Certificate = new X509Certificate2(certBytes);
        }
        Flags = (EnrollmentServiceFlags)Convert.ToInt32(DirEntry.Properties[DsPropertyName.Flags]?.Value);
        DnsHostName = DirEntry.Properties[DsPropertyName.DNSHostName].Value.ToString();
        CaCertificateDn = DirEntry.Properties[DsPropertyName.CACertificateDN].Value.ToString();
        PublishedCertificateTemplates = GetMultiStringAttribute(DsPropertyName.CertificateTemplates);
    }

    void setCalculatedFields() {
        IsEnterprise = (Flags & EnrollmentServiceFlags.IsEnterprise) > 0;
    }

    public static List<AdcsEnrollmentService> GetAllFromDirectory() {

        using var container = new DirectoryEntry(PublicKeyServicesContainerHelper.EnrollmentServicesContainerUrl);
        if (container?.Children is null) {
            return new List<AdcsEnrollmentService>();
        }

        var retValue = new List<AdcsEnrollmentService>();
        foreach (DirectoryEntry dEntry in container.Children) {
            try {
                var enrollService = new AdcsEnrollmentService(dEntry);
                retValue.Add(enrollService);
            } catch (CertificationAuthorityNotFoundException) {
                //This directory entry is not a valid enrollment services object.
            }
        }

        return retValue;
    }
}
