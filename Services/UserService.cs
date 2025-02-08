using System.Globalization;
using UserApi.Models;

public class UserService : IUserService
{
    private static List<User> _users = new List<User>
    {
        new User { Id = 1, Name = "Tala", Email = "talasafa03@gmail.com" },
        new User { Id = 2, Name = "Mira", Email = "mirasafa@gmail.com" },
        new User { Id = 3, Name = "Marwa", Email = "marwasafa@gmail.com" },
        new User { Id = 4, Name = "Rima", Email = "rimabadran@gmail.com" },
        new User { Id = 5, Name = "Hassan", Email = "hassansafa@gmail.com" }
    };

    public List<User> GetAllUsers()
    {
        return _users;
    }

    public User GetUserById(long id)
    {
        if (id <= 0)
            return null;

        return _users.FirstOrDefault(u => u.Id == id);
    }

    public List<User> SearchUsersByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return _users.Where(u => u.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public string GetDateByLocale(string lang)
    {
        if (string.IsNullOrWhiteSpace(lang))
            return "Invalid language code";

        var primaryLang = lang.Split(',')[0].Split('-')[0].Trim();

        var availableCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                                           .Select(c => c.TwoLetterISOLanguageName.ToLowerInvariant())
                                           .Distinct()
                                           .ToList();

        if (!availableCultures.Contains(primaryLang.ToLowerInvariant()))
            return "Culture not supported";

        var culture = CultureInfo.GetCultureInfo(primaryLang);
        return DateTime.Now.ToString("D", culture);
    }

    public string UpdateUser(User updatedUser)
    {
        if (updatedUser == null || updatedUser.Id <= 0)
            return "Invalid user data";

        var user = _users.FirstOrDefault(u => u.Id == updatedUser.Id);
        if (user == null)
            return "User not found";

        if (user.Email != updatedUser.Email)
            return "Email doesn't match the ID, access denied";

        user.Name = updatedUser.Name;
        return "User updated successfully";
    }

    public string UploadImage(IFormFile image)
    {
        if (image == null || image.Length == 0)
            return "No image uploaded";

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(fileExtension))
            return "Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed";

        if (!image.ContentType.StartsWith("image/"))
            return "Uploaded file is not an image";

        try
        {
            var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(imagesPath);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(imagesPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return $"Image uploaded successfully. Saved as: {uniqueFileName}";
        }
        catch (IOException)
        {
            return "Error saving the image";
        }
    }

    public string DeleteUser(long id)
    {
        if (id <= 0)
            return "Invalid user ID";

        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return "User not found";

        _users.Remove(user);
        return "User deleted successfully";
    }
}
