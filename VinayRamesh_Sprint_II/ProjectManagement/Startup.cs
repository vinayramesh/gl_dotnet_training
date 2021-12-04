using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProjectManagementRepository;
using ProjectManagementBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagementBusiness.Models;

namespace ProjectManagement
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
            services.AddDbContext<AppDBContext>(context => { context.UseInMemoryDatabase("ProjectManagement"); });
            services.AddScoped<IUserRepository, UserImplementation>();
            services.AddScoped<IProjectsRepository, ProjectsImplementation>();
            services.AddScoped<ITasksRepository, TasksImplementation>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectManagement", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectManagement v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDBContext>();
            InitialSeedData(context);
        }

        public static void InitialSeedData(AppDBContext context)
        {
            //Initial Data for Users
            context.Users.Add(new Users { ID = 1001, FirstName = "John", LastName = "Doe", Email = "john.doe@test.com", Password = "John123" });
            context.Users.Add(new Users { ID = 1002, FirstName = "John", LastName = "Skeet", Email = "john.skeet@test.com", Password = "Skeet123" });
            context.Users.Add(new Users { ID = 1003, FirstName = "Mark", LastName = "Seeman", Email = "mark.seeman@test.com", Password = "Mark123" });
            context.Users.Add(new Users { ID = 1004, FirstName = "Bob", LastName = "Martin", Email = "bob.martin@test.com", Password = "Bob123" });

            //Initial Data for Projects
            context.Projects.Add(new Projects { ID = 1, Name = "Project 1", Detail = "Details for project 1", CreatedOn = DateTime.Now });
            context.Projects.Add(new Projects { ID = 2, Name = "Project 2", Detail = "Details for project 2", CreatedOn = DateTime.Now });
            context.Projects.Add(new Projects { ID = 3, Name = "Project 3", Detail = "Details for project 3", CreatedOn = DateTime.Now });
            context.Projects.Add(new Projects { ID = 4, Name = "Project 4", Detail = "Details for project 4", CreatedOn = DateTime.Now });

            //Initial Data for Tasks
            context.Tasks.Add(new Tasks { ID = 1, ProjectID = 2, Status = 3, AssignedToUserID = 1003, Detail = "Task 1 details", CreatedOn = DateTime.Now });
            context.Tasks.Add(new Tasks { ID = 2, ProjectID = 1, Status = 1, AssignedToUserID = 1001, Detail = "Task 2 details", CreatedOn = DateTime.Now });
            context.Tasks.Add(new Tasks { ID = 3, ProjectID = 4, Status = 2, AssignedToUserID = 1003, Detail = "Task 3 details", CreatedOn = DateTime.Now });
            context.Tasks.Add(new Tasks { ID = 4, ProjectID = 1, Status = 4, AssignedToUserID = 1002, Detail = "Task 4 details", CreatedOn = DateTime.Now });

            context.SaveChanges();
        }
    }
}
