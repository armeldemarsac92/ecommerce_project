using System.Text.Json.Serialization;

namespace Tdev702.Contracts.OpenFoodFact.Response;

public class OpenFoodFactSearchResult
{
    [JsonPropertyName("count")]
    public int? Count { get; set; }

    [JsonPropertyName("page")]
    public int? Page { get; set; }

    [JsonPropertyName("page_count")]
    public int? PageCount { get; set; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }

    [JsonPropertyName("products")]
    public List<OpenFoodFactPartialProduct> Products { get; set; }
}



public class OpenFoodFactPartialProduct
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; }

    [JsonPropertyName("nutrition_grades")]
    public string NutritionGrades { get; set; }

    [JsonPropertyName("product_name_fr")]
    public string ProductNameFr { get; set; }
}
