using Tdev702.Contracts.OpenFoodFact.Response;
using Tdev702.Contracts.SQL.Request.Product;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Contracts.Mapping;

public static class NutrimentsMapper
{
   public static UpdateNutrimentSQLRequest? MapNutriments(this FullProductSQLResponse product, OpenFoodFactProduct openFoodFactPartialProduct)
   {
       if (openFoodFactPartialProduct?.Nutriments == null)
           return null;

       var nutriments = openFoodFactPartialProduct.Nutriments;

       var nutrimentsRequest = new UpdateNutrimentSQLRequest
       {
           ProductId = product.Id,
           Carbohydrates = product.Carbohydrates == nutriments.Carbohydrates ? null : product.Carbohydrates,
           Carbohydrates100G = product.Carbohydrates100G == nutriments.Carbohydrates100G
               ? null
               : product.Carbohydrates100G,
           CarbohydratesPrepared = product.CarbohydratesPrepared == nutriments.CarbohydratesPrepared
               ? null
               : product.CarbohydratesPrepared,
           CarbohydratesPrepared100G = product.CarbohydratesPrepared100G == nutriments.CarbohydratesPrepared100G
               ? null
               : product.CarbohydratesPrepared100G,
           CarbohydratesPreparedServing =
               product.CarbohydratesPreparedServing == nutriments.CarbohydratesPreparedServing
                   ? null
                   : product.CarbohydratesPreparedServing,
           CarbohydratesPreparedUnit = product.CarbohydratesPreparedUnit == nutriments.CarbohydratesPreparedUnit
               ? null
               : product.CarbohydratesPreparedUnit,
           CarbohydratesPreparedValue = product.CarbohydratesPreparedValue == nutriments.CarbohydratesPreparedValue
               ? null
               : product.CarbohydratesPreparedValue,
           CarbohydratesServing = product.CarbohydratesServing == nutriments.CarbohydratesServing
               ? null
               : product.CarbohydratesServing,
           CarbohydratesUnit = product.CarbohydratesUnit == nutriments.CarbohydratesUnit
               ? null
               : product.CarbohydratesUnit,
           CarbohydratesValue = product.CarbohydratesValue == nutriments.CarbohydratesValue
               ? null
               : product.CarbohydratesValue,
           Energy = product.Energy == nutriments.Energy ? null : product.Energy,
           EnergyKcal = product.EnergyKcal == nutriments.EnergyKcal ? null : product.EnergyKcal,
           EnergyKcal100G = product.EnergyKcal100G == nutriments.EnergyKcal100G ? null : product.EnergyKcal100G,
           EnergyKcalPrepared = product.EnergyKcalPrepared == nutriments.EnergyKcalPrepared
               ? null
               : product.EnergyKcalPrepared,
           EnergyKcalPrepared100G = product.EnergyKcalPrepared100G == nutriments.EnergyKcalPrepared100G
               ? null
               : product.EnergyKcalPrepared100G,
           EnergyKcalPreparedServing = product.EnergyKcalPreparedServing == nutriments.EnergyKcalPreparedServing
               ? null
               : product.EnergyKcalPreparedServing,
           EnergyKcalPreparedUnit = product.EnergyKcalPreparedUnit == nutriments.EnergyKcalPreparedUnit
               ? null
               : product.EnergyKcalPreparedUnit,
           EnergyKcalPreparedValue = product.EnergyKcalPreparedValue == nutriments.EnergyKcalPreparedValue
               ? null
               : product.EnergyKcalPreparedValue,
           EnergyKcalServing = product.EnergyKcalServing == nutriments.EnergyKcalServing
               ? null
               : product.EnergyKcalServing,
           EnergyKcalUnit = product.EnergyKcalUnit == nutriments.EnergyKcalUnit ? null : product.EnergyKcalUnit,
           EnergyKcalValue = product.EnergyKcalValue == nutriments.EnergyKcalValue ? null : product.EnergyKcalValue,
           EnergyKcalValueComputed = product.EnergyKcalValueComputed == nutriments.EnergyKcalValueComputed
               ? null
               : product.EnergyKcalValueComputed,
           EnergyKj = product.EnergyKj == nutriments.EnergyKj ? null : product.EnergyKj,
           EnergyKj100G = product.EnergyKj100G == nutriments.EnergyKj100G ? null : product.EnergyKj100G,
           EnergyKjPrepared = product.EnergyKjPrepared == nutriments.EnergyKjPrepared ? null : product.EnergyKjPrepared,
           EnergyKjPrepared100G = product.EnergyKjPrepared100G == nutriments.EnergyKjPrepared100G
               ? null
               : product.EnergyKjPrepared100G,
           EnergyKjPreparedServing = product.EnergyKjPreparedServing == nutriments.EnergyKjPreparedServing
               ? null
               : product.EnergyKjPreparedServing,
           EnergyKjPreparedUnit = product.EnergyKjPreparedUnit == nutriments.EnergyKjPreparedUnit
               ? null
               : product.EnergyKjPreparedUnit,
           EnergyKjPreparedValue = product.EnergyKjPreparedValue == nutriments.EnergyKjPreparedValue
               ? null
               : product.EnergyKjPreparedValue,
           EnergyKjServing = product.EnergyKjServing == nutriments.EnergyKjServing ? null : product.EnergyKjServing,
           EnergyKjUnit = product.EnergyKjUnit == nutriments.EnergyKjUnit ? null : product.EnergyKjUnit,
           EnergyKjValue = product.EnergyKjValue == nutriments.EnergyKjValue ? null : product.EnergyKjValue,
           EnergyKjValueComputed = product.EnergyKjValueComputed == nutriments.EnergyKjValueComputed
               ? null
               : product.EnergyKjValueComputed,
           Energy100G = product.Energy100G == nutriments.Energy100G ? null : product.Energy100G,
           EnergyPrepared = product.EnergyPrepared == nutriments.EnergyPrepared ? null : product.EnergyPrepared,
           EnergyPrepared100G = product.EnergyPrepared100G == nutriments.EnergyPrepared100G
               ? null
               : product.EnergyPrepared100G,
           EnergyPreparedServing = product.EnergyPreparedServing == nutriments.EnergyPreparedServing
               ? null
               : product.EnergyPreparedServing,
           EnergyPreparedUnit = product.EnergyPreparedUnit == nutriments.EnergyPreparedUnit
               ? null
               : product.EnergyPreparedUnit,
           EnergyPreparedValue = product.EnergyPreparedValue == nutriments.EnergyPreparedValue
               ? null
               : product.EnergyPreparedValue,
           EnergyServing = product.EnergyServing == nutriments.EnergyServing ? null : product.EnergyServing,
           EnergyUnit = product.EnergyUnit == nutriments.EnergyUnit ? null : product.EnergyUnit,
           EnergyValue = product.EnergyValue == nutriments.EnergyValue ? null : product.EnergyValue,
           Fat = product.Fat == nutriments.Fat ? null : product.Fat,
           Fat100G = product.Fat100G == nutriments.Fat100G ? null : product.Fat100G,
           FatPrepared = product.FatPrepared == nutriments.FatPrepared ? null : product.FatPrepared,
           FatPrepared100G = product.FatPrepared100G == nutriments.FatPrepared100G ? null : product.FatPrepared100G,
           FatPreparedServing = product.FatPreparedServing == nutriments.FatPreparedServing
               ? null
               : product.FatPreparedServing,
           FatPreparedUnit = product.FatPreparedUnit == nutriments.FatPreparedUnit ? null : product.FatPreparedUnit,
           FatPreparedValue = product.FatPreparedValue == nutriments.FatPreparedValue ? null : product.FatPreparedValue,
           FatServing = product.FatServing == nutriments.FatServing ? null : product.FatServing,
           FatUnit = product.FatUnit == nutriments.FatUnit ? null : product.FatUnit,
           FatValue = product.FatValue == nutriments.FatValue ? null : product.FatValue,
           FiberPrepared = product.FiberPrepared == nutriments.FiberPrepared ? null : product.FiberPrepared,
           FiberPrepared100G = product.FiberPrepared100G == nutriments.FiberPrepared100G
               ? null
               : product.FiberPrepared100G,
           FiberPreparedServing = product.FiberPreparedServing == nutriments.FiberPreparedServing
               ? null
               : product.FiberPreparedServing,
           FiberPreparedUnit = product.FiberPreparedUnit == nutriments.FiberPreparedUnit
               ? null
               : product.FiberPreparedUnit,
           FiberPreparedValue = product.FiberPreparedValue == nutriments.FiberPreparedValue
               ? null
               : product.FiberPreparedValue,
           FruitsVegetablesLegumesEstimateFromIngredients100G =
               product.FruitsVegetablesLegumesEstimateFromIngredients100G ==
               nutriments.FruitsVegetablesLegumesEstimateFromIngredients100G
                   ? null
                   : product.FruitsVegetablesLegumesEstimateFromIngredients100G,
           FruitsVegetablesLegumesEstimateFromIngredientsServing =
               product.FruitsVegetablesLegumesEstimateFromIngredientsServing ==
               nutriments.FruitsVegetablesLegumesEstimateFromIngredientsServing
                   ? null
                   : product.FruitsVegetablesLegumesEstimateFromIngredientsServing,
           FruitsVegetablesNutsEstimateFromIngredients100G =
               product.FruitsVegetablesNutsEstimateFromIngredients100G ==
               nutriments.FruitsVegetablesNutsEstimateFromIngredients100G
                   ? null
                   : product.FruitsVegetablesNutsEstimateFromIngredients100G,
           FruitsVegetablesNutsEstimateFromIngredientsServing =
               product.FruitsVegetablesNutsEstimateFromIngredientsServing ==
               nutriments.FruitsVegetablesNutsEstimateFromIngredientsServing
                   ? null
                   : product.FruitsVegetablesNutsEstimateFromIngredientsServing,
           NovaGroup = product.NovaGroup == nutriments.NovaGroup ? null : product.NovaGroup,
           NovaGroup100G = product.NovaGroup100G == nutriments.NovaGroup100G ? null : product.NovaGroup100G,
           NovaGroupServing = product.NovaGroupServing == nutriments.NovaGroupServing ? null : product.NovaGroupServing,
           NutritionScoreFr = product.NutritionScoreFr == nutriments.NutritionScoreFr ? null : product.NutritionScoreFr,
           NutritionScoreFr100G = product.NutritionScoreFr100G == nutriments.NutritionScoreFr100G
               ? null
               : product.NutritionScoreFr100G,
           Proteins = product.Proteins == nutriments.Proteins ? null : product.Proteins,
           Proteins100G = product.Proteins100G == nutriments.Proteins100G ? null : product.Proteins100G,
           ProteinsPrepared = product.ProteinsPrepared == nutriments.ProteinsPrepared ? null : product.ProteinsPrepared,
           ProteinsPrepared100G = product.ProteinsPrepared100G == nutriments.ProteinsPrepared100G
               ? null
               : product.ProteinsPrepared100G,
           ProteinsPreparedServing = product.ProteinsPreparedServing == nutriments.ProteinsPreparedServing
               ? null
               : product.ProteinsPreparedServing,
           ProteinsPreparedUnit = product.ProteinsPreparedUnit == nutriments.ProteinsPreparedUnit
               ? null
               : product.ProteinsPreparedUnit,
           ProteinsPreparedValue = product.ProteinsPreparedValue == nutriments.ProteinsPreparedValue
               ? null
               : product.ProteinsPreparedValue,
           ProteinsServing = product.ProteinsServing == nutriments.ProteinsServing ? null : product.ProteinsServing,
           ProteinsUnit = product.ProteinsUnit == nutriments.ProteinsUnit ? null : product.ProteinsUnit,
           ProteinsValue = product.ProteinsValue == nutriments.ProteinsValue ? null : product.ProteinsValue,
           Salt = product.Salt == nutriments.Salt ? null : product.Salt,
           Salt100G = product.Salt100G == nutriments.Salt100G ? null : product.Salt100G,
           SaltPrepared = product.SaltPrepared == nutriments.SaltPrepared ? null : product.SaltPrepared,
           SaltPrepared100G = product.SaltPrepared100G == nutriments.SaltPrepared100G ? null : product.SaltPrepared100G,
           SaltPreparedServing = product.SaltPreparedServing == nutriments.SaltPreparedServing
               ? null
               : product.SaltPreparedServing,
           SaltPreparedUnit = product.SaltPreparedUnit == nutriments.SaltPreparedUnit ? null : product.SaltPreparedUnit,
           SaltPreparedValue = product.SaltPreparedValue == nutriments.SaltPreparedValue
               ? null
               : product.SaltPreparedValue,
           SaltServing = product.SaltServing == nutriments.SaltServing ? null : product.SaltServing,
           SaltUnit = product.SaltUnit == nutriments.SaltUnit ? null : product.SaltUnit,
           SaltValue = product.SaltValue == nutriments.SaltValue ? null : product.SaltValue,
           SaturatedFat = product.SaturatedFat == nutriments.SaturatedFat ? null : product.SaturatedFat,
           SaturatedFat100G = product.SaturatedFat100G == nutriments.SaturatedFat100G ? null : product.SaturatedFat100G,
           SaturatedFatPrepared = product.SaturatedFatPrepared == nutriments.SaturatedFatPrepared
               ? null
               : product.SaturatedFatPrepared,
           SaturatedFatPrepared100G = product.SaturatedFatPrepared100G == nutriments.SaturatedFatPrepared100G
               ? null
               : product.SaturatedFatPrepared100G,
           SaturatedFatPreparedServing = product.SaturatedFatPreparedServing == nutriments.SaturatedFatPreparedServing
               ? null
               : product.SaturatedFatPreparedServing,
           SaturatedFatPreparedUnit = product.SaturatedFatPreparedUnit == nutriments.SaturatedFatPreparedUnit
               ? null
               : product.SaturatedFatPreparedUnit,
           SaturatedFatPreparedValue = product.SaturatedFatPreparedValue == nutriments.SaturatedFatPreparedValue
               ? null
               : product.SaturatedFatPreparedValue,
           SaturatedFatServing = product.SaturatedFatServing == nutriments.SaturatedFatServing
               ? null
               : product.SaturatedFatServing,
           SaturatedFatUnit = product.SaturatedFatUnit == nutriments.SaturatedFatUnit ? null : product.SaturatedFatUnit,
           SaturatedFatValue = product.SaturatedFatValue == nutriments.SaturatedFatValue
               ? null
               : product.SaturatedFatValue,
           Sodium = product.Sodium == nutriments.Sodium ? null : product.Sodium,
           Sodium100G = product.Sodium100G == nutriments.Sodium100G ? null : product.Sodium100G,
           SodiumPrepared = product.SodiumPrepared == nutriments.SodiumPrepared ? null : product.SodiumPrepared,
           SodiumPrepared100G = product.SodiumPrepared100G == nutriments.SodiumPrepared100G
               ? null
               : product.SodiumPrepared100G,
           SodiumPreparedServing = product.SodiumPreparedServing == nutriments.SodiumPreparedServing
               ? null
               : product.SodiumPreparedServing,
           SodiumPreparedUnit = product.SodiumPreparedUnit == nutriments.SodiumPreparedUnit
               ? null
               : product.SodiumPreparedUnit,
           SodiumPreparedValue = product.SodiumPreparedValue == nutriments.SodiumPreparedValue
               ? null
               : product.SodiumPreparedValue,
           SodiumServing = product.SodiumServing == nutriments.SodiumServing ? null : product.SodiumServing,
           SodiumUnit = product.SodiumUnit == nutriments.SodiumUnit ? null : product.SodiumUnit,
           SodiumValue = product.SodiumValue == nutriments.SodiumValue ? null : product.SodiumValue,
           Sugars = product.Sugars == nutriments.Sugars ? null : product.Sugars,
           Sugars100G = product.Sugars100G == nutriments.Sugars100G ? null : product.Sugars100G,
           SugarsPrepared = product.SugarsPrepared == nutriments.SugarsPrepared ? null : product.SugarsPrepared,
           SugarsPrepared100G = product.SugarsPrepared100G == nutriments.SugarsPrepared100G
               ? null
               : product.SugarsPrepared100G,
           SugarsPreparedServing = product.SugarsPreparedServing == nutriments.SugarsPreparedServing
               ? null
               : product.SugarsPreparedServing,
           SugarsPreparedUnit = product.SugarsPreparedUnit == nutriments.SugarsPreparedUnit
               ? null
               : product.SugarsPreparedUnit,
           SugarsPreparedValue = product.SugarsPreparedValue == nutriments.SugarsPreparedValue
               ? null
               : product.SugarsPreparedValue,
           SugarsServing = product.SugarsServing == nutriments.SugarsServing ? null : product.SugarsServing,
           SugarsUnit = product.SugarsUnit == nutriments.SugarsUnit ? null : product.SugarsUnit,
           SugarsValue = product.SugarsValue == nutriments.SugarsValue ? null : product.SugarsValue
       };
       
       if (nutrimentsRequest.GetType().GetProperties()
           .Where(p => p.Name != "ProductId")
           .All(p => p.GetValue(nutrimentsRequest) == null))
       {
           return null;
       }

       return nutrimentsRequest;
   }
   
