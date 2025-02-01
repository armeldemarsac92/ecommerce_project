namespace Tdev702.OpenFoodFact.SDK.Routes;

public static class OpenFoodFactRoutes
{
    private const string ApiBase = "/api/v2";

    public static class Search
    {
        private const string Base = $"{ApiBase}/search";
        public const string Product = Base;
    }

    public static class Product
    {
        private const string Base = "/api/v2/product/{barcode}&fields=image_url,code,nutrition_grades,product_name_fr,nutriments";
        public const string GetByBarcode = Base;
    }
}