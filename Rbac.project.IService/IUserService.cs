
using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using System.Threading.Tasks;

namespace Rbac.project.IService
{
    public interface IUserService:IBaseService<UserData, ResultDtoData>
    {
        Task<ResultDto> UserLog(string name,string pwd);
        ResultDtoData UpdateUser(UserData user);
        ResultDtoData ResetUserPasswrod(UserDto dto);
        Task<PageDto> GetUserInfoPage(UserDto dto);

        ResultDtoData GetNewToken(string token);
    }
}
