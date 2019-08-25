using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurTrace.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OurTrace.Data.Identity.Models;
using OurTrace.Services.Abstraction;
using OurTrace.Services;
using AutoMapper;
using GeekLearning.Storage;
using GeekLearning.Storage.Configuration;

namespace OurTrace.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration,
            IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        private readonly IHostingEnvironment hostingEnvironment;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddStorage(this.Configuration.GetSection("Storage"))
                .AddFileSystemStorage(hostingEnvironment.ContentRootPath)
                .AddFileSystemExtendedProperties();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<OurTraceDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b =>
                    b.MigrationsAssembly("OurTrace.App"));

                });

            services.AddIdentity<OurTraceUser, IdentityRole>(options =>
                  {
                      options.Password.RequireDigit = false;
                      options.Password.RequiredLength = 6;
                      options.Password.RequireUppercase = false;
                      options.Password.RequireNonAlphanumeric = false;
                      options.Password.RequireLowercase = false;
                  })
                .AddEntityFrameworkStores<OurTraceDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Authentication/User");

            services.AddScoped<IRelationsService, RelationsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<INotificationService, NotificationService>();


            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute("Profile", "Profile/{*username}",
                    defaults: new { controller = "User", action = "Profile" });

                routes.MapRoute("Group", "Group/{*name}",
                    defaults: new { controller = "Group", action = "Open" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
