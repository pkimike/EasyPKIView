namespace EasyPKIView.CertificateTemplates {
    /// <summary>
    /// Indicates what key attestation will be performed based on.
    /// <para>This enumeration has a <see cref="FlagsAttribute"/> attribute that allows a bitwise combination of its member values.</para>
    /// </summary>
    [Flags]
    public enum KeyAttestationMethodFlags {
        /// <summary>
        /// None.
        /// </summary>
        None         = 0,
        /// <summary>
        /// This flag indicates that attestation based on the user's credentials is to be performed.
        /// </summary>
        TrustOnUse   = 0x200,
        /// <summary>
        /// This flag indicates that attestation based on the hardware certificate of the Trusted Platform Module (TPM)
        /// is to be performed.
        /// </summary>
        ValidateCert = 0x400,
        /// <summary>
        /// This flag indicates that attestation based on the hardware key of the TPM is to be performed.
        /// </summary>
        ValidateKey  = 0x800
    }
}
