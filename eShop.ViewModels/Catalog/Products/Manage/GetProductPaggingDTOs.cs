using eShop.ViewModels.Common.DTOs;

namespace eShop.ViewModels.Catalog.Products.Manage
{
    public class GetProductPaggingDTOs : PaggingRequestBase
    {
        public string KeyWord { get; set; }
        public List<int> CategoryId { get; set; }
    }
}
