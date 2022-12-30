using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView.AdcsOids;

/// <summary>
/// ADCS OID type enumeration
/// </summary>
public enum AdcsOidType {
    /// <summary>
    /// None.
    /// </summary>
    None                = 0,
    /// <summary>
    /// Certificate Template.
    /// </summary>
    CertificateTemplate = 1,
    /// <summary>
    /// Certificate Policy.
    /// </summary>
    CertificatePolicy   = 2,
    /// <summary>
    /// Enhanced Key Usage
    /// </summary>
    EnhancedKeyUsage    = 3
}
