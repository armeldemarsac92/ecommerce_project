using System.Text.Json.Serialization;

namespace Tdev702.Contracts.OpenFoodFact.Response;

public class OpenFoodFactProductResult
{
    [JsonPropertyName("code")]
    public string Code { get; set; }
    
    [JsonPropertyName("product")]
    public OpenFoodFactProduct? Product { get; set; }
}
public class OpenFoodFactProduct
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; }

    [JsonPropertyName("nutriments")]
    public Nutriments Nutriments { get; set; }

    [JsonPropertyName("nutrition_grades")]
    public string NutritionGrades { get; set; }

    [JsonPropertyName("product_name_fr")]
    public string ProductNameFr { get; set; }
}

public class Nutriments
{
    [JsonPropertyName("carbohydrates")]
    public double? Carbohydrates { get; set; }

    [JsonPropertyName("carbohydrates_100g")]
    public double? Carbohydrates100G { get; set; }

    [JsonPropertyName("carbohydrates_prepared")]
    public int? CarbohydratesPrepared { get; set; }

    [JsonPropertyName("carbohydrates_prepared_100g")]
    public int? CarbohydratesPrepared100G { get; set; }

    [JsonPropertyName("carbohydrates_prepared_serving")]
    public int? CarbohydratesPreparedServing { get; set; }

    [JsonPropertyName("carbohydrates_prepared_unit")]
    public string CarbohydratesPreparedUnit { get; set; }

    [JsonPropertyName("carbohydrates_prepared_value")]
    public int? CarbohydratesPreparedValue { get; set; }

    [JsonPropertyName("carbohydrates_serving")]
    public double? CarbohydratesServing { get; set; }

    [JsonPropertyName("carbohydrates_unit")]
    public string CarbohydratesUnit { get; set; }

    [JsonPropertyName("carbohydrates_value")]
    public double? CarbohydratesValue { get; set; }

    [JsonPropertyName("energy")]
    public int? Energy { get; set; }

    [JsonPropertyName("energy-kcal")]
    public int? EnergyKcal { get; set; }

    [JsonPropertyName("energy-kcal_100g")]
    public int? EnergyKcal100G { get; set; }

    [JsonPropertyName("energy-kcal_prepared")]
    public int? EnergyKcalPrepared { get; set; }

    [JsonPropertyName("energy-kcal_prepared_100g")]
    public int? EnergyKcalPrepared100G { get; set; }

    [JsonPropertyName("energy-kcal_prepared_serving")]
    public double? EnergyKcalPreparedServing { get; set; }

    [JsonPropertyName("energy-kcal_prepared_unit")]
    public string EnergyKcalPreparedUnit { get; set; }

    [JsonPropertyName("energy-kcal_prepared_value")]
    public int? EnergyKcalPreparedValue { get; set; }

    [JsonPropertyName("energy-kcal_serving")]
    public int? EnergyKcalServing { get; set; }

    [JsonPropertyName("energy-kcal_unit")]
    public string EnergyKcalUnit { get; set; }

    [JsonPropertyName("energy-kcal_value")]
    public int? EnergyKcalValue { get; set; }

    [JsonPropertyName("energy-kcal_value_computed")]
    public double? EnergyKcalValueComputed { get; set; }

    [JsonPropertyName("energy-kj")]
    public int? EnergyKj { get; set; }

    [JsonPropertyName("energy-kj_100g")]
    public int? EnergyKj100G { get; set; }

    [JsonPropertyName("energy-kj_prepared")]
    public int? EnergyKjPrepared { get; set; }

    [JsonPropertyName("energy-kj_prepared_100g")]
    public int? EnergyKjPrepared100G { get; set; }

    [JsonPropertyName("energy-kj_prepared_serving")]
    public int? EnergyKjPreparedServing { get; set; }

    [JsonPropertyName("energy-kj_prepared_unit")]
    public string EnergyKjPreparedUnit { get; set; }

    [JsonPropertyName("energy-kj_prepared_value")]
    public int? EnergyKjPreparedValue { get; set; }

    [JsonPropertyName("energy-kj_serving")]
    public int? EnergyKjServing { get; set; }

    [JsonPropertyName("energy-kj_unit")]
    public string EnergyKjUnit { get; set; }

    [JsonPropertyName("energy-kj_value")]
    public int? EnergyKjValue { get; set; }

    [JsonPropertyName("energy-kj_value_computed")]
    public double? EnergyKjValueComputed { get; set; }

