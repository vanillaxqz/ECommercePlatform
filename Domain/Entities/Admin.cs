namespace Domain.Entities
{
    public class Admin
    {
        public Guid AdminId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
