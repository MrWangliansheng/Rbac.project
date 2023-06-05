using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.IService.Eextend
{
    public interface IGetHeaderToken
    {
        string GetHeader(string token);
    }
}
