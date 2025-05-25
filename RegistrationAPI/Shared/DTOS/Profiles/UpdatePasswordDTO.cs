using System.ComponentModel.DataAnnotations;

namespace RegistrationAPI.Shared.DTOS.Profiles
{
    public class UpdatePasswordDTO
    {
        [Required(ErrorMessage = "Old password is required.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Confirm password does not match the new password.")]
        public string ConfirmPassword { get; set; }
    }
}
