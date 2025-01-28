namespace Tdev702.OpenFoodFact.SDK.Routes;

public static class OpenFoodFactRoutes
{
    private const string ApiBase = "/api/v2";

    public static class Search
    {
        private const string Base = $"{ApiBase}/search";
        public const string Product = Base;
    }
}