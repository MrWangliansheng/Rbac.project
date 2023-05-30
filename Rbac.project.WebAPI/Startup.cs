using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Rbac.project.Domain;
using Rbac.project.IRepoistory;
using Rbac.project.IService;
using Rbac.project.Repoistorys;
using Rbac.project.Repoistorys.AutoMapper;
using Rbac.project.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSRedis;
using Rbac.project.IRepoistory.LogOperation;
using Rbac.project.Repoistory.LogOperation;
using Rbac.project.Domain.Dto;
using Rbac.project.Domain.DataDisplay;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.ComponentModel;

namespace Rbac.project.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // 此方法由运行时调用。使用此方法将服务添加到容器中。
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            #region JWT
            services.AddAuthentication("Bearer").AddJwtBearer(m =>
            {
                m.TokenValidationParameters = new TokenValidationParameters()
                {
                    //这里的参数要遵循3（必要）+2（可选）个参数的规则
                    //1、是否开启密钥认证，验证密钥
                    ValidateIssuerSigningKey = true,//验证发行者签名密钥
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Kestrel:Key"])),

                    //签证发行人

                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Kestrel:issuer"],

                    ValidateAudience = true,
                    ValidAudience = Configuration["Kestrel:audience"],

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                };
            });
            #endregion


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rbac.project.WebAPI", Version = "v1" });
                string path = AppContext.BaseDirectory + "Rbac.project.WebAPI.xml";
                c.IncludeXmlComments(path, true);

                #region Swagger 小锁
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "请输入Token",
                    Name="Authorization",
                    In=ParameterLocation.Header,
                    Type=SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer",
                            }
                        },
                        new string[]{ }
                    }
                });
                #endregion
            });
            services.AddSingleton<CSRedisClient>(new CSRedisClient("127.0.0.1:6379"));
            services.AddCors(m =>
            {
                m.AddPolicy("cors", op => op
                .WithOrigins(Configuration["CorsPolicy:Cors"])
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("X-Pagination"));
            });



            services.AddDbContext<RbacDbContext>(m => m.UseSqlServer(Configuration.GetConnectionString("ConStr"), m => m.MigrationsAssembly("Rbac.project.WebAPI")));

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped(typeof(IBaseRepoistory<>), typeof(BaseRepoistory<>));
            services.AddScoped(typeof(IBaseService<ResultDtoData, UserData>), typeof(BaseService<ResultDtoData, UserData>));


            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserRepoistory, UserRepoistory>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IRoleRepoistory, RoleRepoistory>();
            services.AddScoped<IPowerRepoistory, PowerRepoistory>();
            services.AddScoped<IPowerService, PowerService>();

            services.AddScoped<ILogDataRepoistory, LogDataRepoistory>();

        }

        // 此方法由运行时调用。使用此方法配置HTTP请求管道。
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("cors");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rbac.project.WebAPI v1"));
            }
            //顺序必须一致
            app.UseHttpsRedirection();//1

            app.UseRouting();//2

            app.UseAuthentication();//3

            app.UseAuthorization();//4
            //app.UseRouting();

            //app.UseAuthorization();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
