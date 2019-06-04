using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;

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

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AnyOrigin", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        // .AllowCredentials()
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
                //options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                //{
                //    OnRedirectToLogin = async ctx =>
                //    {
                //        if (ctx.Request.Path.Value.Contains("/api/") &&
                //          ctx.Response.StatusCode == 200)
                //        {
                //            ctx.Response.StatusCode = 401;
                //        }
                //        else
                //        {
                //            ctx.Response.Redirect(ctx.RedirectUri);
                //        }
                //        await Task.Yield();
                //    }
                //};
            })
                .AddUserStore<QIQOUserStore<User>>()
                .AddRoleStore<QIQORoleStore<Role>>()
                .AddUserManager<QIQOUserManager>()
                .AddRoleManager<QIQORoleManager>();
            // .AddDefaultTokenProviders();

            //services.AddAuthentication().AddCookie();

            //services.ConfigureApplicationCookie(options =>
            //{
            //    //options.Cookie.Expiration = TimeSpan.FromDays(150);
            //    //options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
            //    //// options.Cookie.Name = "QIQO.Business.Cookie";
            //    //options.LoginPath = "/Account/Login";
            //    //options.AccessDeniedPath = "/Account/Forbidden";

            //    options.Events = new CookieAuthenticationEvents()
            //    {
            //        OnRedirectToLogin = async ctx =>
            //        {
            //            if (ctx.Request.Path.Value.Contains("/api/") &&
            //              ctx.Response.StatusCode == 200)
            //            {
            //                ctx.Response.StatusCode = 401;
            //            }
            //            else
            //            {
            //                ctx.Response.Redirect(ctx.RedirectUri);
            //            }
            //            await Task.Yield();
            //        }
            //    };
            //});

            services.AddMvc();

            services.AddSingleton<IServiceFactory>(new ServiceFactory(services));
            services.AddSingleton<LogTesting>();
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment en)
        { 
            app.UseAuthentication();
            // app.UseCookieAuthentication();

            app.UseCors("AnyOrigin");
            app.UseMvc();
        }
    }
}
