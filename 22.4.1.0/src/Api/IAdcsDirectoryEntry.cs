using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView
{
    /// <summary>
    /// Describes an entry in the Public Key Services container of Active Directory
    /// </summary>
    interface IAdcsDirectoryEntry {
        /// <summary>
        /// The unique ID of this entry in the collection
        /// </summary>
        Guid TransientId { get; set; }
        /// <summary>
        /// The type of ADCS Object
        /// </summary>
        AdcsObjectType ObjectType { get; set; }
        /// <summary>
        /// The Name attribute of the directory entry
        /// </summary>
        String Name { get; set; }
        /// <summary>
        /// The Display name attribute of the directory entry
        /// </summary>
        String DisplayName { get; set; }
        /// <summary>
        /// The Distinguished Name attribute of the directory entry
        /// </summary>
        String DistinguishedName { get; set; }
        /// <summary>
        /// Indicates when the directory entry was created
        /// </summary>
        DateTime WhenCreated { get; set; }
        /// <summary>
        /// Indicates when the directory entry was last modified
        /// </summary>
        DateTime WhenChanged { get; set; }
        /// <summary>
        /// The unique GUID of the directory entry
        /// </summary>
        Guid Guid { get; set; }
    }
}
