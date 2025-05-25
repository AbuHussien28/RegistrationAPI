using System.ComponentModel.DataAnnotations;

namespace RegistrationAPI.Shared.DTOS.Profiles
{
    public class UpdateUserNameDTO
    {
        [Required(ErrorMessage = "New username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        public string NewUserName { get; set; }
    }
}
