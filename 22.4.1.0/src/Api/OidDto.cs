using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyPKIView.AdcsOids;

namespace EasyPKIView;
public class OidDto {
    public OidDto() { }

    public OidDto(String value) {
        Value = value;
    }

    public String FriendlyName { get; set; }
    public String Value { get; set; }

    /// <summary>
    /// Attempts to find a matching OID from the Public Key Services container
    /// where the OID value matches this custom OID value
    /// </summary>
    /// <param name="adcsOids">List of AdcsOids</param>
    public void TryDefineFriendlyName(List<AdcsOid> adcsOids) {
        AdcsOid? match = adcsOids.FirstOrDefault(o => o.Value.Equals(Value));
        if (match != null) {
            FriendlyName = match.DisplayName;
        }
    }
}
