
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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
