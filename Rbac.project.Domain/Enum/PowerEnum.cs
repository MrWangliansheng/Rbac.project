using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Domain.Enum
{
    public enum PowerEnum
    {
        [Description("目录")]
        Directory=1,
        [Description("菜单")]

        Meun,
        [Description("按钮")]

        Button,

    }
}
