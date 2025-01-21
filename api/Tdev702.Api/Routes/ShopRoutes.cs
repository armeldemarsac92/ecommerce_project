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
    
    public static class Inventories
    {
        public const string GetById = $"{Base}/inventories/{{id}}";
        public const string GetAll = $"{Base}/inventories";
        public const string GetInventoryByProductId = $"{Base}/inventories/{{productId}}";
        public const string Create = $"{Base}/inventories";
        public const string Update = $"{Base}/inventories/{{id}}";
        public const string Delete = $"{Base}/inventories/{{id}}";
    }
    
    public static class Invoices
    {
        public const string GetById = $"{Base}/invoices/{{Id}}";
        public const string GetAll = $"{Base}/invoices";
        public const string Create = $"{Base}/invoices";
        public const string Update = $"{Base}/invoices/{{Id}}";
        public const string Delete = $"{Base}/invoices/{{Id}}";
    }

    public static class Orders
    {
       
        public const string GetAll = $"{Base}/orders";
        public const string GetById = $"{Base}/orders/{{orderId}}";
        public const string Create = Base;
        public const string Update = $"{Base}orders/{{orderId}}";
    }

    public static class Payments
    {
        public const string Create = $"{Base}/payments";
    }

    public static class PaymentMethods
    {
        public const string Get = $"{Base}/payment_methods";
        public const string Attach = $"{Base}/payment_methods/{{paymentMethodId}}/attach";
        public const string Detach = $"{Base}/payment_methods/{{paymentMethodId}}/detach";
    }

    public static class Webhooks
    {
        public const string PaymentIntent = $"{Base}/webhooks/payment_intent";
    }
}