using System;
using System.Text;
using Business;
using Data.Domain.Interfaces;
using Data.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using WebApp.Filter;
using WebApp.DTOs_Validators;
using System.Net.Http;

namespace WebApp
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
            services.AddTransient<IDatabaseContext, DatabaseContext>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IVocabularTempRepository, VocabularTempRepository>();
            services.AddTransient<IVocabRelRepository, VocabRelRepository>();

            services.AddTransient<VocabularTempRepository>();
            services.AddTransient<UsersRepository>();
            services.AddTransient<PopulateDb.PopulateDb>();

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("FoxyConnection")));

            services.AddSwaggerDocumentation();

            services.AddMvc();
            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(DefaultControllerFilter));
                    options.Filters.Add(typeof(AuthorizationFilter));
                    //options.Filters.Add(typeof(RegisterValidator));
                }
            ).AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>()).AddSessionStateTempDataProvider();

            HttpClient httpClient = new HttpClient();
            services.AddSingleton<HttpClient>(httpClient); // note the singleton


            //            services.AddSession(options =>
            //            {
            //                // Set a short timeout for easy testing.
            //                options.Cookie.Name = ".Foxy.Session";
            //                options.IdleTimeout = TimeSpan.MaxValue;
            //                options.Cookie.HttpOnly = true;
            //            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options=>
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        SaveSigninToken = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            //            app.UseSwaggerUI(c =>
            //            {
            //                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Foxy V1");
            //            });
            if (env.IsDevelopment())
            {
                //.... rest of app configuration
                app.UseSwaggerDocumentation();
            }

            app.UseStaticFiles();
            //app.UseSession();
            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
