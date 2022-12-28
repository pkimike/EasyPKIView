using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView {
    /// <summary>
    /// Indicates whether or how key attestation is enforced by the template
    /// </summary>
    public enum KeyAttestationEnforcement {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,
        /// <summary>
        /// Key attestation is preferred but not required.
        /// </summary>
        Preferred = 1,
        /// <summary>
        /// Key attestation is required.
        /// If the CA or client is incapable of key attestation, the request will be denied.
        /// </summary>
        Required = 2
    }
}
