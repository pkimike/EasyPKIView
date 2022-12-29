namespace EasyPKIView.CertificateTemplates;
/// <summary>
/// Certificate template extension.
/// </summary>
/// <typeparam name="T">Contains the extension value</typeparam>
public abstract class CertificateTemplateExtension<T> {
    /// <summary>
    /// The OID of this extension.
    /// </summary>
    public String Oid { get; set; }
    /// <summary>
    /// The value of this extension.
    /// </summary>
    public T Value { get; set; }
    /// <summary>
    /// Indicates whether this extension is critical or not.
    /// </summary>
    public Boolean IsCritical { get; set; }
}
