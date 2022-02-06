using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementBusiness;
using ProjectManagementBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksRepository _repository;
        public TasksController(ITasksRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = _repository.GetAllTasks();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var data = _repository.GetTaskById(id);
            if (data == null)
                return BadRequest("Task not found.");
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Post(Tasks task)
        {
            var checkTaskIdExists = _repository.GetTaskById(task.ID);
            if (checkTaskIdExists != null)
                return BadRequest("Task ID already exists.");
            var projectTaskExistForUser = _repository.GetTasksByUserProject(task.AssignedToUserID, task.ProjectID);
            if (projectTaskExistForUser != null)
                return BadRequest("Task already exists for user against the project.");
            var createTask = _repository.AddTask(task);
            if (createTask == null)
                return BadRequest("Something went wrong. Please try again!");
            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host
                + HttpContext.Request.Path + "/" + task.ID, task);
        }

        [HttpPut]
        public IActionResult Put(Tasks task)
        {
            var checkTaskIdExists = _repository.GetTaskById(task.ID);
            if (checkTaskIdExists == null)
                return BadRequest("Task ID not found.");

            var projectTaskExistForUser = _repository.GetTasksByUserProject(task.AssignedToUserID, task.ProjectID);
            if (projectTaskExistForUser != null && task.ID != projectTaskExistForUser.ID)
                return BadRequest("Task already exists for user against the project.");
            checkTaskIdExists.ProjectID = task.ProjectID;
            checkTaskIdExists.Status = task.Status;
            checkTaskIdExists.AssignedToUserID = task.AssignedToUserID;
            checkTaskIdExists.Detail = task.Detail;
            var updatedTask = _repository.UpdateTask(checkTaskIdExists);
            if (updatedTask == null)
                return BadRequest("Something went wrong. Please try again!");
            return Ok(updatedTask);
        }

        [HttpGet("byProjectId/{id}")]
        public IActionResult GetTaskByProjectId(int id)
        {
            var data = _repository.GetTaskByProjectId(id);
            if (data == null)
                return Ok("Task Not Found");
            return Ok("Task Found");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var deletedProject = _repository.DeleteTask(id);
            if (deletedProject == null)
                return BadRequest("Task doesn't exist!");
            return Ok(id);
        }
    }
}
