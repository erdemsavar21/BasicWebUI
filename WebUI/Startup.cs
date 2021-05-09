using System;
using System.Threading.Tasks;
using Business.ServiceAdapters.AspIdentity.DbContext;
using Business.ServiceAdapters.AspIdentity.Model;
using Business.ValidationRules.CustomValidation;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace WebUI
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


         

            //services.AddControllersWithViews();
            services.AddMvc();

            services.AddDbContext<AppIdentityDbContext>(options => options.UseMySql(Configuration["IdentityDbConnection"], b => b.MigrationsAssembly("WebUI")));
            services.AddIdentity<AppIdentityUser, AppIdentityRole>(options =>
            {
                options.Password.RequireDigit = true; //sayisal karakter
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Lockout.MaxFailedAccessAttempts = 5; // sifreyi 5 kez yanlis girerse lock liyor. Bu sekilde sadece verince calismiyor. UserManager ile kullanici Lock lanabiliyor
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
               
                //options.User.AllowedUserNameCharacters = "aäbcdefghijklmnoöpqrstuüvwxyzAÄBCDEFGHIJKLMNOPQRSßTUÜVWXYZ0123456789-._"; //kullanici adi sadece  bu karakterlerden olusabilir.
            }).AddPasswordValidator<CustomPasswordValidator>().AddUserValidator<CustomUserValidator>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

            CookieBuilder cookieBuilder = new CookieBuilder();
            cookieBuilder.Name = "ECommerce";
            cookieBuilder.HttpOnly = true; //Client tarafinda cookie okumayacaksam güvenlik icin true yapmak gerekiyor
            
            cookieBuilder.SameSite = SameSiteMode.Strict; // Strict dersek sadece sistem üzerinden gelen Cookie leri sunucu kabul eder Lax dersek baska sitelerden gelen bizim cookie leride kabul eder.
                                                        // Eger baska bir site ye cookie aktarimi ihtiyaci yoksa güvenlik icin mutlaka Strict yapilmalidir.
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest; //Always dersek sadece https üzerinden gelen istekleri karsilar. SameasRequest ise http gelirse http, https gelirse https gönderir.

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie = cookieBuilder;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Member/LogOut";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true; // belirli bir süre sonra Authorizationun yenilenmesi (30 gün icerisinde etkilesim olursa tekrar 30 gün uzatir)
               
            });



            services.AddSession(options =>
                        {
                            options.IdleTimeout = TimeSpan.FromMinutes(20);
                        });

            services.AddDependencyResolvers(new ICoreModule[] {
                new CoreModule()                                   //Core katmaninda kullandigimiz services modullerini ekliyoruz.
            });




            services.AddAuthentication().AddFacebook(faceoptions => {
                faceoptions.AppId = Configuration["Authentication-Facebook-AppId"];
                faceoptions.AppSecret = Configuration["Authentication-Facebook-AppSecret"];
                faceoptions.AccessDeniedPath = "/Account/AccessDenied";
            }).AddGoogle(googleoptions => {
                googleoptions.ClientId = Configuration["Authentication-Google-AppId"];
                googleoptions.ClientSecret = Configuration["Authentication-Google-AppSecret"];
                googleoptions.AccessDeniedPath = "/Account/AccessDenied";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               // app.UseStatusCodePages(); //Bos sayfa dönmesi yerine hatanin nerede oldugunu gösterir
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            //app.ConfigureCustomExceptionMiddleware(); //Kullaniciya dönen hata mesajlarinin düzgün gösterilmesi icin

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
