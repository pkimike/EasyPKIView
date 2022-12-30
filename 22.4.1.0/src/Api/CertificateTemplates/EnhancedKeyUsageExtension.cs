namespace EasyPKIView.CertificateTemplates;
/// <summary>
/// Enhanced key usage extension
/// </summary>
public class EnhancedKeyUsageExtension : CertificateTemplateExtension<EnhancedKeyUsageFlags> {
    /// <summary>
    /// Default constructor
    /// </summary>
    public EnhancedKeyUsageExtension() { }
    /// <summary>
    /// This constructor accepts enhanced key usage flags.
    /// </summary>
    /// <param name="value">Enhanced key usage flags value</param>
    public EnhancedKeyUsageExtension(EnhancedKeyUsageFlags value) {
        Oid = X509ExtensionOid.EnhancedKeyUsage;
        Value = value;
    }

    /// <summary>
    /// The list of custom enhanced key usage OIDs.
    /// </summary>
    public List<OidDto> Custom { get; set; } = new List<OidDto>();
}
