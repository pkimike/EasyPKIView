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
        protected AdcsDirectoryEntry(AdcsObjectType expectedObjectType, DirectoryEntry dEntry) {
            TransientId = Guid.NewGuid();
            DirEntry = dEntry;
            if (!GetMultiStringAttribute(DsPropertyName.ObjectClass).Contains(expectedObjectType.GetObjectClassName())) {
                ObjectType = AdcsObjectType.None;
                return;
            }
            ObjectType = expectedObjectType;
            setFieldsFromDirectoryEntry(expectedObjectType, dEntry);
        }

        protected AdcsDirectoryEntry(AdcsObjectType expectedObjectType, String ldapURL) : this(expectedObjectType, new DirectoryEntry(ldapURL)) { }

        public Guid TransientId { get; set; }
        public AdcsObjectType ObjectType { get; set; }
        public String Name { get; set; }
        public String DisplayName { get; set; }
        public String DistinguishedName { get; set; }
        public DateTime WhenCreated { get; set; }
        public DateTime WhenChanged { get; set; }
        public Guid Guid { get; set; }

        protected DirectoryEntry DirEntry { get; set; }

        protected virtual Int32? GetInt32(String propertyName) {
            Object? property = DirEntry.Properties[propertyName]?.Value;
            return property is null
                ? null
                : Convert.ToInt32(property);
        }
        protected virtual Int32 GetInt32(String propertyName, Int32 defaultValue) {
            Int32? retValue = GetInt32(propertyName);
            return retValue.HasValue
                ? retValue.Value
                : defaultValue;
        }
        protected virtual List<String> GetMultiStringAttribute(String propertyName) {
            Object[] elements;

            try {
                elements = (Object[])DirEntry.Properties[propertyName].Value;
            } catch {
                //If there's only a single entry, it won't cast as an array.
                elements = new[] { DirEntry.Properties[propertyName].Value };
            }

            return elements is null 
                ? new List<String>() 
                : elements.Select(item => item.ToString()).ToList();
        }

        Boolean testObjectClass(DirectoryEntry dEntry, AdcsObjectType expectedObjectType) {
            String objectCategory = dEntry.Properties[DsPropertyName.ObjectCategory]?.Value?.ToString();
            if (String.IsNullOrEmpty(objectCategory)) {
                return false;
            }
            String[] parts = objectCategory.Split('=');
            if (parts.Length < 2) {
                return false;
            }

            return parts[1].Split(',')[0].Equals(expectedObjectType.GetObjectClassName());
        }
        void setFieldsFromDirectoryEntry(AdcsObjectType expectedObjectType, DirectoryEntry dEntry) {
            if (dEntry is null) {
                ObjectType = AdcsObjectType.None;
                return;
            }

            var objectClass = (Object[])dEntry.Properties[DsPropertyName.ObjectClass]?.Value;
            if (objectClass is null || objectClass.Length < 2 || !objectClass[1].ToString().Equals(expectedObjectType.GetObjectClassName(), StringComparison.InvariantCultureIgnoreCase))
            {
                ObjectType = AdcsObjectType.None;
                return;
            }

            Name = DirEntry.Properties[DsPropertyName.Name].Value?.ToString() ?? String.Empty;
            DisplayName = DirEntry.Properties[DsPropertyName.DisplayName].Value?.ToString() ?? String.Empty;
            DistinguishedName = DirEntry.Properties[DsPropertyName.DistinguishedName].Value?.ToString() ?? String.Empty;
            Guid = new Guid((byte[])DirEntry.Properties[DsPropertyName.ObjectGUID]?.Value);

            try {
                WhenCreated = (DateTime)DirEntry.Properties[DsPropertyName.WhenCreated].Value;
            } catch {
                WhenCreated = DateTime.MinValue;
            }
            try {
                WhenChanged = (DateTime)DirEntry.Properties[DsPropertyName.WhenChanged].Value;
            } catch {
                WhenChanged = DateTime.MinValue;
            }
        }
    }
}
