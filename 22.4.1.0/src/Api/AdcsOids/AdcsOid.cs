using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPKIView.AdcsOids;
public class AdcsOid : AdcsDirectoryEntry {
    public AdcsOid() { }

    public AdcsOid(DirectoryEntry dEntry) : base(AdcsObjectType.Oid, dEntry) {
        if (ObjectType == AdcsObjectType.Oid) {
            setProperties();
        }
    }
    public AdcsOid(String name) : base(AdcsObjectType.Oid, PublicKeyServicesContainerHelper.GetOidLdapUrl(name)) {
        if (ObjectType == AdcsObjectType.Oid) {
            setProperties();
        }
    }

    public AdcsOidType OidType { get; set; }
    public String Value { get; set; }

    void setProperties() {
        OidType = (AdcsOidType)GetInt32(DsPropertyName.Flags, 0);
        Value = DirEntry.Properties[DsPropertyName.OID].Value?.ToString();
    }

    public static List<AdcsOid> GetAllFromDirectory() {
        using DirectoryEntry oidContainer = new DirectoryEntry(PublicKeyServicesContainerHelper.OidContainerUrl);
        if (oidContainer is null) {
            return new List<AdcsOid>();
        }

        return (from DirectoryEntry dEntry 
                in oidContainer.Children 
                select new AdcsOid(dEntry) 
                into currentOid 
                where currentOid is not null && currentOid.ObjectType == AdcsObjectType.Oid 
                select currentOid).ToList();
    }
}
