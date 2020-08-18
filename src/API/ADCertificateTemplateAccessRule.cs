using EasyPKIView;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace EasyPKIView
{
    /// <summary>
    /// Describes an Active Directory identity along with its access rights to the associated certificate template
    /// </summary>
    public class ADCertificateTemplateAccessRule
    {
        /// <summary>
        /// The name of the Active Directory principal (a user, group or device)
        /// </summary>
        public string Identity { get; private set; } = string.Empty;

        /// <summary>
        /// If <strong>True</strong> the principal has complete control over the Certificate Template; it can read, modify or delete it.
        /// </summary>
        public bool FullControl { get; private set; } = false;

        /// <summary>
        /// If <strong>True</strong> the principal has access to read the certificate template object
        /// </summary>
        public bool Read { get; private set; } = false;

        /// <summary>
        /// If <strong>True</strong> the principal has access to write to the certificate template object.
        /// </summary>
        public bool Write { get; private set; } = false;

        /// <summary>
        /// If <strong>True</strong> the principal has access to manually enroll against the certificate template object
        /// </summary>
        public bool Enroll { get; private set; } = false;

        /// <summary>
        /// If <strong>True</strong> the principal has access to automatically enroll against the certificate template object
        /// </summary>
        public bool AutoEnroll { get; private set; } = false;

        internal ADCertificateTemplateAccessRule(ActiveDirectoryAccessRule AccessRule)
        {
            Identity = AccessRule.IdentityReference.ToString();
            ActiveDirectoryRights Rights = AccessRule.ActiveDirectoryRights;
            if (Rights.HasFlag(ActiveDirectoryRights.GenericRead) || Rights.HasFlag(ActiveDirectoryRights.GenericExecute))
            {
                Read = true;
            }
            if (Rights.HasFlag(ActiveDirectoryRights.WriteDacl))
            {
                Write = true;
            }
            if (Rights.HasFlag(ActiveDirectoryRights.GenericAll))
            {
                FullControl = true;
            }
            if (Rights.HasFlag(ActiveDirectoryRights.ExtendedRight))
            {
                switch(AccessRule.ObjectType.ToString())
                {
                    case ExtendedRightGuid.Enroll:
                        Enroll = true;
                        break;
                    case ExtendedRightGuid.AutoEnroll:
                        AutoEnroll = true;
                        break;
                }
            }
        }

        internal void MergeIf(ADCertificateTemplateAccessRule Rule)
        {
            if (Rule.Identity.Matches(Identity))
            {
                FullControl |= Rule.FullControl;
                Read |= Rule.Read;
                Write |= Rule.Write;
                Enroll |= Rule.Enroll;
                AutoEnroll |= Rule.AutoEnroll;
            }
        }
    }
}
