using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Nadlan.Models.Security;
using Nadlan.Repositories;
using System;
using System.Text;
using Microsoft.OpenApi.Models;

namespace Nadlan
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            if (!_env.IsDevelopment())
            {
                services.Configure<MvcOptions>(o =>
                o.Filters.Add(new RequireHttpsAttribute()));
            }

            JwtSettings jwtSettings = GetJwtSettings();

            services.AddSingleton<JwtSettings>(jwtSettings);

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = "JwtBearer";
            //    options.DefaultChallengeScheme = "JwtBearer";
            //})
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(jwtSettings.MinutesToExpiration)
                };
            });

            //services.AddAuthorization(cnf =>
            //{
            //    cnf.AddPolicy("CanViewTransactions", p =>
            //    {
            //        p.RequireClaim("CanViewTransactions", "true");
            //    });
            //});


            services.AddDbContext<NadlanConext>(options => options.UseSqlServer(Configuration.GetConnectionString("Nadlan")));
            services.AddAutoMapper(typeof(Startup));
            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();


            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
               // c.RoutePrefix = string.Empty;
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //app.UseStaticFiles();
            app.UseSpaStaticFiles();

            //app.UseCors(
            //    options => {
            //        options.WithOrigins("http://localhost:4200")
            //        .AllowAnyMethod()
            //        .AllowAnyHeader();
            //    });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200/");
                    //spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        public JwtSettings GetJwtSettings()
        {
            JwtSettings jwtSettings = new JwtSettings
            {
                Key = Configuration["JwtSettings:key"],
                Audience = Configuration["JwtSettings:audience"],
                Issuer = Configuration["JwtSettings:issuer"],
                MinutesToExpiration = Convert.ToInt32(Configuration["JwtSettings:minutesToExpiration"])
            };

            return jwtSettings;
        }
    }
}
