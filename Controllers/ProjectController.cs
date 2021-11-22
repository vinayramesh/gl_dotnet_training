using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GL_dotnet_training.Models;

namespace GL_dotnet_training.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        List<Projects> ProjectList = new List<Projects>
        {
            new Projects{ID=1,Name="Project 1", Detail ="Details for project 1",CreatedOn=DateTime.Now},
            new Projects{ID=2,Name="Project 2", Detail ="Details for project 2",CreatedOn=DateTime.Now},
            new Projects{ID=3,Name="Project 3", Detail ="Details for project 3",CreatedOn=DateTime.Now},
        };


        [HttpGet]
        public List<Projects> GetAllProjects()
        {
            return ProjectList;
        }

        [Route("{id}")]
        [HttpGet]
        public dynamic GetProjectByID(int id)
        {
            var requestedProject = ProjectList.FirstOrDefault(p => p.ID == id);
            if (requestedProject == null)
            {
                return NoContent();
            }
            return requestedProject;
        }

        [HttpPut]
        public ActionResult<Projects> UpdateProject(Projects project)
        {
            var updatedProject = ProjectList.FirstOrDefault(p => p.ID == project.ID);

            if (updatedProject == null)
            {
                return NotFound();
            }
            updatedProject.Name = project.Name;
            updatedProject.Detail = project.Detail;

            return updatedProject;
        }

        [HttpPost]
        public ActionResult<Projects> CreateProject(Projects project)
        {
            if (project.ID == 0 || project.Name == null || project.Detail == null)
            {
                return BadRequest();
            }
            var projectIdExists = ProjectList.FirstOrDefault(p => p.ID == project.ID);
            if (projectIdExists != null)
            {
                return StatusCode(409, "ProjectID already exists");
            }
            ProjectList.Add(project);
            return Ok();
        }
    }
}