    [JsonPropertyName("energy_100g")]
    public int? Energy100G { get; set; }

    [JsonPropertyName("energy_prepared")]
    public int? EnergyPrepared { get; set; }

    [JsonPropertyName("energy_prepared_100g")]
    public int? EnergyPrepared100G { get; set; }

    [JsonPropertyName("energy_prepared_serving")]
    public int? EnergyPreparedServing { get; set; }

    [JsonPropertyName("energy_prepared_unit")]
    public string EnergyPreparedUnit { get; set; }

    [JsonPropertyName("energy_prepared_value")]
    public int? EnergyPreparedValue { get; set; }

    [JsonPropertyName("energy_serving")]
    public int? EnergyServing { get; set; }

    [JsonPropertyName("energy_unit")]
    public string EnergyUnit { get; set; }

    [JsonPropertyName("energy_value")]
    public int? EnergyValue { get; set; }

    [JsonPropertyName("fat")]
    public double? Fat { get; set; }

    [JsonPropertyName("fat_100g")]
    public double? Fat100G { get; set; }

    [JsonPropertyName("fat_prepared")]
    public double? FatPrepared { get; set; }

    [JsonPropertyName("fat_prepared_100g")]
    public double? FatPrepared100G { get; set; }

    [JsonPropertyName("fat_prepared_serving")]
    public double? FatPreparedServing { get; set; }

    [JsonPropertyName("fat_prepared_unit")]
    public string FatPreparedUnit { get; set; }

    [JsonPropertyName("fat_prepared_value")]
    public double? FatPreparedValue { get; set; }

    [JsonPropertyName("fat_serving")]
    public double? FatServing { get; set; }

    [JsonPropertyName("fat_unit")]
    public string FatUnit { get; set; }

    [JsonPropertyName("fat_value")]
    public double? FatValue { get; set; }

    [JsonPropertyName("fiber_prepared")]
    public double? FiberPrepared { get; set; }

    [JsonPropertyName("fiber_prepared_100g")]
    public double? FiberPrepared100G { get; set; }

    [JsonPropertyName("fiber_prepared_serving")]
    public double? FiberPreparedServing { get; set; }

    [JsonPropertyName("fiber_prepared_unit")]
    public string FiberPreparedUnit { get; set; }

    [JsonPropertyName("fiber_prepared_value")]
    public double? FiberPreparedValue { get; set; }

    [JsonPropertyName("fruits-vegetables-legumes-estimate-from-ingredients_100g")]
    public double? FruitsVegetablesLegumesEstimateFromIngredients100G { get; set; }

    [JsonPropertyName("fruits-vegetables-legumes-estimate-from-ingredients_serving")]
    public double? FruitsVegetablesLegumesEstimateFromIngredientsServing { get; set; }

    [JsonPropertyName("fruits-vegetables-nuts-estimate-from-ingredients_100g")]
    public double? FruitsVegetablesNutsEstimateFromIngredients100G { get; set; }

    [JsonPropertyName("fruits-vegetables-nuts-estimate-from-ingredients_serving")]
    public double? FruitsVegetablesNutsEstimateFromIngredientsServing { get; set; }

    [JsonPropertyName("nova-group")]
    public int? NovaGroup { get; set; }

    [JsonPropertyName("nova-group_100g")]
    public int? NovaGroup100G { get; set; }

    [JsonPropertyName("nova-group_serving")]
    public int? NovaGroupServing { get; set; }

    [JsonPropertyName("nutrition-score-fr")]
    public int? NutritionScoreFr { get; set; }

    [JsonPropertyName("nutrition-score-fr_100g")]
    public int? NutritionScoreFr100G { get; set; }

    [JsonPropertyName("proteins")]
    public double? Proteins { get; set; }

    [JsonPropertyName("proteins_100g")]
    public double? Proteins100G { get; set; }

    [JsonPropertyName("proteins_prepared")]
    public double? ProteinsPrepared { get; set; }

    [JsonPropertyName("proteins_prepared_100g")]
    public double? ProteinsPrepared100G { get; set; }

    [JsonPropertyName("proteins_prepared_serving")]
    public double? ProteinsPreparedServing { get; set; }

    [JsonPropertyName("proteins_prepared_unit")]
    public string ProteinsPreparedUnit { get; set; }

    [JsonPropertyName("proteins_prepared_value")]
    public double? ProteinsPreparedValue { get; set; }

