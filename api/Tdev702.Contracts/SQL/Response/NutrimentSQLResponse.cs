using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response;

public class NutrimentSQLResponse
{
   [Column("id")]
   public int Id { get; set; }
   
   [Column("product_id")]
   public long ProductId { get; set; }

   [Column("carbohydrates")]
   public double? Carbohydrates { get; set; }

   [Column("carbohydrates_100g")]
   public double? Carbohydrates100G { get; set; }

   [Column("carbohydrates_prepared")]
   public int? CarbohydratesPrepared { get; set; }

   [Column("carbohydrates_prepared_100g")]
   public int? CarbohydratesPrepared100G { get; set; }

   [Column("carbohydrates_prepared_serving")]
   public int? CarbohydratesPreparedServing { get; set; }

   [Column("carbohydrates_prepared_unit")]
   public string CarbohydratesPreparedUnit { get; set; }

   [Column("carbohydrates_prepared_value")]
   public int? CarbohydratesPreparedValue { get; set; }

   [Column("carbohydrates_serving")]
   public double? CarbohydratesServing { get; set; }

   [Column("carbohydrates_unit")]
   public string CarbohydratesUnit { get; set; }

   [Column("carbohydrates_value")]
   public double? CarbohydratesValue { get; set; }

   [Column("energy")]
   public int? Energy { get; set; }

   [Column("energy_kcal")]
   public int? EnergyKcal { get; set; }

   [Column("energy_kcal_100g")]
   public int? EnergyKcal100G { get; set; }

   [Column("energy_kcal_prepared")]
   public int? EnergyKcalPrepared { get; set; }

   [Column("energy_kcal_prepared_100g")]
   public int? EnergyKcalPrepared100G { get; set; }

   [Column("energy_kcal_prepared_serving")]
   public double? EnergyKcalPreparedServing { get; set; }

   [Column("energy_kcal_prepared_unit")]
   public string EnergyKcalPreparedUnit { get; set; }

   [Column("energy_kcal_prepared_value")]
   public int? EnergyKcalPreparedValue { get; set; }

   [Column("energy_kcal_serving")]
   public int? EnergyKcalServing { get; set; }

   [Column("energy_kcal_unit")]
   public string EnergyKcalUnit { get; set; }

   [Column("energy_kcal_value")]
   public int? EnergyKcalValue { get; set; }

   [Column("energy_kcal_value_computed")]
   public double? EnergyKcalValueComputed { get; set; }

   [Column("energy_kj")]
   public int? EnergyKj { get; set; }

   [Column("energy_kj_100g")]
   public int? EnergyKj100G { get; set; }

   [Column("energy_kj_prepared")]
   public int? EnergyKjPrepared { get; set; }

   [Column("energy_kj_prepared_100g")]
   public int? EnergyKjPrepared100G { get; set; }

   [Column("energy_kj_prepared_serving")]
   public int? EnergyKjPreparedServing { get; set; }

   [Column("energy_kj_prepared_unit")]
   public string EnergyKjPreparedUnit { get; set; }

   [Column("energy_kj_prepared_value")]
   public int? EnergyKjPreparedValue { get; set; }

   [Column("energy_kj_serving")]
   public int? EnergyKjServing { get; set; }

   [Column("energy_kj_unit")]
   public string EnergyKjUnit { get; set; }

   [Column("energy_kj_value")]
   public int? EnergyKjValue { get; set; }

   [Column("energy_kj_value_computed")]
   public double? EnergyKjValueComputed { get; set; }

   [Column("energy_100g")]
   public int? Energy100G { get; set; }

   [Column("energy_prepared")]
   public int? EnergyPrepared { get; set; }

   [Column("energy_prepared_100g")]
   public int? EnergyPrepared100G { get; set; }

   [Column("energy_prepared_serving")]
   public int? EnergyPreparedServing { get; set; }

   [Column("energy_prepared_unit")]
   public string EnergyPreparedUnit { get; set; }

   [Column("energy_prepared_value")]
   public int? EnergyPreparedValue { get; set; }

   [Column("energy_serving")]
   public int? EnergyServing { get; set; }

   [Column("energy_unit")]
   public string EnergyUnit { get; set; }

   [Column("energy_value")]
   public int? EnergyValue { get; set; }

   [Column("fat")]
   public double? Fat { get; set; }

   [Column("fat_100g")]
   public double? Fat100G { get; set; }

   [Column("fat_prepared")]
   public double? FatPrepared { get; set; }

   [Column("fat_prepared_100g")]
   public double? FatPrepared100G { get; set; }

   [Column("fat_prepared_serving")]
   public double? FatPreparedServing { get; set; }

   [Column("fat_prepared_unit")]
   public string FatPreparedUnit { get; set; }

   [Column("fat_prepared_value")]
   public double? FatPreparedValue { get; set; }

   [Column("fat_serving")]
   public double? FatServing { get; set; }

   [Column("fat_unit")]
   public string FatUnit { get; set; }

   [Column("fat_value")]
   public double? FatValue { get; set; }

   [Column("fiber_prepared")]
   public double? FiberPrepared { get; set; }

   [Column("fiber_prepared_100g")]
   public double? FiberPrepared100G { get; set; }

   [Column("fiber_prepared_serving")]
   public double? FiberPreparedServing { get; set; }

   [Column("fiber_prepared_unit")]
   public string FiberPreparedUnit { get; set; }

   [Column("fiber_prepared_value")]
   public double? FiberPreparedValue { get; set; }

