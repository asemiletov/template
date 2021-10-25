using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LementPro.Server.SvcTemplate.Common.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BlockWorkStatus
    {
        Undefined,
        New,
        Confirmed,
        Deleted
    }
}