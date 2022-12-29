using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView
{
    /// <summary>
    /// ADCS Directory Object type.
    /// </summary>
    public enum AdcsObjectType {
        /// <summary>
        /// Not an ADCS directory object.
        /// </summary>
        None                = 0,
        /// <summary>
        /// Enrollment service (online CA).
        /// </summary>
        EnrollmentService   = 1,
        /// <summary>
        /// Certificate Template.
        /// </summary>
        CertificateTemplate = 2
    }
}
