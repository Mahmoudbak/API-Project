namespace ProjectApi.Model
{
    public class OrderCreateDTO
    {
        public OrderStatus Status { get; set; }
        public int? CustomerId { get; set; } 
        public List<OrderDetailDTO> OrderDetails { get; set; } 

    }

    public class OrderUpdateDTO
    {
        public OrderStatus Status { get; set; }
    }

}
