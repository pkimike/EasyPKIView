using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView
{
    public abstract class AdcsDirectoryEntry : IAdcsDirectoryEntry
    {
        protected AdcsDirectoryEntry() { }

        protected AdcsDirectoryEntry(AdcsObjectType objectType) {
            TransientId = Guid.NewGuid();
            ObjectType = objectType;
        }

        protected AdcsDirectoryEntry(AdcsObjectType objectType, DirectoryEntry dEntry, String expectedObjectClass) : this(objectType) {
            setFieldsFromDirectoryEntry(dEntry, expectedObjectClass);
        }

        public AdcsDirectoryEntry(AdcsObjectType objectType, String ldapURL, string expectedObjectClass) : this(objectType, new DirectoryEntry(ldapURL), expectedObjectClass) { }

        public Guid TransientId { get; set; }
        public AdcsObjectType ObjectType { get; set; }
        public String Name { get; set; }
        public String DisplayName { get; set; }
        public String DistinguishedName { get; set; }
        public DateTime WhenCreated { get; set; }
        public DateTime WhenChanged { get; set; }
        public Guid Guid { get; set; }

        protected DirectoryEntry DirEntry { get; set; }

        void setFieldsFromDirectoryEntry(DirectoryEntry dEntry, String expectedObjectClass) {
            if (dEntry is null) {
                ObjectType = AdcsObjectType.None;
                return;
            }

            var objectClass = (Object[])dEntry.Properties[DsPropertyIndex.ObjectClass]?.Value;
            if (objectClass is null || objectClass.Length < 2 || !objectClass[1].ToString().Equals(expectedObjectClass, StringComparison.InvariantCultureIgnoreCase))
            {
                ObjectType = AdcsObjectType.None;
                return;
            }

            DirEntry = dEntry;
            Name = DirEntry.Properties[DsPropertyIndex.Name].Value?.ToString() ?? String.Empty;
            DisplayName = DirEntry.Properties[DsPropertyIndex.DisplayName].Value?.ToString() ?? String.Empty;
            DistinguishedName = DirEntry.Properties[DsPropertyIndex.DistinguishedName].Value?.ToString() ?? String.Empty;
            Guid = new Guid((byte[])DirEntry.Properties[DsPropertyIndex.ObjectGUID]?.Value);

            try {
                WhenCreated = (DateTime)DirEntry.Properties[DsPropertyIndex.WhenCreated].Value;
            } catch {
                WhenCreated = DateTime.MinValue;
            }
            try {
                WhenChanged = (DateTime)DirEntry.Properties[DsPropertyIndex.WhenChanged].Value;
            } catch {
                WhenChanged = DateTime.MinValue;
            }
        }
    }
}
