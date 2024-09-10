using eShop.Service.DTOs;


namespace eShop.Service.Catalog.Products.DTOs.Manage
{
    public class GetProductPaggingDTOs : PaggingRequestBase
    {
        public string KeyWord { get; set; }
        public List<int> CategoryId { get; set; }
    }
}
