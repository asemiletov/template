using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
