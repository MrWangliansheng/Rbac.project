using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Rbac.project.IRepoistory.Eextend;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.project.Repoistorys.Eextend
{
    public class ReflectRepoistory<TEntity>:IReflectRepoistory<TEntity> where TEntity : class
    {
        public readonly RbacDbContext db;
        public readonly IHttpContextAccessor http;
        private readonly IMapper mapper;
        public ReflectRepoistory(RbacDbContext db, IHttpContextAccessor http, IMapper mapper)
        {
            this.db = db;
            this.http = http;
            this.mapper = mapper;
        }

        public void CreateAudit(TEntity entity, string name)
        {
            //name = http.HttpContext.User.Identity.Name;
            DateTime now=DateTime.Now;

            Type type= entity.GetType();

            //获取审计字段属性
            var CreateUser = type.GetProperty("MesgCreateUser");
            var CreateDate = type.GetProperty("MsgCreateTime");

            CreateUser.SetValue(entity, name);
            CreateDate.SetValue(entity, now);

            db.Update(entity);
            db.SaveChanges();
        }

        public void DeleteAudit(TEntity entity, string name)
        {
           
            DateTime now = DateTime.Now;

            Type type = entity.GetType();
            

            //获取审计字段属性
            var DeleteUser = type.GetProperty("MegDeleteUser");
            var DeleteDate = type.GetProperty("MsgDeleteTime");

            DeleteUser.SetValue(entity, name);
            DeleteDate.SetValue(entity, now);

            db.Set<TEntity>().Update(entity);
            db.SaveChanges();
        }

        public void UpdateAudit(TEntity entity, string name)
        {
            DateTime now = DateTime.Now;

            Type type = entity.GetType();

            //获取审计字段属性
            var UpdateUser = type.GetProperty("MsgUpdateUser");
            var UpdateDate = type.GetProperty("MegUpdateTipme");

            UpdateUser.SetValue(entity, name);
            UpdateDate.SetValue(entity, now);

            db.Update(entity);
            db.SaveChanges();
        }
    }
}
