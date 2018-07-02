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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using ArtistWebService.RepoService;

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
            services.AddTransient<IArtistRepo, ArtistRepo>();
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration["Data:Connection"]));
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(Configuration["Data:IConnection"]));
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

            // configure

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = ctx =>
                 {
                     if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == StatusCodes.Status200OK)
                     {
                         ctx.Response.Clear();
                         ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                         return Task.FromResult<object>(null);
                     }
                     ctx.Response.Redirect(ctx.RedirectUri);
                     return Task.CompletedTask;
                 };

                options.Events.OnRedirectToAccessDenied = ctx =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == StatusCodes.Status200OK)
                    {
                        ctx.Response.Clear();
                        ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.FromResult<object>(null);
                    }
                    ctx.Response.Redirect(ctx.RedirectUri);
                    return Task.CompletedTask;
                };
            });

            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Auth:Issuer"],
                    ValidAudience = Configuration["Auth:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Key"])),
                    ClockSkew = TimeSpan.Zero,

                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true
                };
            });




            services.AddAutoMapper();
            // adding CORS
            services.AddCors(config =>
            {
                // for LimCode Kade
                config.AddPolicy("limAdmin", p =>
                {
                    p.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
                config.AddPolicy("public", p =>
                 {
                     p.AllowAnyHeader()
                     .WithMethods("Get")
                     .AllowAnyOrigin();
                 });
            });

            services.AddMvc(option =>
            {
                if (!env.IsProduction())
                {
                    option.SslPort = 44370;
                }
                option.Filters.Add(new RequireHttpsAttribute());
            })
            .AddJsonOptions(opt=>
            
                opt.SerializerSettings.ReferenceLoopHandling= Newtonsoft.Json.ReferenceLoopHandling.Serialize
            );


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",new  Info
                {
                    Title = "Artist Micro-Services", Version = "v1",
                    Description="Building secured Restful web service with EFCore 2 " +
                    "using token, cookie authentification and Https",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact() { Name="Kade", Email="kaderderk@gmail.com"}
                });
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json","Artists web service");
            });
        }
    }
}
