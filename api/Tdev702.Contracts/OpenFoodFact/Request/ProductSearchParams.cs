using Refit;

namespace Tdev702.Contracts.OpenFoodFact.Request;

public class ProductSearchParams
{
    [AliasAs("categories_tags_fr")]
    public string Category { get; set; }

    [AliasAs("nutrition_grades_tags")]
    public string NutritionGrade { get; set; }

    [AliasAs("fields")]
    public string Fields { get; set; }

    [AliasAs("page_size")]
    public int PageSize { get; set; }
}