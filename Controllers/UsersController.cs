using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserApi.Models;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(long id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("searchByName")]
        public ActionResult<List<User>> SearchUsersByName([FromQuery] string name)
        {
            try
            {
                return Ok(_userService.SearchUsersByName(name));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("dateFormat")]
        public ActionResult<string> GetDateByLocale()
        {
            try
            {
                var lang = Request.Headers["Accept-Language"].ToString();
                return Ok(_userService.GetDateByLocale(lang));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("updateUser")]
        public ActionResult<string> UpdateUser([FromBody] User updatedUser)
        {
            try
            {
                return Ok(_userService.UpdateUser(updatedUser));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public class ImageUploadRequest
        {
            [Required]
            public required IFormFile Image { get; set; }
        }
        [HttpPost("uploadImage")]
        [Consumes("multipart/form-data")]
        public ActionResult<string> UploadImage([FromForm] ImageUploadRequest request)
        {
            try
            {
                return Ok(_userService.UploadImage(request.Image));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<string> DeleteUser(long id)
        {
            try
            {
                return Ok(_userService.DeleteUser(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}



