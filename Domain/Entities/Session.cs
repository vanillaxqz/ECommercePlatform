namespace Domain.Entities
{
    public class Session
    {
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
    }
}
