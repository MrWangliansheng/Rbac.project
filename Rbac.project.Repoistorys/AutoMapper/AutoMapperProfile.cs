using AutoMapper;
using Rbac.project.Domain;
using Rbac.project.Domain.DataDisplay;

namespace Rbac.project.Repoistorys.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserData>().ReverseMap();
            CreateMap<RoleData, Role>().ReverseMap();
            CreateMap<Power,PowerData>().ReverseMap();
            CreateMap<object,User>().ReverseMap();
            //CreateMap<List<User>,object>().ReverseMap();
            //CreateMap<List<Power>,object>().ReverseMap();
        }
    }
}
