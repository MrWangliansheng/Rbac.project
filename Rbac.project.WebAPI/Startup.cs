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
using Rbac.project.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rbac.project.WebAPI", Version = "v1" });
                string path = AppContext.BaseDirectory + "Rbac.project.WebAPI.xml";
                c.IncludeXmlComments(path,true);
            });

            services.AddCors(m =>
            {
                m.AddPolicy("cors", op => op
                .WithOrigins(Configuration["CorsPolicy:Cors"])
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("X-Pagination"));
            });

            services.AddDbContext<RbacDbContext>(m => m.UseSqlServer(Configuration.GetConnectionString("ConStr"), m => m.MigrationsAssembly("Rbac.project.WebAPI")));

            services.AddScoped(typeof(IBaseRepoistory<>), typeof(BaseRepoistory<>));
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserRepoistory, UserRepoistory>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IRoleRepoistory, RoleRepoistory>();

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
