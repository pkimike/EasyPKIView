using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView.EnrollmentServices;
/// <summary>
/// Defines the general-enrollment flags.
/// <para>This enumeration has a <see cref="FlagsAttribute"/> attribute that allows a bitwise combination of its member values.</para>
/// </summary>
[Flags]
public enum EnrollmentServiceFlags {
    /// <summary>
    /// None.
    /// </summary>
    None         = 0,
    /// <summary>
    /// Unclear what this bit position does, but it appears to always be set.
    /// </summary>
    Unknown      = 0x2,
    /// <summary>
    /// Indicates that the enrollment service is an Enterprise CA.
    /// </summary>
    IsEnterprise = 0x8
}
