using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using Rbac.project.IRepoistory;
using Rbac.project.IService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Service
{
    public class PowerService : BaseService<PowerData, ResultDtoData>, IPowerService
    {
        private readonly IPowerRepoistory dal;
        private readonly IMapper mapper;
        public PowerService(IPowerRepoistory dal, IMapper mapper) : base(dal, mapper)
        {
            this.dal = dal;
            this.mapper = mapper;
        }
        
        public ResultDtoData GetPowerEnum()
        {
            return dal.GetPowerEnum();
        }

        /// <summary>
        /// 菜单级联选择器绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultDtoData GetPowerTree()
        {
            return dal.GetPowerTree();
        }
        /// <summary>
        /// 菜单权限级联绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ResultDtoData GetPowerTreeData()
        {
            return dal.GetPowerTreeData();
        }

        /// <summary>
        /// 树形菜单列表显示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultDtoData> GetPowerTreeTableLevelone(int id = 0)
        {
            return await dal.GetPowerTreeTableLevelone(id);
        }

        #region 权限菜单操作
        /// <summary>
        /// 添加菜单信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override async Task<ResultDtoData> InsertAsync(PowerData t)
        {
            var pdata = await Idal.InsertAsync(t);
            if (pdata == null)
            {
                return new ResultDtoData { Result = Result.Error, Message = "添加菜单异常，详情请联系管理员" };
            }
            else if (pdata.PowerId == -1)
            {
                return new ResultDtoData { Result = Result.Warning, Message = "改菜单已存在" };
            }
            else
            {
                return new ResultDtoData { Result = Result.Success, Message = "添加成功" };
            }
        }
        /// <summary>
        /// 添加子菜单使用查询所有父级菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<int> ids = new List<int>();
        public async Task<ResultDtoData> GetPar(int id)
        {
            var parlist = await dal.GetALL();
            List<int> ints = new List<int>();
            ids.Add(id);
            foreach (var item in parlist.Where(m => m.PowerId.Equals(id)))
            {
                if (item.PowerParentId != 0)
                    ints=GetPars(parlist, item.PowerParentId);
            }
            ids.Reverse();

            return new ResultDtoData { Result = Result.Success, Data = ids };
        }
        public List<int> GetPars(List<PowerData> data, int id)
        {

            foreach (var item in data.Where(m => m.PowerId.Equals(id)))
            {
                ids.Add(item.PowerId);

                 GetPars(data, item.PowerParentId);

            }
            return ids;
        }
        /// <summary>
        /// 菜单信息回显
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<ResultDtoData> FindAsync(int id)
        {
            var power = await dal.FindAsync(id);
            if (power == null)
            {
                return new ResultDtoData { Result = Result.Error, Message = "信息回显失败详情请联系管理员" };
            }
            else
            {
                return new ResultDtoData { Result = Result.Success, Message = "信息回显成功", Data = power };
            }
        }
        /// <summary>
        /// 修改菜单信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override ResultDtoData Update(PowerData t)
        {
            var data = dal.Update(t);
            if (data==null)
            {
                return new ResultDtoData { Result = Result.Error, Message = "修改菜单异常了详情请联系管理员" };
            }else if(data.PowerId==-1)
            {
                return new ResultDtoData { Result = Result.Warning, Message = "改菜单已存在无法修改" };
            }
            else
            {
                return new ResultDtoData { Result = Result.Success, Message = "修改成功" };
            }
        }

        public override async Task<ResultDtoData> LogicDeleteAsync(int id)
        {
            var data = await dal.LogicDeleteAsync(id);
            if (data==null)
            {
                return new ResultDtoData { Result = Result.Success, Message = "删除成功" };
            }
            else
            {
                return new ResultDtoData { Result = Result.Error, Message = "删除信息异常请重试或联系管理员" };
            }
        }
        #endregion
    }
}
