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

    public static class Brands
    {
        public const string GetById = $"{Base}/brands/{{brandId}}";
        public const string GetAll = $"{Base}/brands";
        public const string Create = $"{Base}/brands";
        public const string Update = $"{Base}/brands/{{brandId}}";
        public const string Delete = $"{Base}/brands/{{brandId}}";
    }

    public static class Categories
    {
        public const string GetById = $"{Base}/categories/{{id}}";
        public const string GetAll = $"{Base}/categories";
        public const string Create = $"{Base}/categories";
        public const string Update = $"{Base}/categories/{{id}}";
        public const string Delete = $"{Base}/categories/{{id}}";
    }
}