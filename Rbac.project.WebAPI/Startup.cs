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
using Rbac.project.Domain.Dto;
using Rbac.project.Domain.DataDisplay;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.ComponentModel;
using Lazy.Captcha.Core.Generator;
using SkiaSharp;
using Lazy.Captcha.Core;
using Rbac.project.IRepoistory.Eextend;
using Rbac.project.Repoistory.Eextend;
using Rbac.project.Repoistorys.Eextend;
using Rbac.project.IService.Eextend;
using Rbac.project.Service.Eextend;

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

            #region 生成验证码
            // 默认使用内存存储（AddDistributedMemoryCache）
            services.AddCaptcha(Configuration);
            // 全部配置
            services.AddCaptcha(Configuration, option =>
            {
                option.CaptchaType = CaptchaType.WORD; // 验证码类型
                option.CodeLength = 4; // 验证码长度, 要放在CaptchaType设置后.  当类型为算术表达式时，长度代表操作的个数
                option.ExpirySeconds = 30; // 验证码过期时间
                option.IgnoreCase = true; // 比较时是否忽略大小写
                option.StoreageKeyPrefix = ""; // 存储键前缀

                option.ImageOption.Animation = false; // 是否启用动画
                option.ImageOption.FrameDelay = 1000; // 每帧延迟,Animation=true时有效, 默认30

                option.ImageOption.Width = 150; // 验证码宽度
                option.ImageOption.Height = 50; // 验证码高度
                option.ImageOption.BackgroundColor = SKColors.White; // 验证码背景色

                option.ImageOption.BubbleCount = 20; // 气泡数量
                option.ImageOption.BubbleMinRadius = 5; // 气泡最小半径
                option.ImageOption.BubbleMaxRadius = 15; // 气泡最大半径
                option.ImageOption.BubbleThickness = 1; // 气泡边沿厚度

                option.ImageOption.InterferenceLineCount = 2; // 干扰线数量

                option.ImageOption.FontSize = 36; // 字体大小
                option.ImageOption.FontFamily = DefaultFontFamilys.Instance.Actionj; // 字体

                /* 
                 * 中文使用kaiti，其他字符可根据喜好设置（可能部分转字符会出现绘制不出的情况）。
                 * 当验证码类型为“ARITHMETIC”时，不要使用“Ransom”字体。（运算符和等号绘制不出来）
                 */

                option.ImageOption.TextBold = true;// 粗体，该配置2.0.3新增
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
            services.AddSingleton<CSRedisClient>(new CSRedisClient(Configuration["ConnectionStrings:RedisCache"]));
            services.AddCors(m =>
            {
                m.AddPolicy("cors", op => op
                .WithOrigins(Configuration["CorsPolicy:Cors"])
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("X-Pagination"));
            });
            //注入上下文
            services.AddHttpContextAccessor();

            services.AddDbContext<RbacDbContext>(m => m.UseSqlServer(Configuration.GetConnectionString("ConStr"), m => m.MigrationsAssembly("Rbac.project.WebAPI")));

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped(typeof(IBaseRepoistory<>), typeof(BaseRepoistory<>));
            services.AddScoped(typeof(IBaseService<ResultDtoData, UserData>), typeof(BaseService<ResultDtoData, UserData>));
            services.AddScoped(typeof(IReflectRepoistory<>), typeof(ReflectRepoistory<>));
            services.AddScoped(typeof(IReflectService<>), typeof(ReflectService<>));
            services.AddScoped<IGetHeaderToken, GetHeaderToken>();

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
