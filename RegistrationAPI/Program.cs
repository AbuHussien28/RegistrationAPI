
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RegistrationAPI.API.Middlewares;
using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Core.Models;
using RegistrationAPI.Data;
using RegistrationAPI.Infrastructure.Repositorys;
using RegistrationAPI.Infrastructure.Services;
using RegistrationAPI.Infrastructure.UnitOfWorks;
using RegistrationAPI.Shared.DTOS;
using RegistrationAPI.Shared.Helpers;
using System.Text;

namespace RegistrationAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string txt = "";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<RegistrationContext>(options =>
        options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("Registration")));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<RegistrationContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddAuthentication(op => op.DefaultAuthenticateScheme = "RegistrationAPI")
               .AddJwtBearer("RegistrationAPI", option => {

                   var key = builder.Configuration["Jwt:Key"];
                   var secertkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

                   option.TokenValidationParameters = new TokenValidationParameters()
                   {
                       IssuerSigningKey = secertkey,
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };


               });
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddAutoMapper(typeof(MappingProfile));
         

            //   builder.Services.AddScoped<IUnitOfWork>();

            // 6. تسجيل AuthService و IAuthService
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            builder.Services.AddHttpContextAccessor();
            var app = builder.Build();
            //using (var scope = app.Services.CreateScope())
            //{
            //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //    await SeedRoles(roleManager);
            //}
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.MapOpenApi();
                app.UseSwaggerUI(option => option.SwaggerEndpoint("/openapi/v1.json", "v1"));

            //}
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.MapControllers();

            app.Run();
       
        }
        //static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        //{
        //    string[] roles = { "Admin", "User
        //    ", "Organizer" };

        //    foreach (var role in roles)
        //    {
        //        if (!await roleManager.RoleExistsAsync(role))
        //        {
        //            await roleManager.CreateAsync(new IdentityRole(role));
        //        }
        //    }
        //}
    }
}
