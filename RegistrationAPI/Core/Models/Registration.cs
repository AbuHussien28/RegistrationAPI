namespace RegistrationAPI.Core.Models
{
    public class Registration
    {
        public int RegistrationId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
