using AutoMapper;
using Rbac.project.Domain;
using Rbac.project.Domain.Dto;
using Rbac.project.IRepoistory;
using Rbac.project.IRepoistory.LogOperation;
using Rbac.project.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Service
{
    public class RoleService : BaseService<Role, ResultDtoData>, IRoleService
    {
        private readonly IRoleRepoistory dal;
        private readonly IMapper mapper;
        private readonly ILogDataRepoistory logdata;
        public RoleService(IRoleRepoistory dal, IMapper mapper, ILogDataRepoistory logdata) : base(dal, mapper)
        {
            this.dal = dal;
            this.mapper = mapper;
            this.logdata = logdata;
        }
        /// <summary>
        /// 分页查询角色信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PageDto> GetRolePage(RoleDto dto)
        {
            try
            {
                var list = await dal.GetAll(m => m.RoleIsDelete.Equals(false));
                if (!string.IsNullOrEmpty(dto.name))
                {
                    list = list.Where(m => m.RoleName.Contains(dto.name)).ToList();
                }
                dto.total = list.Count;
                dto.pagecount = (int)Math.Ceiling(dto.total * 1.0 / dto.pagesize);

                list = list.Skip((dto.pageindex - 1) * dto.pagesize).Take(dto.pagesize).ToList();
                logdata.CreateLog("/RoleRepoistory/GetRolePage", "查询角色信息", "");

                return new PageDto { Result=Result.Success,Data=list,pagecount=dto.pagecount,total=dto.total};
                
            }
            catch (Exception ex)
            {
                logdata.CreateLog("/RoleRepoistory/GetRolePage", "查询角色信息:"+ex.Message, "");
                return new PageDto { Result = Result.Error,Message=ex.Message};
            }
        }

        public ResultDtoData GetRoleTree()
        {
            return dal.GetRoleTree();
        }

        public override async Task<ResultDtoData> LogicDeleteAsync(int id)
        {
            var role = await Idal.LogicDeleteAsync(id);
            if (role != null)
            {
                if (role.RoleIsDelete == true)
                {
                    return new ResultDtoData { Result = Result.Success, Message = "删除成功" };
                }
                else
                {
                    return new ResultDtoData { Result = Result.Warning, Message = "删除失败" };
                }
            }
            else
            {
                return new ResultDtoData { Result = Result.Error, Message = "删除出现异常请联系管理员" };
            }
        }

        
    }
}
