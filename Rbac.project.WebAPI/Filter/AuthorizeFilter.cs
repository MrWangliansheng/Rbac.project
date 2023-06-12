using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rbac.project.Repoistorys;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace Rbac.project.WebAPI.Filter
{
    /// <summary>
    /// 自定义授权过滤器
    /// </summary>
    public class AuthorizeFilter : Attribute, IAuthorizationFilter
    {
        private readonly RbacDbContext db;
        private readonly IMapper mapper;
        public AuthorizeFilter(IMapper _mapper, RbacDbContext db)
        {
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
            if (!context.ActionDescriptor.EndpointMetadata.Any(m => m is IAllowAnonymous))
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
                        #region 注释
                        //Type controllerType = context.HttpContext.GetType();
                        //MethodInfo[] methods = controllerType.GetMethods();
                        //foreach (MethodInfo method in methods)
                        //{
                        //    object[] attributes = method.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);
                        //    if (attributes.Length > 0)
                        //    {

                        //        AllowAnonymousAttribute attribute = attributes[0] as AllowAnonymousAttribute;
                        //        // 处理特性
                        //    }
                        //}
                        ////获取token
                        //string token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                        //var handler = new JwtSecurityTokenHandler();
                        ////解析token获取Payload对象数据
                        //var payload = handler.ReadJwtToken(token).Payload;
                        ////获取Claim中存储的用户数据
                        //var roleid = payload.Claims.Where(m => m.Type == "role").FirstOrDefault().Value.Split(",").Select(m => Convert.ToInt32(m)).ToList();
                        #endregion

                        //获取Claim中存储的用户数据
                        var roleid = context.HttpContext.User.Claims.Where(m => m.Type == ClaimTypes.Role).FirstOrDefault().Value.Split(",").Select(m => Convert.ToInt32(m)).ToList();
                        //查询拥有当前接口权限的角色信息
                        var list = db.RolePower.Where(m => db.Power.Where(m => m.PowerAPIUrl.Equals(teamplate.Replace("api", ""))).Select(s => s.PowerId).Contains(m.PowerID)).Select(m => m.RoleID).ToList();
                        bool state = true;
                        //如果当前登录用户的角色不允许访问接口状态码为403
                        if (!roleid.Any(m => list.Contains(m)))
                        {
                            state = false;
                            context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                        }
                    }
                }
            }
        }
    }
}
