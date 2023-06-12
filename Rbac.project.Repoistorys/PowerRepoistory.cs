using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Rbac.project.Domain;
using Rbac.project.Domain.DataDisplay;
using Rbac.project.Domain.Dto;
using Rbac.project.Domain.Enum;
using Rbac.project.Domain.ParentIdAll;
using Rbac.project.IRepoistory;
using Rbac.project.IRepoistory.Eextend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Rbac.project.Repoistorys
{
    public class PowerRepoistory : BaseRepoistory<PowerData>, IPowerRepoistory
    {
        private readonly RbacDbContext db;
        private readonly ILogDataRepoistory logdata;
        private readonly IMapper mapper;
        private readonly IReflectRepoistory<Power> reflect;
        private readonly IHttpContextAccessor http;
        string name = "";
        public PowerRepoistory(RbacDbContext db, ILogDataRepoistory logdata, IMapper mapper, IReflectRepoistory<Power> reflect, IHttpContextAccessor http) : base(db)
        {
            this.logdata = logdata;
            this.db = db;
            this.mapper = mapper;
            this.reflect = reflect;
            this.http = http;
            name = http.HttpContext.User.Claims.Where(m=>m.Type=="name").FirstOrDefault().Value;
        }
        #region 权限菜单
        /// <summary>
        /// 添加权限菜单
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override async Task<PowerData> InsertAsync(PowerData t)
        {
            var star = await db.Database.BeginTransactionAsync();
            try
            {
                var power = db.Power.Where(m => m.PowerParentId.Equals(t.PowerParentId) & m.PowerName.Equals(t.PowerName)).ToList();
                if (power.Count > 0)
                {
                    t.PowerId = -1;
                    return t;
                }
                var pwdata = mapper.Map<Power>(t);
                await db.AddAsync(pwdata);
                var rolepower = new RolePower();
                rolepower.RoleID = 1;
                rolepower.PowerID = pwdata.PowerId;
                var rolepoweridall = new RolePowerIdAll();
                rolepoweridall.RoleID = 1;
                rolepoweridall.PowerIdAll = pwdata.PowerParentIdAll + "," + pwdata.PowerId;
                await db.AddAsync(rolepower);
                await db.AddAsync(rolepoweridall);
                logdata.CreateLog("/Repoistorys/InsertAsync", "添加权限菜单成功", "");
                await db.SaveChangesAsync();
                //修改审计字段
                reflect.CreateAudit(pwdata, name);
                await star.CommitAsync();
                return t;
            }
            catch (Exception ex)
            {
                await star.RollbackAsync();
                logdata.CreateLog("/Repoistorys/InsertAsync", ex.Message, "");
                return null;
            }
        }
        /// <summary>
        /// 反填权限菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<PowerData> FindAsync(int id)
        {
            try
            {
                var power = await db.Power.FindAsync(id);
                var data = mapper.Map<PowerData>(power);
                logdata.CreateLog("/PowerRopeistory/FindAsync", "反填菜单信息", "");
                return data;
            }
            catch (Exception ex)
            {
                logdata.CreateLog("/PowerRopeistory/FindAsync", ex.Message, "");
                return null;
            }

        }
        /// <summary>
        /// 修改菜单信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override PowerData Update(PowerData t)
        {
            var tran = db.Database.BeginTransaction();
            try
            {
                var list = db.Power.Where(m => m.PowerParentId.Equals(t.PowerParentId) & m.PowerName.Equals(t.PowerName)).FirstOrDefault();
                if (list != null)
                {
                    var power = mapper.Map(t, list);
                    db.Update(power);
                    db.SaveChanges();
                    logdata.CreateLog("/PowerRepoistory/Update", "修改菜单信息", "");
                    //修改审计字段
                    reflect.UpdateAudit(power, name);
                    tran.Commit();
                    return t;
                }
                else
                {
                    list = db.Power.Where(m => m.PowerName.Equals(t.PowerName)).FirstOrDefault();
                    if (list != null)
                    {
                        t.PowerId = -1;
                        return t;
                    }
                    else
                    {
                        var power = mapper.Map<Power>(t);
                        db.Update(power);
                        db.SaveChanges();
                        logdata.CreateLog("/PowerRepoistory/Update", "修改菜单信息", "");
                        //修改审计字段
                        reflect.UpdateAudit(power, name);
                        tran.Commit();
                        return t;
                    }
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                logdata.CreateLog("/PowerRepoistory/Update", ex.Message, "");
                return null;
            }
        }
        /// <summary>
        /// 逻辑删除菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<PowerData> LogicDeleteAsync(int id)
        {
            try
            {
                var power = await db.Power.FindAsync(id);
                power.PowerIsDelete = true;
                logdata.CreateLog("/PowerRepoistory/LogicDeleteAsync", "删除菜单信息", "");
                //修改审计字段
                reflect.DeleteAudit(power, name);
                return mapper.Map<PowerData>(power);
            }
            catch (Exception ex)
            {
                logdata.CreateLog("/PowerRepoistory/LogicDeleteAsync", ex.Message, "");
                return null;
            }

        }
        #endregion

        #region 菜单级联绑定
        /// <summary>
        /// 权限菜单级联绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultDtoData GetPowerTree(int id = 0)
        {
            try
            {
                var list = db.Set<Power>().Where(m => m.PowerParentId.Equals(id) && m.PowerIsDelete.Equals(false)).ToList();
                var treelist = new List<TreeDto>();
                foreach (var item in list)
                {
                    TreeDto tree = new TreeDto();
                    tree.value = item.PowerId;
                    tree.label = item.PowerName;
                    tree.children = GetTreeDto(item.PowerId).Count > 0 ? GetTreeDto(item.PowerId) : null;
                    treelist.Add(tree);
                }
                return new ResultDtoData { Result = Result.Success, Message = "菜单级联查询成功", Data = treelist };
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<TreeDto> GetTreeDto(int id)
        {
            try
            {
                var list = db.Set<Power>().Where(m => m.PowerParentId.Equals(id) && m.PowerIsDelete.Equals(false)).ToList();
                var treelist = new List<TreeDto>();
                foreach (var item in list)
                {
                    TreeDto tree = new TreeDto();
                    tree.value = item.PowerId;
                    tree.label = item.PowerName;
                    tree.children = GetTreeDto(item.PowerId).Count > 0 ? GetTreeDto(item.PowerId) : null;
                    treelist.Add(tree);
                }
                return treelist;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 菜单表格树形显示

        public async Task<ResultDtoData> GetPowerTreeTableLevelone(int id = 0)
        {
            var list = await db.Power.Where(m => m.PowerIsDelete.Equals(false) & m.PowerParentId.Equals(id)).ToListAsync();
            foreach (var item in list)
            {
                item.children = await GetPowerTreeSublevel(item.PowerId);
            }

            return new ResultDtoData { Result = Result.Success, Message = "数据查询成功", Data = list };
        }
        public async Task<List<Power>> GetPowerTreeSublevel(int id)
        {
            var list = await db.Power.Where(m => m.PowerIsDelete.Equals(false) & m.PowerParentId.Equals(id)).ToListAsync();
            foreach (var item in list)
            {
                item.children = await GetPowerTreeSublevel(item.PowerId);
            }
            return list;
        }
        /// <summary>
        /// 权限菜单级联选择绑定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ResultDtoData GetPowerTreeData(int id = 0)
        {
            try
            {
                var list = db.Power.Where(m => m.PowerParentId.Equals(id) && m.PowerIsDelete.Equals(false)).ToList();
                var treelist = new List<TreeDto>();
                foreach (var item in list)
                {
                    TreeDto tree = new TreeDto();
                    tree.value = item.PowerId;
                    tree.label = item.PowerName;
                    tree.children = GetPowerTreeDataSublevel(item.PowerId).Count > 0 ? GetPowerTreeDataSublevel(item.PowerId) : null;
                    treelist.Add(tree);
                }

                logdata.CreateLog("/PowerRepoistory/GetPowerTreeData", "查询权限菜单成功", "");
                return new ResultDtoData { Result = Result.Success, Message = "查询权限菜单成功", Data = treelist };
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<TreeDto> GetPowerTreeDataSublevel(int id)
        {
            var list = db.Power.Where(m => m.PowerParentId.Equals(id) && m.PowerIsDelete.Equals(false)).ToList();
            var treelist = new List<TreeDto>();
            foreach (var item in list)
            {
                TreeDto tree = new TreeDto();
                tree.value = item.PowerId;
                tree.label = item.PowerName;
                tree.children = GetPowerTreeDataSublevel(item.PowerId).Count > 0 ? GetPowerTreeDataSublevel(item.PowerId) : null;
                treelist.Add(tree);
            }
            return treelist;
        }
        #endregion
        #region 权限菜单枚举列表
        public ResultDtoData GetPowerEnum()
        {
            var list = new List<object>();
            foreach (var item in Enum.GetValues<PowerEnum>())
            {
                var obj = (DescriptionAttribute)item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false)[0];
                list.Add(new
                {
                    id = item,
                    name = obj.Description
                });
            }
            return new ResultDtoData { Result = Result.Success, Message = "权限菜单枚举列表查询成功", Data = list };
        }

        public override async Task<List<PowerData>> GetALL()
        {
            var parlist = await db.Power.Where(m => m.PowerIsDelete.Equals(false)).ToListAsync();
            return mapper.Map<List<PowerData>>(parlist);
        }
        #endregion

    }
}
