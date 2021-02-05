using Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;

namespace WebAppMongo.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            this._service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return Ok(_service.GetAllUsers() as List<User>);
        }

        [Route("/api/user/count/")]
        public ActionResult<long> Count()
        {
            return Ok(_service.CountThemAll());
        }

        [Route("/api/user/GetUserById/{id}")]
        public ActionResult<User> GetUserById(string id)
        {
            User user = _service.GetSingle(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [Route("/api/user/getbysearch/{input}")]
        public ActionResult<IEnumerable<User>> GetUserSearch(string input)
        {
            return Ok(_service.GetAllUsersSearch(input));
        }

        [HttpPost]
        [Route("/api/user/create/", Name = "Create")]
        public ActionResult<User> Create(User user)
        {

            if (user == null)
            {
                return BadRequest();
            }
            _service.AddUser(user);

            return Ok(user);
        }

        [HttpPatch]
        [Route("/api/user/update/{id}", Name = "Update")]
        public IActionResult Update(string id, [FromBody]JsonPatchDocument<User> patchDoc)
        {
            User founduser = _service.GetSingle(id);
            patchDoc.ApplyTo(founduser);
            _service.UpdateUser(founduser);

            return Ok();
        }



        [Route("/api/user/DeleteUser/{id}")]
        public IActionResult DeleteUser(string id)
        {
            User user = _service.GetSingle(id);

            if (user == null)
            {
                return NotFound();
            }

            _service.RemoveUser(user);

            return Ok();
        }
    }
}