namespace ProjectApi.Model
{
    public class ProductDTO
    {
        public class ProductCreateDto
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int QuantityInStock { get; set; }
        }

        public class ProductUpdateDto
        {
            public decimal Price { get; set; }
            public int QuantityInStock { get; set; }
        }
    }
}
