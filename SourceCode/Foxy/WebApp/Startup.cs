using System;
using Business;
using Data.Domain.Interfaces;
using Data.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Filter;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using WebApp.Services;
using Business.Template;
using Data.Domain.Interfaces.UserRelated;
using Business.UserRelated;
using Data.Domain.Interfaces.Template;

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
            services.AddTransient<IUsersRepository, UsersRepo>();
            services.AddTransient<IVocabularTempRepo, VocabularTempRepo>();
            services.AddTransient<IVocabRelRepo, VocabRelRepo>();
            services.AddTransient<IVocabularItemRepo, VocabularItemRepo>();
            
            services.AddTransient<IAnswerTempRepo, AnswerTempRepo>();
            services.AddTransient<IQuestionTempRepo, QuestionTempRepo>();
            services.AddTransient<IQuestionItemRepo, QuestionItemRepo>();
            services.AddTransient<IFormularItemRepo, FormularItemRepo>();
            services.AddTransient<IFormularTempRepo, FormularTempRepo>();
            services.AddTransient<ICommonRepo, CommonRepo>();
            services.AddTransient<IWordsElemRelRepo, WordsElemRelRepo>();

            services.AddScoped<IMainService, MainService>();

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("FoxyConnection")));

            services.AddSwaggerDocumentation();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.AccessDeniedPath = new PathString("/Login");
                        options.LoginPath = new PathString("/Login");
                        options.CookieName = "FoxyCookie";
                        options.ExpireTimeSpan = new TimeSpan(2, 0, 0);
                    });
            
            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(DefaultControllerFilter));
                    options.Filters.Add(typeof(AuthorizationFilter));
                }
            ).AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>())
            .AddSessionStateTempDataProvider();
            
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
            });

        }
    }
}
