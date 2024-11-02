namespace Domain.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public Status Status { get; set; }
        public Guid PaymentId { get; set; }
    }
}
