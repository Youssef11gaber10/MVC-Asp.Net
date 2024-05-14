using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Demo.PL.Profiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Startup
    {
        public Startup(IConfiguration configuration) //use dependency injection=> inject with type of obj will be created
        {
            Configuration = configuration;
            //use dependency injection=>equal the created obj with ref
        }




        public IConfiguration Configuration { get; }//use dependency injection => ref for use the created obj
                                                    //this refer to appsettings.json => configration file 
                                                    //so can access data of inside appsetting with this ref



        #region Use this method to add services to the container


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<MvcAppDbContext>(Options =>
            { Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); }, ServiceLifetime.Scoped);//to allow dependency injection allow service of dbContext in container
            //i make clr when asked in constructor for object from class mvcappdbcontext create this obj
            //and make him to send options to paramterless ctor to send it to  base onConfiguring()
            //not prefer to put connection string like that cause if the app go to another env like test/stagging/production
            //server name will change so must put it in appsettings.json and import it to be dynamic



            //not need both repo not use them use unit of work instead of them
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();//allow dependancy injection for nomal work not db
                                                                              //when someone ask for obj from IdepartmentRepo(interface), create obj from concrete class DepartmentRepo


            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();//allow dependency injection for entity Employee Repo



            //services.AddSingleton<IEmployeeRepository, EmployeeRepository>();//application
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();//request
            //services.AddTransient<IEmployeeRepository, EmployeeRepository>();//operation

            //services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            //services.AddAutoMapper(M => M.AddProfile(new UserProfile()));
            services.AddAutoMapper(M => M.AddProfiles(new List<Profile>()
            { new EmployeeProfile(), new UserProfile(),new RoleProfile() }));

            services.AddScoped<IUnitOfWork, UnitOfWork>();




            //services.AddScoped<UserManager<ApplicationUser>>();//when ask obj from this class create it
            //if you wanna func add 3 service for (user Manger, RoleManger, SignInManager )
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)//its like funct add service to dbContext
                .AddCookie(Options =>//this cookie will store at cookies
                //but we will use jwt in api
                {
                    Options.LoginPath = ("Account/Login");
                    Options.AccessDeniedPath = "Home/Error";
                });
            //add services (repos) that create async,and other func  use them inside it
            services.AddIdentity<ApplicationUser, IdentityRole>(//here i add interface ,need to add classes
                Options =>
                {
                    Options.Password.RequireNonAlphanumeric = true;// ! @ #
                    Options.Password.RequireDigit = true;//1232
                    Options.Password.RequireLowercase = true;//aa
                    Options.Password.RequireUppercase = true;//Aag
                })
              .AddEntityFrameworkStores<MvcAppDbContext>()//these classes (repos) handel with database  need to determine your dbContext
              .AddDefaultTokenProviders();//must define schema at addAuthentication function()

        }

        #endregion


        #region Use this method to configure the HTTP request pipeline.


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //ordered
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();//first
            app.UseAuthentication();//second
            app.UseAuthorization();//second

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }


        #endregion

    }



}
