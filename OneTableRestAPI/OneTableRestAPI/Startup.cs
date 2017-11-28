using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneTableRestAPI.Models;

using EzCoreKit.AspNetCore;
using EzCoreKit.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace OneTableRestAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AnonChatContext>(options => {// 加入EFCore DI
                options.UseSqlServer(Configuration["ConnectionString"]);//連線字串從appsetting.json抓
            });

            services.AddEzJwtBearerWithDefaultSchema(//正常來講這個KEY可以從憑證取
                new SymmetricSecurityKey(Configuration["SecretKey"].ToHash<MD5>()),
                "AnonChat");

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
