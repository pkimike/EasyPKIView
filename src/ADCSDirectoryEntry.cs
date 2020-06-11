using System;
using System.DirectoryServices;

namespace EasyPKIView
{
    public class ADCSDirectoryEntry
    {
        internal DirectoryEntry DirEntry;
        internal bool Usable = true;

        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public string DistinguishedName { get; private set; }
        public DateTime WhenCreated { get; private set; }
        public DateTime WhenChanged { get; private set; }
        public Guid ObjectGuid { get; private set; }

        public ADCSDirectoryEntry(DirectoryEntry Entry, string expectedObjectClass)
        {
            DirEntry = Entry;
            SetFieldsFromDirectoryEntry(expectedObjectClass);
        }

        public ADCSDirectoryEntry(string ldapURL, string expectedObjectClass)
            : this(new DirectoryEntry(ldapURL), expectedObjectClass)
        { }

        private void SetFieldsFromDirectoryEntry(string expectedObjectClass)
        {
            if (DirEntry == null)
            {
                Usable = false;
                return;
            }

            object[] objectClass = (object[])DirEntry.Properties[PropertyIndex.ObjectClass].Value;
            if (objectClass.Length < 2 || !objectClass[1].ToString().Matches(expectedObjectClass))
            {
                Usable = false;
                return;
            }

            Name = DirEntry.Properties[PropertyIndex.Name].Value.ToString();
            DisplayName = DirEntry.Properties[PropertyIndex.DisplayName].Value.ToString();
            DistinguishedName = DirEntry.Properties[PropertyIndex.DistinguishedName].Value.ToString();
            ObjectGuid = new Guid((byte[])DirEntry.Properties[PropertyIndex.ObjectGUID].Value);
            WhenCreated = (DateTime)DirEntry.Properties[PropertyIndex.WhenCreated].Value;
            WhenChanged = (DateTime)DirEntry.Properties[PropertyIndex.WhenChanged].Value;
        }
    }
}