   [Column("fruits_vegetables_legumes_estimate_from_ingredients_100g")]
   public double? FruitsVegetablesLegumesEstimateFromIngredients100G { get; set; }

   [Column("fruits_vegetables_legumes_estimate_from_ingredients_serving")]
   public double? FruitsVegetablesLegumesEstimateFromIngredientsServing { get; set; }

   [Column("fruits_vegetables_nuts_estimate_from_ingredients_100g")]
   public double? FruitsVegetablesNutsEstimateFromIngredients100G { get; set; }

   [Column("fruits_vegetables_nuts_estimate_from_ingredients_serving")]
   public double? FruitsVegetablesNutsEstimateFromIngredientsServing { get; set; }

   [Column("nova_group")]
   public int? NovaGroup { get; set; }

   [Column("nova_group_100g")]
   public int? NovaGroup100G { get; set; }

   [Column("nova_group_serving")]
   public int? NovaGroupServing { get; set; }

   [Column("nutrition_score_fr")]
   public int? NutritionScoreFr { get; set; }

   [Column("nutrition_score_fr_100g")]
   public int? NutritionScoreFr100G { get; set; }

   [Column("proteins")]
   public double? Proteins { get; set; }

   [Column("proteins_100g")]
   public double? Proteins100G { get; set; }

   [Column("proteins_prepared")]
   public double? ProteinsPrepared { get; set; }

   [Column("proteins_prepared_100g")]
   public double? ProteinsPrepared100G { get; set; }

   [Column("proteins_prepared_serving")]
   public double? ProteinsPreparedServing { get; set; }

   [Column("proteins_prepared_unit")]
   public string ProteinsPreparedUnit { get; set; }

   [Column("proteins_prepared_value")]
   public double? ProteinsPreparedValue { get; set; }

   [Column("proteins_serving")]
   public double? ProteinsServing { get; set; }

   [Column("proteins_unit")]
   public string ProteinsUnit { get; set; }

   [Column("proteins_value")]
   public double? ProteinsValue { get; set; }

   [Column("salt")]
   public double? Salt { get; set; }

   [Column("salt_100g")]
   public double? Salt100G { get; set; }

   [Column("salt_prepared")]
   public double? SaltPrepared { get; set; }

   [Column("salt_prepared_100g")]
   public double? SaltPrepared100G { get; set; }

   [Column("salt_prepared_serving")]
   public double? SaltPreparedServing { get; set; }

   [Column("salt_prepared_unit")]
   public string SaltPreparedUnit { get; set; }

   [Column("salt_prepared_value")]
   public double? SaltPreparedValue { get; set; }

   [Column("salt_serving")]
   public double? SaltServing { get; set; }

   [Column("salt_unit")]
   public string SaltUnit { get; set; }

   [Column("salt_value")]
   public double? SaltValue { get; set; }

   [Column("saturated_fat")]
   public double? SaturatedFat { get; set; }

   [Column("saturated_fat_100g")]
   public double? SaturatedFat100G { get; set; }

   [Column("saturated_fat_prepared")]
   public double? SaturatedFatPrepared { get; set; }

   [Column("saturated_fat_prepared_100g")]
   public double? SaturatedFatPrepared100G { get; set; }

   [Column("saturated_fat_prepared_serving")]
   public double? SaturatedFatPreparedServing { get; set; }

   [Column("saturated_fat_prepared_unit")]
   public string SaturatedFatPreparedUnit { get; set; }

   [Column("saturated_fat_prepared_value")]
   public double? SaturatedFatPreparedValue { get; set; }

   [Column("saturated_fat_serving")]
   public double? SaturatedFatServing { get; set; }

   [Column("saturated_fat_unit")]
   public string SaturatedFatUnit { get; set; }

   [Column("saturated_fat_value")]
   public double? SaturatedFatValue { get; set; }

   [Column("sodium")]
   public double? Sodium { get; set; }

   [Column("sodium_100g")]
   public double? Sodium100G { get; set; }

   [Column("sodium_prepared")]
   public double? SodiumPrepared { get; set; }

   [Column("sodium_prepared_100g")]
   public double? SodiumPrepared100G { get; set; }

   [Column("sodium_prepared_serving")]
   public double? SodiumPreparedServing { get; set; }

   [Column("sodium_prepared_unit")]
   public string SodiumPreparedUnit { get; set; }

   [Column("sodium_prepared_value")]
   public double? SodiumPreparedValue { get; set; }

   [Column("sodium_serving")]
   public double? SodiumServing { get; set; }

   [Column("sodium_unit")]
   public string SodiumUnit { get; set; }

   [Column("sodium_value")]
   public double? SodiumValue { get; set; }

   [Column("sugars")]
   public double? Sugars { get; set; }

   [Column("sugars_100g")]
   public double? Sugars100G { get; set; }

   [Column("sugars_prepared")]
   public double? SugarsPrepared { get; set; }

   [Column("sugars_prepared_100g")]
   public double? SugarsPrepared100G { get; set; }

   [Column("sugars_prepared_serving")]
   public double? SugarsPreparedServing { get; set; }

   [Column("sugars_prepared_unit")]
   public string SugarsPreparedUnit { get; set; }

   [Column("sugars_prepared_value")]
   public double? SugarsPreparedValue { get; set; }

   [Column("sugars_serving")]
   public double? SugarsServing { get; set; }

   [Column("sugars_unit")]
   public string SugarsUnit { get; set; }

   [Column("sugars_value")]
   public double? SugarsValue { get; set; }
}