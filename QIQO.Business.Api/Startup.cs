using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Routing;
using System;

using QIQO.Business.Core;
using QIQO.Business.Client.Contracts;
using QIQO.Business.Services;
using QIQO.Business.Client.Proxies;
using QIQO.Business.Client.Entities;
using QIQO.Business.Identity;

namespace QIQO.Business.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDistributedMemoryCache();
            services.AddCors(options =>
            {
                options.AddPolicy("AnyOrigin", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.User.AllowedUserNameCharacters = null;
                options.User.RequireUniqueEmail = true;
                //options.Lockout.AllowedForNewUsers = false;
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.SignIn.RequireConfirmedEmail = false;
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/Login";
                options.Cookies.ApplicationCookie.AccessDeniedPath = "/Account/Forbidden";
            })
                .AddUserStore<QIQOUserStore<User>>()
                .AddRoleStore<QIQORoleStore<Role>>()
                .AddUserManager<QIQOUserManager>()
                .AddRoleManager<QIQORoleManager>()
                .AddDefaultTokenProviders();

            services.AddMvc().AddJsonOptions
                (
                    opt => { opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); }
                );

            services.AddSingleton<IServiceFactory>(new ServiceFactory(services));
            services.AddTransient<IIdentityUserService, IdentityUserClient>();
            services.AddTransient<IIdentityRoleService, IdentityRoleClient>();
            services.AddTransient<IAccountService, AccountClient>();
            services.AddTransient<IAddressService, AddressClient>();
            services.AddTransient<ICompanyService, CompanyClient>();
            services.AddTransient<IEmployeeService, EmployeeClient>();
            services.AddTransient<IEntityProductService, EntityProductClient>();
            services.AddTransient<IFeeScheduleService, FeeScheduleClient>();
            services.AddTransient<IOrderService, OrderClient>();
            services.AddTransient<IInvoiceService, InvoiceClient>();
            services.AddTransient<IProductService, ProductClient>();
            services.AddTransient<ITypeService, TypeClient>();
            services.AddTransient<IEntityService, EntityService>();

            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseIdentity();
            app.UseCookieAuthentication();

            app.UseCors("AnyOrigin");
            app.UseMvc(ConfigureRoutes);
        }
        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            //routeBuilder.MapRoute("Product", "api/{controller}/{product_key?}", new { product_key = RouteP);
        }
    }
}
