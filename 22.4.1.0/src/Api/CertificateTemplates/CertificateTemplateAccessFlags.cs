namespace EasyPKIView.CertificateTemplates {
    /// <summary>
    /// Defines certificate template enrollment flags.
    /// <para>This enumeration has a <see cref="FlagsAttribute"/> attribute that allows a bitwise combination of its member values.</para>
    /// </summary>
    [Flags]
    public enum CertificateTemplateAccessFlags {
        /// <summary>
        /// No rights.
        /// </summary>
        None        = 0,
        /// <summary>
        /// Read permissions
        /// </summary>
        Read        = 0x1,
        /// <summary>
        /// Write permissions
        /// </summary>
        Write       = 0x2,
        /// <summary>
        /// Full control.
        /// </summary>
        FullControl = 0x4,
        /// <summary>
        /// Enroll permission.
        /// </summary>
        Enroll      = 0x8,
        /// <summary>
        /// Autoenroll permission
        /// </summary>
        Autoenroll  = 0x10
    }
}
