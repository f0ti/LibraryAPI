using LibraryAPI.Data;
using LibraryAPI.IServices;
using LibraryAPI.Model;
using LibraryAPI.Services;
using LibraryAPI.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // SQL Database configurations
            services.AddDbContext<LibraryContext>(options => options.EnableSensitiveDataLogging(true).UseSqlServer(Configuration.GetConnectionString("LibraryAPI")));

            // Swagger configurations
            services.AddSwaggerGen();

            // Email configurations
            services.Configure<SMTPSettings>(Configuration.GetSection("SMTPSettings"));

            services.AddControllers();

            // Identity configurations
            services.AddIdentity<User, IdentityRole>(options =>
                    {
                        options.User.RequireUniqueEmail = true;
                        //options.SignIn.RequireConfirmedAccount = true
                        //options.Password.RequiredLength = 3;
                        //options.Password.RequireDigit = false;
                        //options.Password.RequireNonAlphanumeric = false;
                        //options.Password.RequireLowercase = false;
                        //options.Password.RequireUppercase = false;
                    })
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddDefaultTokenProviders()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<LibraryContext>();

            // Injecting services
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IAdminServices, AdminServices>();
            services.AddScoped<IBookServices, BookServices>();
            services.AddScoped<IBuyServices, BuyServices>();
            services.AddScoped<IQRServices, QRServices>();
            services.AddScoped<IPostmanServices, PostmanServices>();
            services.AddTransient<IEmailServices, EmailServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            RolesData.SeedRoles(roleManager).Wait();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
