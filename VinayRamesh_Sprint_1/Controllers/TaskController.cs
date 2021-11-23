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
    public class TaskController : ControllerBase
    {
        List<Tasks> TasksList = new List<Tasks>
        {
            new Tasks{ID=2001, ProjectID=1, Status=2, AssignedToUserID=1001, Detail="Test Task 1", CreatedOn=DateTime.Now},
            new Tasks{ID=2002, ProjectID=2, Status=4, AssignedToUserID=1002, Detail="Test Task 2", CreatedOn=DateTime.Now},
            new Tasks{ID=2003, ProjectID=4, Status=3, AssignedToUserID=1001, Detail="Test Task 3", CreatedOn=DateTime.Now},
            new Tasks{ID=2004, ProjectID=2, Status=1, AssignedToUserID=1003, Detail="Test Task 4", CreatedOn=DateTime.Now},
        };

        [HttpPost]
        public ActionResult<Tasks> CreateTask(Tasks task)
        {
            if (task.ID == 0 || task.ProjectID == 0 || task.Status == 0 || task.AssignedToUserID == 0 || task.Detail == null)
            {
                return BadRequest();
            }

            var existingTask = TasksList.FirstOrDefault(t => t.ID == task.ID);
            if (existingTask != null)
            {
                return StatusCode(409, "TaskID already exists.");
            }
            TasksList.Add(task);
            return Ok();
        }

        [HttpPut]
        public ActionResult<Tasks> UpdateTask(Tasks task)
        {
            var updatedTask = TasksList.FirstOrDefault(t => t.ID == task.ID);
            if (updatedTask == null)
            {
                return NotFound();
            }
            updatedTask.ProjectID = task.ProjectID;
            updatedTask.Status = task.Status;
            updatedTask.AssignedToUserID = task.AssignedToUserID;
            updatedTask.Detail = task.Detail;

            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Tasks> GetTaskById(int id)
        {
            var requestedTask = TasksList.FirstOrDefault(t => t.ID == id);
            if (requestedTask == null)
            {
                return NoContent();
            }
            return requestedTask;
        }

        [HttpGet]
        public List<Tasks> GetAllTasks()
        {
            return TasksList;
        }
    }
}
