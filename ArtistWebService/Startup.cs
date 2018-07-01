using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DataAccessLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ArtistWebService
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment _env)
        {
            Configuration = configuration;
            env = _env;
        }

        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddTransient<IWebService, WebService>();
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration["Data:Connection"]));
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(Configuration["Data:IConnection"]));
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();



            services.AddAutoMapper();
            services.AddMvc(option =>
            {
                if (!env.IsProduction())
                {
                    option.SslPort = 44370;
                }
                option.Filters.Add(new RequireHttpsAttribute());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,IServiceProvider provider, DataContext context,
            IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseAuthentication();
            app.UseStaticFiles();
            context.Seed();


           SeedIdentity.SeedUser(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);

            app.UseMvc();

            
        }
    }
}
