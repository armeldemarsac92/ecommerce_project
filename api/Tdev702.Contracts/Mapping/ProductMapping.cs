using Tdev702.Contracts.API.Request.Product;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Request.Product;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Contracts.Mapping;

public static class ProductMapping
{
    public static CreateProductSQLRequest MapToCreateProductRequest(this CreateProductRequest createProductRequest)
    {
        return new CreateProductSQLRequest
        {
            Title = createProductRequest.Title,
            Description = createProductRequest.Description,
            Price = createProductRequest.Price,
            BrandId = createProductRequest.BrandId,
            CategoryId = createProductRequest.CategoryId,
            OpenFoodFactId = createProductRequest.OpenFoodFactId,
            ImageUrl = createProductRequest.ImageUrl
        };
    }

    public static UpdateProductSQLRequest MapToUpdateProductRequest(this UpdateProductRequest updateProductRequest,
        long productId)
    {
        return new UpdateProductSQLRequest
        {
            Id = productId,
            Title = updateProductRequest.Title,
            Description = updateProductRequest.Description,
            Price = updateProductRequest.Price,
            BrandId = updateProductRequest.BrandId,
            CategoryId = updateProductRequest.CategoryId,
            OpenFoodFactId = updateProductRequest.OpenFoodFactId,
            ImageUrl = updateProductRequest.ImageUrl
        };
    }

public static ShopProductResponse MapToProduct(this FullProductSQLResponse fullProductSqlResponse)
{
    return new ShopProductResponse
    {
        Id = fullProductSqlResponse.Id,
        Title = fullProductSqlResponse.Title,
        Description = fullProductSqlResponse.Description,
        Price = fullProductSqlResponse.Price,
        PriceHt = fullProductSqlResponse.Price / 1.2,
        BrandTitle = fullProductSqlResponse.BrandTitle,
        CategoryTitle = fullProductSqlResponse.CategoryTitle,
        OpenFoodFactId = fullProductSqlResponse.OpenFoodFactId,
        ImageUrl = fullProductSqlResponse.ImageUrl,
        Tags = fullProductSqlResponse.Tags,
        UpdatedAt = fullProductSqlResponse.UpdatedAt,
        CreatedAt = fullProductSqlResponse.CreatedAt,
        Carbohydrates = fullProductSqlResponse.Carbohydrates,
        Carbohydrates100G = fullProductSqlResponse.Carbohydrates100G,
        CarbohydratesPrepared = fullProductSqlResponse.CarbohydratesPrepared,
        CarbohydratesPrepared100G = fullProductSqlResponse.CarbohydratesPrepared100G,
        CarbohydratesPreparedServing = fullProductSqlResponse.CarbohydratesPreparedServing,
        CarbohydratesPreparedUnit = fullProductSqlResponse.CarbohydratesPreparedUnit,
        CarbohydratesPreparedValue = fullProductSqlResponse.CarbohydratesPreparedValue,
        CarbohydratesServing = fullProductSqlResponse.CarbohydratesServing,
        CarbohydratesUnit = fullProductSqlResponse.CarbohydratesUnit,
        CarbohydratesValue = fullProductSqlResponse.CarbohydratesValue,
        Energy = fullProductSqlResponse.Energy,
        EnergyKcal = fullProductSqlResponse.EnergyKcal,
        EnergyKcal100G = fullProductSqlResponse.EnergyKcal100G,
        EnergyKcalPrepared = fullProductSqlResponse.EnergyKcalPrepared,
        EnergyKcalPrepared100G = fullProductSqlResponse.EnergyKcalPrepared100G,
        EnergyKcalPreparedServing = fullProductSqlResponse.EnergyKcalPreparedServing,
        EnergyKcalPreparedUnit = fullProductSqlResponse.EnergyKcalPreparedUnit,
        EnergyKcalPreparedValue = fullProductSqlResponse.EnergyKcalPreparedValue,
        EnergyKcalServing = fullProductSqlResponse.EnergyKcalServing,
        EnergyKcalUnit = fullProductSqlResponse.EnergyKcalUnit,
        EnergyKcalValue = fullProductSqlResponse.EnergyKcalValue,
        EnergyKcalValueComputed = fullProductSqlResponse.EnergyKcalValueComputed,
        EnergyKj = fullProductSqlResponse.EnergyKj,
        EnergyKj100G = fullProductSqlResponse.EnergyKj100G,
        EnergyKjPrepared = fullProductSqlResponse.EnergyKjPrepared,
        EnergyKjPrepared100G = fullProductSqlResponse.EnergyKjPrepared100G,
        EnergyKjPreparedServing = fullProductSqlResponse.EnergyKjPreparedServing,
        EnergyKjPreparedUnit = fullProductSqlResponse.EnergyKjPreparedUnit,
        EnergyKjPreparedValue = fullProductSqlResponse.EnergyKjPreparedValue,
        EnergyKjServing = fullProductSqlResponse.EnergyKjServing,
        EnergyKjUnit = fullProductSqlResponse.EnergyKjUnit,
        EnergyKjValue = fullProductSqlResponse.EnergyKjValue,
        EnergyKjValueComputed = fullProductSqlResponse.EnergyKjValueComputed,
        Energy100G = fullProductSqlResponse.Energy100G,
        EnergyPrepared = fullProductSqlResponse.EnergyPrepared,
        EnergyPrepared100G = fullProductSqlResponse.EnergyPrepared100G,
        EnergyPreparedServing = fullProductSqlResponse.EnergyPreparedServing,
        EnergyPreparedUnit = fullProductSqlResponse.EnergyPreparedUnit,
        EnergyPreparedValue = fullProductSqlResponse.EnergyPreparedValue,
        EnergyServing = fullProductSqlResponse.EnergyServing,
        EnergyUnit = fullProductSqlResponse.EnergyUnit,
        EnergyValue = fullProductSqlResponse.EnergyValue,
        Fat = fullProductSqlResponse.Fat,
        Fat100G = fullProductSqlResponse.Fat100G,
        FatPrepared = fullProductSqlResponse.FatPrepared,
        FatPrepared100G = fullProductSqlResponse.FatPrepared100G,
        FatPreparedServing = fullProductSqlResponse.FatPreparedServing,
        FatPreparedUnit = fullProductSqlResponse.FatPreparedUnit,
        FatPreparedValue = fullProductSqlResponse.FatPreparedValue,
        FatServing = fullProductSqlResponse.FatServing,
        FatUnit = fullProductSqlResponse.FatUnit,
        FatValue = fullProductSqlResponse.FatValue,
        FiberPrepared = fullProductSqlResponse.FiberPrepared,
        FiberPrepared100G = fullProductSqlResponse.FiberPrepared100G,
        FiberPreparedServing = fullProductSqlResponse.FiberPreparedServing,
        FiberPreparedUnit = fullProductSqlResponse.FiberPreparedUnit,
        FiberPreparedValue = fullProductSqlResponse.FiberPreparedValue,
        FruitsVegetablesLegumesEstimateFromIngredients100G = fullProductSqlResponse.FruitsVegetablesLegumesEstimateFromIngredients100G,
        FruitsVegetablesLegumesEstimateFromIngredientsServing = fullProductSqlResponse.FruitsVegetablesLegumesEstimateFromIngredientsServing,
        FruitsVegetablesNutsEstimateFromIngredients100G = fullProductSqlResponse.FruitsVegetablesNutsEstimateFromIngredients100G,
        FruitsVegetablesNutsEstimateFromIngredientsServing = fullProductSqlResponse.FruitsVegetablesNutsEstimateFromIngredientsServing,
        NovaGroup = fullProductSqlResponse.NovaGroup,
        NovaGroup100G = fullProductSqlResponse.NovaGroup100G,
        NovaGroupServing = fullProductSqlResponse.NovaGroupServing,
        NutritionScoreFr = fullProductSqlResponse.NutritionScoreFr,
        NutritionScoreFr100G = fullProductSqlResponse.NutritionScoreFr100G,
        Proteins = fullProductSqlResponse.Proteins,
        Proteins100G = fullProductSqlResponse.Proteins100G,
        ProteinsPrepared = fullProductSqlResponse.ProteinsPrepared,
        ProteinsPrepared100G = fullProductSqlResponse.ProteinsPrepared100G,
        ProteinsPreparedServing = fullProductSqlResponse.ProteinsPreparedServing,
        ProteinsPreparedUnit = fullProductSqlResponse.ProteinsPreparedUnit,
        ProteinsPreparedValue = fullProductSqlResponse.ProteinsPreparedValue,
        ProteinsServing = fullProductSqlResponse.ProteinsServing,
        ProteinsUnit = fullProductSqlResponse.ProteinsUnit,
        ProteinsValue = fullProductSqlResponse.ProteinsValue,
        Salt = fullProductSqlResponse.Salt,
        Salt100G = fullProductSqlResponse.Salt100G,
        SaltPrepared = fullProductSqlResponse.SaltPrepared,
        SaltPrepared100G = fullProductSqlResponse.SaltPrepared100G,
        SaltPreparedServing = fullProductSqlResponse.SaltPreparedServing,
        SaltPreparedUnit = fullProductSqlResponse.SaltPreparedUnit,
        SaltPreparedValue = fullProductSqlResponse.SaltPreparedValue,
        SaltServing = fullProductSqlResponse.SaltServing,
        SaltUnit = fullProductSqlResponse.SaltUnit,
        SaltValue = fullProductSqlResponse.SaltValue,
        SaturatedFat = fullProductSqlResponse.SaturatedFat,
        SaturatedFat100G = fullProductSqlResponse.SaturatedFat100G,
        SaturatedFatPrepared = fullProductSqlResponse.SaturatedFatPrepared,
        SaturatedFatPrepared100G = fullProductSqlResponse.SaturatedFatPrepared100G,
        SaturatedFatPreparedServing = fullProductSqlResponse.SaturatedFatPreparedServing,
        SaturatedFatPreparedUnit = fullProductSqlResponse.SaturatedFatPreparedUnit,
        SaturatedFatPreparedValue = fullProductSqlResponse.SaturatedFatPreparedValue,
        SaturatedFatServing = fullProductSqlResponse.SaturatedFatServing,
        SaturatedFatUnit = fullProductSqlResponse.SaturatedFatUnit,
        SaturatedFatValue = fullProductSqlResponse.SaturatedFatValue,
        Sodium = fullProductSqlResponse.Sodium,
        Sodium100G = fullProductSqlResponse.Sodium100G,
        SodiumPrepared = fullProductSqlResponse.SodiumPrepared,
        SodiumPrepared100G = fullProductSqlResponse.SodiumPrepared100G,
        SodiumPreparedServing = fullProductSqlResponse.SodiumPreparedServing,
        SodiumPreparedUnit = fullProductSqlResponse.SodiumPreparedUnit,
        SodiumPreparedValue = fullProductSqlResponse.SodiumPreparedValue,
        SodiumServing = fullProductSqlResponse.SodiumServing,
        SodiumUnit = fullProductSqlResponse.SodiumUnit,
        SodiumValue = fullProductSqlResponse.SodiumValue,
        Sugars = fullProductSqlResponse.Sugars,
        Sugars100G = fullProductSqlResponse.Sugars100G,
        SugarsPrepared = fullProductSqlResponse.SugarsPrepared,
        SugarsPrepared100G = fullProductSqlResponse.SugarsPrepared100G,
        SugarsPreparedServing = fullProductSqlResponse.SugarsPreparedServing,
        SugarsPreparedUnit = fullProductSqlResponse.SugarsPreparedUnit,
        SugarsPreparedValue = fullProductSqlResponse.SugarsPreparedValue,
        SugarsServing = fullProductSqlResponse.SugarsServing,
        SugarsUnit = fullProductSqlResponse.SugarsUnit,
        SugarsValue = fullProductSqlResponse.SugarsValue
    };
}
    
    public static List<ShopProductResponse> MapToProducts(this List<FullProductSQLResponse> productResponses)
    {
        return productResponses.Select(MapToProduct).ToList();
    }
}