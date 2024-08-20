using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTO
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
