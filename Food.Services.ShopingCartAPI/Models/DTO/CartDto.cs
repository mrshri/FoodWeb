namespace Food.Services.ShoppingCartAPI.Models.DTO
{
    public class CartDto
    {
        public CartHeader? CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetails  { get; set; }
    }
}
