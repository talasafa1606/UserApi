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
            throw new ArgumentException("Id must be greater than 0");

        return _users.FirstOrDefault(u => u.Id == id)
            ?? throw new KeyNotFoundException($"User with ID {id} not found.");
    }

    public List<User> SearchUsersByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Search name cannot be empty");

        return _users.Where(u => u.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public string GetDateByLocale(string lang)
    {
        if (string.IsNullOrWhiteSpace(lang))
            throw new ArgumentException("Language code cannot be empty");

        try
        {
            var primaryLang = lang.Split(',')[0].Split('-')[0].Trim();

            var availableCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                                               .Select(c => c.TwoLetterISOLanguageName.ToLowerInvariant())
                                               .Distinct()
                                               .ToList();

            if (!availableCultures.Contains(primaryLang.ToLowerInvariant()))
                throw new ArgumentException($"Culture '{primaryLang}' is not supported");

            var culture = CultureInfo.GetCultureInfo(primaryLang);
            return DateTime.Now.ToString("D", culture); 
        }
        catch (CultureNotFoundException)
        {
            throw new ArgumentException($"Invalid culture code: {lang}");
        }
    }

    public string UpdateUser(User updatedUser)
    {
        if (updatedUser == null)
            throw new ArgumentNullException(nameof(updatedUser));

        if (updatedUser.Id <= 0)
            throw new ArgumentException("Invalid user ID");

        var user = _users.FirstOrDefault(u => u.Id == updatedUser.Id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {updatedUser.Id} not found");

        if (user.Email != updatedUser.Email)
            throw new UnauthorizedAccessException("Email doesn't match the id, you have no access to change the name.");

        user.Name = updatedUser.Name;
        return "User updated successfully";
    }

    public string UploadImage(IFormFile image)
    {
        if (image == null || image.Length == 0)
            throw new ArgumentException("No image uploaded");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(fileExtension))
            throw new ArgumentException("Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed");

        if (!image.ContentType.StartsWith("image/"))
            throw new ArgumentException("Uploaded file is not an image");

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
        catch (IOException ex)
        {
            throw new InvalidOperationException("Error saving the image", ex);
        }
    }

    public string DeleteUser(long id)
    {
        if (id <= 0)
            throw new ArgumentException("Id must be greater than 0");

        var user = _users.FirstOrDefault(u => u.Id == id)
            ?? throw new KeyNotFoundException($"User with ID {id} not found");

        _users.Remove(user);
        return "User deleted successfully";
    }
}
