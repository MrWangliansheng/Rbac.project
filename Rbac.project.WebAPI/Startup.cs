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

        // �˷���������ʱ���á�ʹ�ô˷�����������ӵ������С�
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            #region JWT
            services.AddAuthentication("Bearer").AddJwtBearer(m =>
            {
                m.TokenValidationParameters = new TokenValidationParameters()
                {
                    //����Ĳ���Ҫ��ѭ3����Ҫ��+2����ѡ���������Ĺ���
                    //1���Ƿ�����Կ��֤����֤��Կ
                    ValidateIssuerSigningKey = true,//��֤������ǩ����Կ
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Kestrel:Key"])),

                    //ǩ֤������

                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Kestrel:issuer"],

                    ValidateAudience = true,
                    ValidAudience = Configuration["Kestrel:audience"],

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                };
            });
            #endregion

            #region ������֤��
            // Ĭ��ʹ���ڴ�洢��AddDistributedMemoryCache��
            services.AddCaptcha(Configuration);
            // ȫ������
            services.AddCaptcha(Configuration, option =>
            {
                option.CaptchaType = CaptchaType.WORD; // ��֤������
                option.CodeLength = 4; // ��֤�볤��, Ҫ����CaptchaType���ú�.  ������Ϊ�������ʽʱ�����ȴ�������ĸ���
                option.ExpirySeconds = 30; // ��֤�����ʱ��
                option.IgnoreCase = true; // �Ƚ�ʱ�Ƿ���Դ�Сд
                option.StoreageKeyPrefix = ""; // �洢��ǰ׺

                option.ImageOption.Animation = false; // �Ƿ����ö���
                option.ImageOption.FrameDelay = 1000; // ÿ֡�ӳ�,Animation=trueʱ��Ч, Ĭ��30

                option.ImageOption.Width = 150; // ��֤����
                option.ImageOption.Height = 50; // ��֤��߶�
                option.ImageOption.BackgroundColor = SKColors.White; // ��֤�뱳��ɫ

                option.ImageOption.BubbleCount = 20; // ��������
                option.ImageOption.BubbleMinRadius = 5; // ������С�뾶
                option.ImageOption.BubbleMaxRadius = 15; // �������뾶
                option.ImageOption.BubbleThickness = 1; // ���ݱ��غ��

                option.ImageOption.InterferenceLineCount = 2; // ����������

                option.ImageOption.FontSize = 36; // �����С
                option.ImageOption.FontFamily = DefaultFontFamilys.Instance.Actionj; // ����

                /* 
                 * ����ʹ��kaiti�������ַ��ɸ���ϲ�����ã����ܲ���ת�ַ�����ֻ��Ʋ������������
                 * ����֤������Ϊ��ARITHMETIC��ʱ����Ҫʹ�á�Ransom�����塣��������͵ȺŻ��Ʋ�������
                 */

                option.ImageOption.TextBold = true;// ���壬������2.0.3����
            });
            #endregion
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rbac.project.WebAPI", Version = "v1" });
                string path = AppContext.BaseDirectory + "Rbac.project.WebAPI.xml";
                c.IncludeXmlComments(path, true);

                #region Swagger С��
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "������Token",
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
            //ע��������
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

        // �˷���������ʱ���á�ʹ�ô˷�������HTTP����ܵ���
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("cors");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rbac.project.WebAPI v1"));
            }
            //˳�����һ��
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
