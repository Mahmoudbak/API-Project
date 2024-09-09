namespace ProjectApi.Model
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
    public class Order
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; }

        public Customer Customers { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
