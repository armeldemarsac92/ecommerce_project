namespace Tdev702.Api.Routes;

public static class ShopRoutes
{
    public const string Base = "/api/v1";

    public static class Products
    {
        public const string GetById = $"{Base}/products/{{Id}}";
        public const string GetAll = $"{Base}/products";
        public const string Create = $"{Base}/products";
        public const string Update = $"{Base}/products/{{Id}}";
        public const string Delete = $"{Base}/products/{{Id}}";
    }
    
    public static class ProductsTags
    {
        public const string GetById = $"{Base}/products_tags/{{productTagsId}}";
        public const string GetAll = $"{Base}/products_tags";
        public const string Create = $"{Base}/products_tags";
        public const string Update = $"{Base}/products_tags/{{productTagsId}}";
        public const string Delete = $"{Base}/products_tags/{{productTagsId}}";
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

    public static class TagsLinks
    {
        public const string GetById = $"{Base}/tags_links/{{tagId}}";
        public const string GetAll = $"{Base}/tags_links";
        public const string Create = $"{Base}/tags_links";
    }
}