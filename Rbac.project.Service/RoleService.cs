using AutoMapper;
using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using Rbac.project.IRepoistory;
using Rbac.project.IRepoistory.Eextend;
using Rbac.project.IService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rbac.project.Service
{
    public class RoleService : BaseService<RoleData, ResultDtoData>, IRoleService
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

                return new PageDto { Result = Result.Success, Data = list, pagecount = dto.pagecount, total = dto.total };

            }
            catch (Exception ex)
            {
                logdata.CreateLog("/RoleRepoistory/GetRolePage", "查询角色信息:" + ex.Message, "");
                return new PageDto { Result = Result.Error, Message = ex.Message };
            }
        }
        /// <summary>
        /// 查询角色树
        /// </summary>
        /// <returns></returns>
        public ResultDtoData GetRoleTree()
        {
            return dal.GetRoleTree();
        }
        /// <summary>
        /// 逻辑删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<ResultDtoData> LogicDeleteAsync(int id)
        {
            var role = await Idal.LogicDeleteAsync(id);
            return new ResultDtoData { Result = Result.Success, Message = "删除成功" };
        }
        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override async Task<ResultDtoData> InsertAsync(RoleData t)
        {
            var role = await Idal.InsertAsync(t);
            if (role == null)
            {
                return new ResultDtoData { Result = Result.Error, Message = "添加角色异常详情请联系管理员" };
            }
            else if (role.RoleId > 0)
            {
                return new ResultDtoData { Result = Result.Success, Message = "添加角色成功" };
            }
            else
            {
                return new ResultDtoData { Result = Result.Warning, Message = "角色已重复" };
            }
        }
        /// <summary>
        /// 反填角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<ResultDtoData> FindAsync(int id)
        {
            var role = await dal.FindAsync(id);

            if (role != null)
            {
                return new ResultDtoData { Result = Result.Success, Message = "反填信息成功" ,Data=role};
            }
            else
            {
                return new ResultDtoData { Result = Result.Error, Message = "反填信息失败" };
            }
        }
        /// <summary>
        /// 修改用户角色信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override ResultDtoData Update(RoleData t)
        {
            var role = dal.Update(t);
            if (role == null)
            {
                return new ResultDtoData { Result = Result.Error, Message = "修改信息异常请重新操作或联系管理员" };
            }
            else if (role.RoleId == -1)
            {
                return new ResultDtoData { Result = Result.Warning, Message = "该角色已存在无法修改" };
            }
            else
            {
                return new ResultDtoData { Result = Result.Success, Message = "修改成功" };
            }
        }

        public ResultDtoData GetRolePowerButton(int id, int state)
        {
            return dal.GetRolePowerButton(id,state);
        }

        public ResultDtoData GetRolePower()
        {
           return dal.GetRolePower();
        }
    }
}
