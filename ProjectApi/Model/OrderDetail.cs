using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApi.Model
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
       
        
            public int Quantity { get; set; }
            public decimal PriceAtPurchase { get; set; }

            public int OrderId { get; set; }
            public int ProductId { get; set; }

            public virtual Order Order { get; set; }
            public virtual Product Product { get; set; }
        
    }
}
