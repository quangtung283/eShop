using eShop.Data.Entities;

namespace eShop.Service.Catalog.Products.DTOs.Manage
{
    public class UpdateProductDTOs
    {
        public int Id { get; set; }
        public int ProductId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }
        public string LanguageId { set; get; }

    }
}
