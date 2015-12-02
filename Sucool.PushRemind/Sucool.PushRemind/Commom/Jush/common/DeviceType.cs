using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;


namespace Jpush.api.common
{
    public enum DeviceType
    {
        [Description("android")] android,
        [Description("ios")]     ios,
        [Description("winphone")]  winphone

    }
}
