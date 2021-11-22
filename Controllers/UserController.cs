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
    public class UserController : ControllerBase
    {
        List<Users> userList = new List<Users>
        {
            new Users{ID=1001, FirstName="Raj", LastName="R", Email="raj@gmail.com", Password="Raj123"},
            new Users{ID=1002, FirstName="Sammy", LastName="S", Email="sammy@gmail.com", Password="Sam123"},
            new Users{ID=1003, FirstName="Chammy", LastName="C", Email="chammy@gmail.com", Password="Chammy123"},
        };

        [HttpPost]
        public ActionResult<Users> CreateUser(Users user)
        {
            if (user.ID == 0 || user.FirstName == null || user.LastName == null || user.Email == null || user.Password == null)
            {
                return BadRequest();
            }
            var userExists = userList.FirstOrDefault(p => p.ID == user.ID);
            if (userExists != null)
            {
                return StatusCode(409, "UserID already exists");
            }
            userList.Add(user);
            return Ok();
        }

        [HttpGet]
        public List<Users> GetAllUsers()
        {
            return userList;
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult<Users> GetUserByID(int id)
        {
            var requestedUser = userList.FirstOrDefault(u => u.ID == id);
            if (requestedUser == null)
            {
                return NoContent();
            }
            return requestedUser;
        }

        [HttpPut]
        public ActionResult<Users> UpdateUser(Users user)
        {
            var updatedObj = userList.FirstOrDefault(u => u.ID == user.ID);

            if (updatedObj == null)
            {
                return NotFound();
            }
            updatedObj.FirstName = user.FirstName;
            updatedObj.LastName = user.LastName;
            updatedObj.Email = user.Email;
            updatedObj.Password = user.Password;

            return updatedObj;
        }

        [HttpPost]
        [Route("userLogin")]
        public ActionResult<Users> UserLogin(Users user)
        {
            var userDetails = userList.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
            if (userDetails == null)
            {
                return Unauthorized();
            }
            return Ok();
        }

    }
}
