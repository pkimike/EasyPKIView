using System.Security.Cryptography.X509Certificates;

namespace EasyPKIView.CertificateTemplates;
/// <summary>
/// Key usage extension
/// </summary>
public class KeyUsageExtension : CertificateTemplateExtension<X509KeyUsageFlags> {
    /// <summary>
    /// Default constructor
    /// </summary>
    public KeyUsageExtension() { }
    /// <summary>
    /// This constructor accepts <see cref="X509KeyUsageFlags"/>
    /// </summary>
    /// <param name="value">Key usage flags</param>
    public KeyUsageExtension(X509KeyUsageFlags value) {
        Oid = X509ExtensionOid.KeyUsage;
        Value = value;
    }
}
