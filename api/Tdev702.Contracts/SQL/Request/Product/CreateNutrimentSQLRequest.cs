using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Request.Product;

public class CreateNutrimentSQLRequest
{
   public required long ProductId { get; set; }
   public double? Carbohydrates { get; set; }

   public double? Carbohydrates100G { get; set; }

   public int? CarbohydratesPrepared { get; set; }

   public int? CarbohydratesPrepared100G { get; set; }

   public int? CarbohydratesPreparedServing { get; set; }

   public string CarbohydratesPreparedUnit { get; set; }

   public int? CarbohydratesPreparedValue { get; set; }

   public double? CarbohydratesServing { get; set; }

   public string CarbohydratesUnit { get; set; }

   public double? CarbohydratesValue { get; set; }

   public int? Energy { get; set; }

   public int? EnergyKcal { get; set; }

   public int? EnergyKcal100G { get; set; }

   public int? EnergyKcalPrepared { get; set; }

   public int? EnergyKcalPrepared100G { get; set; }

   public double? EnergyKcalPreparedServing { get; set; }

   public string EnergyKcalPreparedUnit { get; set; }

   public int? EnergyKcalPreparedValue { get; set; }

   public int? EnergyKcalServing { get; set; }

   public string EnergyKcalUnit { get; set; }

   public int? EnergyKcalValue { get; set; }

   public double? EnergyKcalValueComputed { get; set; }

   public int? EnergyKj { get; set; }

   public int? EnergyKj100G { get; set; }

   public int? EnergyKjPrepared { get; set; }

   public int? EnergyKjPrepared100G { get; set; }

   public int? EnergyKjPreparedServing { get; set; }

   public string EnergyKjPreparedUnit { get; set; }

   public int? EnergyKjPreparedValue { get; set; }

   public int? EnergyKjServing { get; set; }

   public string EnergyKjUnit { get; set; }

   public int? EnergyKjValue { get; set; }

   public double? EnergyKjValueComputed { get; set; }

   public int? Energy100G { get; set; }

   public int? EnergyPrepared { get; set; }

   public int? EnergyPrepared100G { get; set; }

   public int? EnergyPreparedServing { get; set; }

   public string EnergyPreparedUnit { get; set; }

   public int? EnergyPreparedValue { get; set; }

   public int? EnergyServing { get; set; }

   public string EnergyUnit { get; set; }

   public int? EnergyValue { get; set; }

   public double? Fat { get; set; }

   public double? Fat100G { get; set; }

   public double? FatPrepared { get; set; }

   public double? FatPrepared100G { get; set; }

   public double? FatPreparedServing { get; set; }

   public string FatPreparedUnit { get; set; }

   public double? FatPreparedValue { get; set; }

   public double? FatServing { get; set; }

   public string FatUnit { get; set; }

   public double? FatValue { get; set; }

   public double? FiberPrepared { get; set; }

   public double? FiberPrepared100G { get; set; }

   public double? FiberPreparedServing { get; set; }

   public string FiberPreparedUnit { get; set; }

   public double? FiberPreparedValue { get; set; }

   public double? FruitsVegetablesLegumesEstimateFromIngredients100G { get; set; }

   public double? FruitsVegetablesLegumesEstimateFromIngredientsServing { get; set; }

   public double? FruitsVegetablesNutsEstimateFromIngredients100G { get; set; }

   public double? FruitsVegetablesNutsEstimateFromIngredientsServing { get; set; }

   public int? NovaGroup { get; set; }

   public int? NovaGroup100G { get; set; }

   public int? NovaGroupServing { get; set; }

   public int? NutritionScoreFr { get; set; }

   public int? NutritionScoreFr100G { get; set; }

   public double? Proteins { get; set; }

   public double? Proteins100G { get; set; }

   public double? ProteinsPrepared { get; set; }

   public double? ProteinsPrepared100G { get; set; }

   public double? ProteinsPreparedServing { get; set; }

   public string ProteinsPreparedUnit { get; set; }

   public double? ProteinsPreparedValue { get; set; }

   public double? ProteinsServing { get; set; }

   public string ProteinsUnit { get; set; }

   public double? ProteinsValue { get; set; }

   public double? Salt { get; set; }
   
   public double? Salt100G { get; set; }

   public double? SaltPrepared { get; set; }

   public double? SaltPrepared100G { get; set; }

   public double? SaltPreparedServing { get; set; }

   public string SaltPreparedUnit { get; set; }

   public double? SaltPreparedValue { get; set; }

   public double? SaltServing { get; set; }

   public string SaltUnit { get; set; }

   public double? SaltValue { get; set; }

   public double? SaturatedFat { get; set; }

   public double? SaturatedFat100G { get; set; }

   public double? SaturatedFatPrepared { get; set; }

   public double? SaturatedFatPrepared100G { get; set; }

   public double? SaturatedFatPreparedServing { get; set; }

   public string SaturatedFatPreparedUnit { get; set; }

   public double? SaturatedFatPreparedValue { get; set; }

   public double? SaturatedFatServing { get; set; }

   public string SaturatedFatUnit { get; set; }

   public double? SaturatedFatValue { get; set; }

   public double? Sodium { get; set; }

   public double? Sodium100G { get; set; }

   public double? SodiumPrepared { get; set; }

   public double? SodiumPrepared100G { get; set; }

   public double? SodiumPreparedServing { get; set; }

   public string SodiumPreparedUnit { get; set; }

   public double? SodiumPreparedValue { get; set; }

   public double? SodiumServing { get; set; }

   public string SodiumUnit { get; set; }

   public double? SodiumValue { get; set; }

   public double? Sugars { get; set; }

   public double? Sugars100G { get; set; }

   public double? SugarsPrepared { get; set; }

   public double? SugarsPrepared100G { get; set; }

   public double? SugarsPreparedServing { get; set; }

   public string SugarsPreparedUnit { get; set; }

   public double? SugarsPreparedValue { get; set; }

   public double? SugarsServing { get; set; }

   public string SugarsUnit { get; set; }

   public double? SugarsValue { get; set; }
}