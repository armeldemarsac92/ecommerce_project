using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response;

public class NutriscoreSQLResponse
{ 
   [Column("id")]
   public int Id { get; set; }
   
   [Column("nutri_score")]
   public string? Nutriscore { get; set; }

   [Column("energy")]
   public int? Energy { get; set; }

   [Column("energy_points")] 
   public int? EnergyPoints { get; set; }

   [Column("energy_value")]
   public double? EnergyValue { get; set; }

   [Column("fiber")]
   public double? Fiber { get; set; }

   [Column("fiber_points")]
   public int? FiberPoints { get; set; }

   [Column("fiber_value")]
   public double? FiberValue { get; set; }

   [Column("fruits_vegetables_nuts_colza_walnut_olive_oils")]
   public double? FruitsVegetablesNutsOils { get; set; }

   [Column("fruits_vegetables_nuts_colza_walnut_olive_oils_points")]
   public int? FruitsVegetablesNutsOilsPoints { get; set; }

   [Column("fruits_vegetables_nuts_colza_walnut_olive_oils_value")]
   public double? FruitsVegetablesNutsOilsValue { get; set; }

   [Column("grade")]
   public string? Grade { get; set; }

   [Column("is_beverage")]
   public bool? IsBeverage { get; set; }

   [Column("is_cheese")]
   public bool? IsCheese { get; set; }

   [Column("is_fat")]
   public bool? IsFat { get; set; }

   [Column("is_water")]
   public bool? IsWater { get; set; }

   [Column("negative_points")]
   public int? NegativePoints { get; set; }

   [Column("positive_points")]
   public int? PositivePoints { get; set; }

   [Column("proteins")]
   public double? Proteins { get; set; }

   [Column("proteins_points")]
   public int? ProteinsPoints { get; set; }

   [Column("proteins_value")]
   public double? ProteinsValue { get; set; }

   [Column("saturated_fat")]
   public double? SaturatedFat { get; set; }

   [Column("saturated_fat_points")]
   public int? SaturatedFatPoints { get; set; }

   [Column("saturated_fat_value")]
   public double? SaturatedFatValue { get; set; }

   [Column("score")]
   public int? Score { get; set; }

   [Column("sodium")]
   public int? Sodium { get; set; }

   [Column("sodium_points")]
   public int? SodiumPoints { get; set; }

   [Column("sodium_value")]
   public double? SodiumValue { get; set; }

   [Column("sugars")]
   public double? Sugars { get; set; }

   [Column("sugars_points")]
   public int? SugarsPoints { get; set; }

   [Column("sugars_value")]
   public double? SugarsValue { get; set; }
}