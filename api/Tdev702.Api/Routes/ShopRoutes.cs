namespace Tdev702.Api.Routes;

public static class ShopRoutes
{
    public const string Base = "/api/v1";

    public const string HealthCheck = $"{Base}/healthcheck";

    public static class Products
    {
        public const string GetById = $"{Base}/products/{{id}}";
        public const string GetAll = $"{Base}/products";
        public const string GetLiked = $"{Base}/products/liked";
        public const string Create = $"{Base}/products";
        public const string Update = $"{Base}/products/{{id}}";
        public const string Delete = $"{Base}/products/{{id}}";
        public const string Like = $"{Base}/products/{{id}}/like";
        public const string Unlike = $"{Base}/products/{{id}}/unlike";
    }

    public static class OpenFoodFactProducts
    {
        public const string GetAll = $"{Base}/open_food_fact";
        public const string GetByBarcode = $"{Base}/open_food_fact/{{barcode}}";
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
        public const string GetInventoryByProductId = $"{Base}/inventories/product/{{productId}}";
        public const string Create = $"{Base}/inventories";
        public const string Update = $"{Base}/inventories/{{id}}";
        public const string Delete = $"{Base}/inventories/{{id}}";
        public const string IncreamentStockInventory = $"{Base}/inventories/{{productId}}/increament";
        public const string SubstractStockInventory = $"{Base}/inventories/{{productId}}/substract";
    }

    public static class Customers
    {
        public const string GetById = $"{Base}/customers/{{id}}";
        public const string GetAll = $"{Base}/customers";
    }
    
    public static class Orders
    {
       
        public const string GetUserOrders = $"{Base}/orders/me";
        public const string CreatePayment = $"{Base}/orders/{{orderId}}/payment";
        public const string CreateSession = $"{Base}/orders/{{orderId}}/session";
        public const string GetAll = $"{Base}/orders";
        public const string GetById = $"{Base}/orders/{{orderId}}";
        public const string GetInvoice = $"{Base}/orders/{{orderId}}/invoice";
        public const string Create = $"{Base}/orders";
        public const string Update = $"{Base}/orders/{{orderId}}";
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
        public const string Session = $"{Base}/webhooks/session";
    }

    public static class Stats
    {
        public const string GetNewUsers = $"{Base}/stats/new_users/{{dateRange}}";
        public const string GetCartAverage = $"{Base}/stats/cart_average/{{dateRange}}";
        public const string GetRevenue = $"{Base}/stats/revenue/{{dateRange}}";
    }
}