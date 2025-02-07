using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LoggingActionFilter))]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IObjectMapperService _mapperService;


        public UsersController(IUserService userService, ILogger<UsersController> logger, IObjectMapperService mapperService)
        {
            _userService = userService;
            _mapperService = mapperService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            _logger.LogInformation("Retrieving all users");
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(long id)
        {
            _logger.LogInformation("Retrieving user with ID: {Id}", id);
            var user = _userService.GetUserById(id);
            return Ok(user);
        }

        [HttpGet("searchByName")]
        public ActionResult<List<User>> SearchUsersByName([FromQuery] string name)
        {
            _logger.LogInformation("Searching users by name: {Name}", name);
            return Ok(_userService.SearchUsersByName(name));
        }

        [HttpGet("dateFormat")]
        public ActionResult<string> GetDateByLocale()
        {
            var lang = Request.Headers["Accept-Language"].ToString();
            _logger.LogInformation("Getting date format for language: {Language}", lang);
            return Ok(_userService.GetDateByLocale(lang));
        }

        [HttpPost("updateUser")]
        public ActionResult<string> UpdateUser([FromBody] User updatedUser)
        {
            _logger.LogInformation("Updating user with ID: {Id}", updatedUser.Id);
            return Ok(_userService.UpdateUser(updatedUser));
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
            _logger.LogInformation("Uploading image: {FileName}", request.Image.FileName);
            return Ok(_userService.UploadImage(request.Image));
        }

        [HttpDelete("{id}")]
        public ActionResult<string> DeleteUser(long id)
        {
            _logger.LogInformation("Deleting user with ID: {Id}", id);
            return Ok(_userService.DeleteUser(id));
        }

          [HttpGet("student/{id}")]
        public ActionResult<Student> GetStudentFromUser(long id)
        {
            var user = _userService.GetUserById(id);
            var Student = _mapperService.Map<User, Student>(user);
            return Ok(Student);
        }
    }
}




