using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserApi.Exceptions;
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
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Retrieving all users");
                var users = _userService.GetAllUsers();
                if (users == null || users.Count == 0)
                {
                    return NotFound("No users found");
                }
                return Ok(users);
            }
            catch (UserApiException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving users");
                return StatusCode(500, new { message = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(long id)
        {
            try
            {
                _logger.LogInformation("Retrieving user with ID: {Id}", id);
                var user = _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }
                return Ok(user);
            }
            catch (UserApiException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving user by ID");
                return StatusCode(500, new { message = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpGet("searchByName")]
        public IActionResult SearchUsersByName([FromQuery] string name)
        {
            try
            {
                _logger.LogInformation("Searching users by name: {Name}", name);
                var users = _userService.SearchUsersByName(name);
                if (users == null || users.Count == 0)
                {
                    return NotFound($"No users found with the name '{name}'");
                }
                return Ok(users);
            }
            catch (UserApiException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while searching users by name");
                return StatusCode(500, new { message = "An unexpected error occurred", details = ex.Message });
            }
        }
        [HttpGet("get-date")]
        public ActionResult<string> GetDateByLocale()
        {
            var lang = Request.Headers["Accept-Language"].ToString();
            try
            {
                var date = _userService.GetDateByLocale(lang);
                return Ok(date);
            }
            catch (UserApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpPost("updateUser")]
        public IActionResult UpdateUser([FromBody] User updatedUser)
        {
            try
            {
                _logger.LogInformation("Updating user with ID: {Id}", updatedUser.Id);
                var result = _userService.UpdateUser(updatedUser);
                return Ok(result);
            }
            catch (UserApiException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the user");
                return StatusCode(500, new { message = "An unexpected error occurred", details = ex.Message });
            }
        }

        public class ImageUploadRequest
        {
            [Required]
            public IFormFile Image { get; set; }
        }

        [HttpPost("uploadImage")]
        [Consumes("multipart/form-data")]
        public IActionResult UploadImage([FromForm] ImageUploadRequest request)
        {
            try
            {
                _logger.LogInformation("Uploading image: {FileName}", request.Image.FileName);
                var result = _userService.UploadImage(request.Image);
                return Ok(result);
            }
            catch (UserApiException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while uploading the image");
                return StatusCode(500, new { message = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(long id)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID: {Id}", id);
                var result = _userService.DeleteUser(id);
                return Ok(result);
            }
            catch (UserApiException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the user");
                return StatusCode(500, new { message = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpGet("student/{id}")]
        public IActionResult GetStudentFromUser(long id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }
                var student = _mapperService.Map<User, Student>(user);
                return Ok(student);
            }
            catch (UserApiException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving student data");
                return StatusCode(500, new { message = "An unexpected error occurred", details = ex.Message });
            }
        }
    }
}




