using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Domain.Dto
{
    public class PageDto:ResultDtoData
    {
        public int pageindex { get; set; }

        public int pagesize { get; set; }

        public int total { get; set; }

        public int pagecount { get; set; }
    }
}
