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
}