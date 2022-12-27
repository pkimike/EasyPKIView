using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView;

/// <summary>
/// Key Attestation Type Enumeration.  
/// See https://tinyurl.com/y9c6oxnp for more information
/// </summary>
public enum KeyAttestationType {
    /// <summary>
    /// None. Key Attestation not supported by this certificate template
    /// </summary>
    [Description("None")]
    None = 0x0,

    /// <summary>
    /// An authenticating user is allowed to vouch for a valid TPM by specifying their domain credentials.
    /// </summary>
    [Description("Account Credentials")]
    AccountCredentials = 0x200,

    /// <summary>
    /// The EKCert of the device must validate through administrator-managed TPM intermediate CA certificates to an administrator-managed root CA certificate.
    /// </summary>
    [Description("Manufacturer Signing Certificate")]
    SigningCertificate = 0x400,

    /// <summary>
    /// The EKPub of the device must appear in the PKI administrator-managed list.
    /// </summary>
    [Description("Manufacturer Pre-Shared Key")]
    PreSharedKey = 0x800
}
