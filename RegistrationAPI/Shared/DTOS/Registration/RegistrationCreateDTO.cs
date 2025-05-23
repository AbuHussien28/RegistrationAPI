using System.ComponentModel.DataAnnotations;

namespace RegistrationAPI.Shared.DTOS.Registration
{
    public class RegistrationCreateDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "EventId is required.")]
        public int EventId { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
