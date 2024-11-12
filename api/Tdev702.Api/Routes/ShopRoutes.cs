namespace Tdev702.Api.Routes;

public static class ShopRoutes
{
    public const string Base = "/api/v1";

    public static class Products
    {
        public const string GetById = $"{Base}/products/{{productId}}";
        public const string GetAll = $"{Base}/products";
        public const string Create = $"{Base}/products";
        public const string Update = $"{Base}/products/{{productId}}";
    }
    
    public static class ProductsTags
    {
        public const string GetById = $"{Base}/products_tags/{{productTagsId}}";
        public const string GetAll = $"{Base}/products_tags";
        public const string Create = $"{Base}/products_tags";
        public const string Update = $"{Base}/products_tags/{{productTagsId}}";
        public const string Delete = $"{Base}/products_tags/{{productTagsId}}";
    }

    public static class Brands
    {
        public const string GetById = $"{Base}/brand/{{brandId}}";
        public const string GetAll = $"{Base}/brands";
        public const string Create = $"{Base}/brands";
        public const string Update = $"{Base}/brands/{{brandId}}";
        public const string Delete = $"{Base}/brands/{{brandId}}";
    }
}