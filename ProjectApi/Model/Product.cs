using System.ComponentModel.DataAnnotations;
namespace ProjectApi.Model;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
    public decimal Price  { get; set; }
    [Range(0.01,double.MaxValue,ErrorMessage = "Quantity must be a non-negative number.")]
    public int QuantityInStock { get; set; }


}
