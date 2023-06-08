using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NPOI.Util;
using Rbac.project.Domain;
using Rbac.project.IRepoistory;
using Rbac.project.IService;
using Rbac.project.Repoistorys;
using Rbac.project.WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;

namespace Rbac.project.WebAPI.Filter
{
    /// <summary>
    /// 自定义授权过滤器
    /// </summary>
    public class AuthorizeFilter : Attribute, IAuthorizationFilter
    {
        private readonly IRoleRepoistory dal;
        private readonly RbacDbContext db;
        private readonly IMapper mapper;
        public AuthorizeFilter(IRoleRepoistory dal, IMapper _mapper, RbacDbContext db)
        {
            this.dal = dal;
            mapper = _mapper;
            this.db = db;
        }

        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //是否登录
            var islog = context.HttpContext.User.Identity.IsAuthenticated;
            if (!islog)
            {
                //访问状态码返回未授权401
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
            else
            {
                lock ("")
                {
                    //获取登录名
                    var UserName = context.HttpContext.User.Claims.Where(m => m.Type == "name").FirstOrDefault().Value;
                    //获取访问控制器接口
                    var teamplate = context.ActionDescriptor.AttributeRouteInfo.Template;
                    Type controllerType = context.HttpContext.GetType();
                    MethodInfo[] methods = controllerType.GetMethods();
                    foreach (MethodInfo method in methods)
                    {
                        object[] attributes = method.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);
                        if (attributes.Length > 0)
                        {

                            AllowAnonymousAttribute attribute = attributes[0] as AllowAnonymousAttribute;
                            // 处理特性
                        }
                    }
                    //获取token
                    string token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    var handler = new JwtSecurityTokenHandler();
                    //解析token获取Payload对象数据
                    var payload = handler.ReadJwtToken(token).Payload;
                    //获取Claim中存储的用户数据
                    var roleid = payload.Claims.Where(m => m.Type == "role").FirstOrDefault().Value.Split(",").Select(m => Convert.ToInt32(m)).ToList();

                    //var router = dal.GetRolePower(Convert.ToInt32(claims.Where(m => m.Type == "id").FirstOrDefault().Value), 0);
                    var list = db.RolePower.Where(m => db.Power.Where(m => m.PowerAPIUrl.Equals(teamplate.Replace("api", ""))).Select(s => s.PowerId).Contains(m.PowerID)).Select(m => m.RoleID).ToList();
                    bool state = true;
                    //if (!roleid.Any(m => list.Contains(m)))
                    //{
                    //    state = false;
                    //    context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                    //}
                }
            }
        }
    }
}
