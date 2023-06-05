using AutoMapper;
using Rbac.project.Domain;
using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
