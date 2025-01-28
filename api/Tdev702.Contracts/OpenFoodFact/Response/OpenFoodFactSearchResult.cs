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
    public List<OpenFoodFactProduct> Products { get; set; }
}

public class NutriscoreData
    {
        [JsonPropertyName("energy")]
        public int? Energy { get; set; }

        [JsonPropertyName("energy_points")]
        public int? EnergyPoints { get; set; }

        [JsonPropertyName("energy_value")]
        public int? EnergyValue { get; set; }

        [JsonPropertyName("fiber")]
        public double? Fiber { get; set; }

        [JsonPropertyName("fiber_points")]
        public int? FiberPoints { get; set; }

        [JsonPropertyName("fiber_value")]
        public double? FiberValue { get; set; }

        [JsonPropertyName("fruits_vegetables_nuts_colza_walnut_olive_oils")]
        public double? FruitsVegetablesNutsColzaWalnutOliveOils { get; set; }

        [JsonPropertyName("fruits_vegetables_nuts_colza_walnut_olive_oils_points")]
        public int? FruitsVegetablesNutsColzaWalnutOliveOilsPoints { get; set; }

        [JsonPropertyName("fruits_vegetables_nuts_colza_walnut_olive_oils_value")]
        public double? FruitsVegetablesNutsColzaWalnutOliveOilsValue { get; set; }

        [JsonPropertyName("grade")]
        public string Grade { get; set; }

        [JsonPropertyName("is_beverage")]
        public int? IsBeverage { get; set; }

        [JsonPropertyName("is_cheese")]
        public int? IsCheese { get; set; }

        [JsonPropertyName("is_fat")]
        public int? IsFat { get; set; }

        [JsonPropertyName("is_water")]
        public int? IsWater { get; set; }

        [JsonPropertyName("negative_points")]
        public int? NegativePoints { get; set; }

        [JsonPropertyName("positive_points")]
        public int? PositivePoints { get; set; }

        [JsonPropertyName("proteins")]
        public string? Proteins { get; set; }

        [JsonPropertyName("proteins_points")]
        public string? ProteinsPoints { get; set; }

        [JsonPropertyName("proteins_value")]
        public string? ProteinsValue { get; set; }

        [JsonPropertyName("saturated_fat")]
        public string? SaturatedFat { get; set; }

        [JsonPropertyName("saturated_fat_points")]
        public string? SaturatedFatPoints { get; set; }

        [JsonPropertyName("saturated_fat_value")]
        public string? SaturatedFatValue { get; set; }

        [JsonPropertyName("score")]
        public string? Score { get; set; }

        [JsonPropertyName("sodium")]
        public string? Sodium { get; set; }

        [JsonPropertyName("sodium_points")]
        public string? SodiumPoints { get; set; }

        [JsonPropertyName("sodium_value")]
        public int? SodiumValue { get; set; }

        [JsonPropertyName("sugars")]
        public double? Sugars { get; set; }

        [JsonPropertyName("sugars_points")]
        public int? SugarsPoints { get; set; }

        [JsonPropertyName("sugars_value")]
        public double? SugarsValue { get; set; }
    }

    public class OpenFoodFactProduct
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("nutriscore_data")]
        public NutriscoreData NutriscoreData { get; set; }

        [JsonPropertyName("nutrition_grades")]
        public string NutritionGrades { get; set; }

        [JsonPropertyName("product_name_fr")]
        public string ProductNameFr { get; set; }
    }
