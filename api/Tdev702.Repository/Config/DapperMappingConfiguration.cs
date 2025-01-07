using System.ComponentModel.DataAnnotations.Schema;
using Dapper;
using Tdev702.Contracts.SQL.Response.Shop;

namespace Tdev702.Repository.Config;

public class DapperMappingConfiguration
{
    public static void ConfigureMappings()
    {
        SetupDapperMapping<ProductResponse>();
        SetupDapperMapping<BrandResponse>();
        SetupDapperMapping<CategoryResponse>();
        SetupDapperMapping<ProductTagResponse>();
    }

    private static void SetupDapperMapping<TModel>()
    {
        SqlMapper.SetTypeMap(
            typeof(TModel),
            new CustomPropertyTypeMap(
                typeof(TModel),
                (type, columnName) =>
                    type.GetProperties().FirstOrDefault(prop =>
                        prop.GetCustomAttributes(false)
                            .OfType<ColumnAttribute>()
                            .Any(attr => attr.Name == columnName))));
    }
}