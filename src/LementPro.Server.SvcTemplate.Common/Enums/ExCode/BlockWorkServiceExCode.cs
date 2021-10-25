using System.ComponentModel;

namespace LementPro.Server.SvcTemplate.Common.Enums.ExCode
{
    public enum BlockWorkServiceExCode
    {
        [Description("Error unknown")]
        Undefined = 0,

        [Description("Item not found")]
        NotFound = 1
    }
}