   public static CreateNutrimentSQLRequest MapToCreateNutrimentSQLRequest(this Nutriments nutriments, long productId)
    {
        return new CreateNutrimentSQLRequest
        {
            ProductId = productId,
            Carbohydrates = nutriments.Carbohydrates,
            Carbohydrates100G = nutriments.Carbohydrates100G,
            CarbohydratesPrepared = nutriments.CarbohydratesPrepared,
            CarbohydratesPrepared100G = nutriments.CarbohydratesPrepared100G,
            CarbohydratesPreparedServing = nutriments.CarbohydratesPreparedServing,
            CarbohydratesPreparedUnit = nutriments.CarbohydratesPreparedUnit,
            CarbohydratesPreparedValue = nutriments.CarbohydratesPreparedValue,
            CarbohydratesServing = nutriments.CarbohydratesServing,
            CarbohydratesUnit = nutriments.CarbohydratesUnit,
            CarbohydratesValue = nutriments.CarbohydratesValue,
            Energy = nutriments.Energy,
            EnergyKcal = nutriments.EnergyKcal,
            EnergyKcal100G = nutriments.EnergyKcal100G,
            EnergyKcalPrepared = nutriments.EnergyKcalPrepared,
            EnergyKcalPrepared100G = nutriments.EnergyKcalPrepared100G,
            EnergyKcalPreparedServing = nutriments.EnergyKcalPreparedServing,
            EnergyKcalPreparedUnit = nutriments.EnergyKcalPreparedUnit,
            EnergyKcalPreparedValue = nutriments.EnergyKcalPreparedValue,
            EnergyKcalServing = nutriments.EnergyKcalServing,
            EnergyKcalUnit = nutriments.EnergyKcalUnit,
            EnergyKcalValue = nutriments.EnergyKcalValue,
            EnergyKcalValueComputed = nutriments.EnergyKcalValueComputed,
            EnergyKj = nutriments.EnergyKj,
            EnergyKj100G = nutriments.EnergyKj100G,
            EnergyKjPrepared = nutriments.EnergyKjPrepared,
            EnergyKjPrepared100G = nutriments.EnergyKjPrepared100G,
            EnergyKjPreparedServing = nutriments.EnergyKjPreparedServing,
            EnergyKjPreparedUnit = nutriments.EnergyKjPreparedUnit,
            EnergyKjPreparedValue = nutriments.EnergyKjPreparedValue,
            EnergyKjServing = nutriments.EnergyKjServing,
            EnergyKjUnit = nutriments.EnergyKjUnit,
            EnergyKjValue = nutriments.EnergyKjValue,
            EnergyKjValueComputed = nutriments.EnergyKjValueComputed,
            Energy100G = nutriments.Energy100G,
            EnergyPrepared = nutriments.EnergyPrepared,
            EnergyPrepared100G = nutriments.EnergyPrepared100G,
            EnergyPreparedServing = nutriments.EnergyPreparedServing,
            EnergyPreparedUnit = nutriments.EnergyPreparedUnit,
            EnergyPreparedValue = nutriments.EnergyPreparedValue,
            EnergyServing = nutriments.EnergyServing,
            EnergyUnit = nutriments.EnergyUnit,
            EnergyValue = nutriments.EnergyValue,
            Fat = nutriments.Fat,
            Fat100G = nutriments.Fat100G,
            FatPrepared = nutriments.FatPrepared,
            FatPrepared100G = nutriments.FatPrepared100G,
            FatPreparedServing = nutriments.FatPreparedServing,
            FatPreparedUnit = nutriments.FatPreparedUnit,
            FatPreparedValue = nutriments.FatPreparedValue,
            FatServing = nutriments.FatServing,
            FatUnit = nutriments.FatUnit,
            FatValue = nutriments.FatValue,
            FiberPrepared = nutriments.FiberPrepared,
            FiberPrepared100G = nutriments.FiberPrepared100G,
            FiberPreparedServing = nutriments.FiberPreparedServing,
            FiberPreparedUnit = nutriments.FiberPreparedUnit,
            FiberPreparedValue = nutriments.FiberPreparedValue,
            FruitsVegetablesLegumesEstimateFromIngredients100G = nutriments.FruitsVegetablesLegumesEstimateFromIngredients100G,
            FruitsVegetablesLegumesEstimateFromIngredientsServing = nutriments.FruitsVegetablesLegumesEstimateFromIngredientsServing,
            FruitsVegetablesNutsEstimateFromIngredients100G = nutriments.FruitsVegetablesNutsEstimateFromIngredients100G,
            FruitsVegetablesNutsEstimateFromIngredientsServing = nutriments.FruitsVegetablesNutsEstimateFromIngredientsServing,
            NovaGroup = nutriments.NovaGroup,
            NovaGroup100G = nutriments.NovaGroup100G,
            NovaGroupServing = nutriments.NovaGroupServing,
            NutritionScoreFr = nutriments.NutritionScoreFr,
            NutritionScoreFr100G = nutriments.NutritionScoreFr100G,
            Proteins = nutriments.Proteins,
            Proteins100G = nutriments.Proteins100G,
            ProteinsPrepared = nutriments.ProteinsPrepared,
            ProteinsPrepared100G = nutriments.ProteinsPrepared100G,
            ProteinsPreparedServing = nutriments.ProteinsPreparedServing,
            ProteinsPreparedUnit = nutriments.ProteinsPreparedUnit,
            ProteinsPreparedValue = nutriments.ProteinsPreparedValue,
            ProteinsServing = nutriments.ProteinsServing,
            ProteinsUnit = nutriments.ProteinsUnit,
            ProteinsValue = nutriments.ProteinsValue,
            Salt = nutriments.Salt,
            Salt100G = nutriments.Salt100G,
            SaltPrepared = nutriments.SaltPrepared,
            SaltPrepared100G = nutriments.SaltPrepared100G,
            SaltPreparedServing = nutriments.SaltPreparedServing,
            SaltPreparedUnit = nutriments.SaltPreparedUnit,
            SaltPreparedValue = nutriments.SaltPreparedValue,
            SaltServing = nutriments.SaltServing,
            SaltUnit = nutriments.SaltUnit,
            SaltValue = nutriments.SaltValue,
            SaturatedFat = nutriments.SaturatedFat,
            SaturatedFat100G = nutriments.SaturatedFat100G,
            SaturatedFatPrepared = nutriments.SaturatedFatPrepared,
            SaturatedFatPrepared100G = nutriments.SaturatedFatPrepared100G,
            SaturatedFatPreparedServing = nutriments.SaturatedFatPreparedServing,
            SaturatedFatPreparedUnit = nutriments.SaturatedFatPreparedUnit,
            SaturatedFatPreparedValue = nutriments.SaturatedFatPreparedValue,
            SaturatedFatServing = nutriments.SaturatedFatServing,
            SaturatedFatUnit = nutriments.SaturatedFatUnit,
            SaturatedFatValue = nutriments.SaturatedFatValue,
            Sodium = nutriments.Sodium,
            Sodium100G = nutriments.Sodium100G,
            SodiumPrepared = nutriments.SodiumPrepared,
            SodiumPrepared100G = nutriments.SodiumPrepared100G,
            SodiumPreparedServing = nutriments.SodiumPreparedServing,
            SodiumPreparedUnit = nutriments.SodiumPreparedUnit,
            SodiumPreparedValue = nutriments.SodiumPreparedValue,
            SodiumServing = nutriments.SodiumServing,
            SodiumUnit = nutriments.SodiumUnit,
            SodiumValue = nutriments.SodiumValue,
            Sugars = nutriments.Sugars,
            Sugars100G = nutriments.Sugars100G,
            SugarsPrepared = nutriments.SugarsPrepared,
            SugarsPrepared100G = nutriments.SugarsPrepared100G,
            SugarsPreparedServing = nutriments.SugarsPreparedServing,
            SugarsPreparedUnit = nutriments.SugarsPreparedUnit,
            SugarsPreparedValue = nutriments.SugarsPreparedValue,
            SugarsServing = nutriments.SugarsServing,
            SugarsUnit = nutriments.SugarsUnit,
            SugarsValue = nutriments.SugarsValue
        };
    }
}