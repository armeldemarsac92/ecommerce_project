using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json;
using Dapper;
using Tdev702.Contracts.SQL;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Repository.Config;

public class DapperMappingConfiguration
{
    public static void ConfigureMappings()
    {
        SqlMapper.AddTypeHandler(new JsonTypeHandler<OrderItem[]>());

        SetupDapperMapping<FullProductSQLResponse>();
        SetupDapperMapping<BrandSQLResponse>();
        SetupDapperMapping<CategorySQLResponse>();
        SetupDapperMapping<InventorySQLResponse>();
        SetupDapperMapping<TagSQLResponse>();
        SetupDapperMapping<ProductTagSQLResponse>();
        SetupDapperMapping<OrderSQLResponse>();
        SetupDapperMapping<OrderProductSQLResponse>();
        SetupDapperMapping<OrderSummarySQLResponse>();
        SetupDapperMapping<CustomerSQLResponse>();
        SetupDapperMapping<NutrimentSQLResponse>();
        SetupDapperMapping<OrderItem>();
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

public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public override T Parse(object value)
    {
        return JsonSerializer.Deserialize<T>(value.ToString(), _options);
    }

    public override void SetValue(IDbDataParameter parameter, T value)
    {
        parameter.Value = JsonSerializer.Serialize(value, _options);
        parameter.DbType = DbType.String;
    }
}