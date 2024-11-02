namespace Application.DTOs
{
    public class PaymentDto
    {
        public Guid PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid UserId { get; set; }
    }
}
