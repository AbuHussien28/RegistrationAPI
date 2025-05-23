namespace RegistrationAPI.Shared.DTOS.Events
{
    public class EventReadDto
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDeleted { get; set; }
        public string OrganizerName { get; set; }
    }
}
