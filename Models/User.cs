using System.ComponentModel.DataAnnotations;

namespace UserApi.Models
{
    public class User
    {
        [Required(ErrorMessage = "Id is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be a positive number.")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(30, ErrorMessage = "Name can't be longer than 30 characters.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }
    }
}
