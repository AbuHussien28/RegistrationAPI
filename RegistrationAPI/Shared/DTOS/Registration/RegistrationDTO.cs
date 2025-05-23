namespace RegistrationAPI.Shared.DTOS.Registration
{
    public class RegistrationDTO
    {
        public int RegistrationId { get; set; }
        public int EventId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string EventName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