    [JsonPropertyName("proteins_serving")]
    public double? ProteinsServing { get; set; }

    [JsonPropertyName("proteins_unit")]
    public string ProteinsUnit { get; set; }

    [JsonPropertyName("proteins_value")]
    public double? ProteinsValue { get; set; }

    [JsonPropertyName("salt")]
    public double? Salt { get; set; }

    [JsonPropertyName("salt_100g")]
    public double? Salt100G { get; set; }

    [JsonPropertyName("salt_prepared")]
    public double? SaltPrepared { get; set; }

    [JsonPropertyName("salt_prepared_100g")]
    public double? SaltPrepared100G { get; set; }

    [JsonPropertyName("salt_prepared_serving")]
    public double? SaltPreparedServing { get; set; }

    [JsonPropertyName("salt_prepared_unit")]
    public string SaltPreparedUnit { get; set; }

    [JsonPropertyName("salt_prepared_value")]
    public double? SaltPreparedValue { get; set; }

    [JsonPropertyName("salt_serving")]
    public double? SaltServing { get; set; }

    [JsonPropertyName("salt_unit")]
    public string SaltUnit { get; set; }

    [JsonPropertyName("salt_value")]
    public double? SaltValue { get; set; }

    [JsonPropertyName("saturated-fat")]
    public double? SaturatedFat { get; set; }

    [JsonPropertyName("saturated-fat_100g")]
    public double? SaturatedFat100G { get; set; }

    [JsonPropertyName("saturated-fat_prepared")]
    public double? SaturatedFatPrepared { get; set; }

    [JsonPropertyName("saturated-fat_prepared_100g")]
    public double? SaturatedFatPrepared100G { get; set; }

    [JsonPropertyName("saturated-fat_prepared_serving")]
    public double? SaturatedFatPreparedServing { get; set; }

    [JsonPropertyName("saturated-fat_prepared_unit")]
    public string SaturatedFatPreparedUnit { get; set; }

    [JsonPropertyName("saturated-fat_prepared_value")]
    public double? SaturatedFatPreparedValue { get; set; }

    [JsonPropertyName("saturated-fat_serving")]
    public double? SaturatedFatServing { get; set; }

    [JsonPropertyName("saturated-fat_unit")]
    public string SaturatedFatUnit { get; set; }

    [JsonPropertyName("saturated-fat_value")]
    public double? SaturatedFatValue { get; set; }

    [JsonPropertyName("sodium")]
    public double? Sodium { get; set; }

    [JsonPropertyName("sodium_100g")]
    public double? Sodium100G { get; set; }

    [JsonPropertyName("sodium_prepared")]
    public double? SodiumPrepared { get; set; }

    [JsonPropertyName("sodium_prepared_100g")]
    public double? SodiumPrepared100G { get; set; }

    [JsonPropertyName("sodium_prepared_serving")]
    public double? SodiumPreparedServing { get; set; }

    [JsonPropertyName("sodium_prepared_unit")]
    public string SodiumPreparedUnit { get; set; }

    [JsonPropertyName("sodium_prepared_value")]
    public double? SodiumPreparedValue { get; set; }

    [JsonPropertyName("sodium_serving")]
    public double? SodiumServing { get; set; }

    [JsonPropertyName("sodium_unit")]
    public string SodiumUnit { get; set; }

    [JsonPropertyName("sodium_value")]
    public double? SodiumValue { get; set; }

    [JsonPropertyName("sugars")]
    public double? Sugars { get; set; }

    [JsonPropertyName("sugars_100g")]
    public double? Sugars100G { get; set; }

    [JsonPropertyName("sugars_prepared")]
    public double? SugarsPrepared { get; set; }

    [JsonPropertyName("sugars_prepared_100g")]
    public double? SugarsPrepared100G { get; set; }

    [JsonPropertyName("sugars_prepared_serving")]
    public double? SugarsPreparedServing { get; set; }

    [JsonPropertyName("sugars_prepared_unit")]
    public string SugarsPreparedUnit { get; set; }

    [JsonPropertyName("sugars_prepared_value")]
    public double? SugarsPreparedValue { get; set; }

    [JsonPropertyName("sugars_serving")]
    public double? SugarsServing { get; set; }

    [JsonPropertyName("sugars_unit")]
    public string SugarsUnit { get; set; }

    [JsonPropertyName("sugars_value")]
    public double? SugarsValue { get; set; }
}