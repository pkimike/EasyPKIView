using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView {
    [Flags]
    public enum CertificateTemplateAccessFlags {
        None        = 0,
        Read        = 0x1,
        Write       = 0x2,
        FullControl = 0x4,
        Enroll      = 0x8,
        Autoenroll  = 0x10
    }
}
