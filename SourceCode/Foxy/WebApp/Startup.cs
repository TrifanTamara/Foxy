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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using WebApp.Services;

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
            services.AddTransient<IVocabularItemRepository, VocabularItemRepository>();

            services.AddScoped<IMainService, MainService>();

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("FoxyConnection")));

            services.AddSwaggerDocumentation();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.AccessDeniedPath = new PathString("/Login"); //cahnge maybe for unothorized page
                        options.LoginPath = new PathString("/Login");
                        options.CookieName = "FoxyCookie";
                        options.ExpireTimeSpan = new TimeSpan(2, 0, 0);

                    });

            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(DefaultControllerFilter));
                    options.Filters.Add(typeof(AuthorizationFilter));
                    //options.Filters.Add(typeof(RegisterValidator));
                }
            ).AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>())
            .AddSessionStateTempDataProvider();

            HttpClient httpClient = new HttpClient();
            services.AddSingleton<HttpClient>(httpClient); // note the singleton
            
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


            if (env.IsDevelopment())
            {
                //.... rest of app configuration
                app.UseSwaggerDocumentation();
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "vocabular_route",
                    template: "{controller=Vocabular}/{action=Radical}/{name}"
                    );
            });

        }
    }
}
