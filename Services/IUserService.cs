using UserApi.Models;

public interface IUserService
{
    List<User> GetAllUsers();
    User GetUserById(long id);
    List<User> SearchUsersByName(string name);
    string GetDateByLocale(string lang);
    string UpdateUser(User updatedUser);
    string UploadImage(IFormFile image);
    string DeleteUser(long id);
}
