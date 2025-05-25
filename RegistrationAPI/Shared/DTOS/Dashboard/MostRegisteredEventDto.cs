namespace RegistrationAPI.Shared.DTOS.Dashboard
{
    public class MostRegisteredEventDto
    {
        public string Name { get; set; }
        public int Registrations { get; set; }
        public DateTime StartDate { get; set; }
        public string Location { get; set; }
    }
}
