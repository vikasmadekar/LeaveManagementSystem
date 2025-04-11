
using FluentValidation;
using LeaveManagementSystem.Helper;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Repository.Implementation;
using LeaveManagementSystem.Repository;
using LeaveManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using System;
using FluentValidation.AspNetCore;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LeaveManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<LDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));
            builder.Services.AddScoped<IEmployeRepository, EmployeRepository>();
            builder.Services.AddScoped<IEmployeService, EmployeService>();

            var jwtSection = builder.Configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSection["Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            //builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //builder.Services.AddScoped<IValidator<EmployeDTO>, EmployeValidator>();
            //builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IValidator<EmployeDTO>, EmployeDTOValidator>();




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); 
            }
            app.UseAuthentication();

            app.UseHttpsRedirection();

     


            app.MapControllers();

            app.Run();
        }
    }
}
