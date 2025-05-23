using System.ComponentModel.DataAnnotations;

namespace RegistrationAPI.Shared.DTOS.Events
{
    public class EventCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = "StartDate must be a valid date/time")]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "EndDate is required")]
        [DataType(DataType.DateTime, ErrorMessage = "EndDate must be a valid date/time")]
        [CustomValidation(typeof(EventCreateDto), "ValidateEndDate")]
        public DateTime EndDate { get; set; }

        public static ValidationResult ValidateEndDate(DateTime endDate, ValidationContext context)
        {
            var instance = (EventCreateDto)context.ObjectInstance;
            if (endDate <= instance.StartDate)
            {
                return new ValidationResult("EndDate must be after StartDate");
            }
            return ValidationResult.Success;
        }
    }
}
