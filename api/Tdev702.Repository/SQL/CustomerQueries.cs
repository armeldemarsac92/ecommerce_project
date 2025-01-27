namespace Tdev702.Repository.SQL;

public class CustomerQueries
{
    public static string GetCustomerById = @"
    SELECT *
    FROM backoffice.vw_customers
    WHERE ""Id"" = @UserId;";
    
    public static string GetAllCustomers = @"
    SELECT *
    FROM backoffice.vw_customers
    ORDER BY 
        CASE WHEN @orderBy = 'DESC' THEN ""Id"" END DESC,
        CASE WHEN @orderBy = 'ASC' THEN ""Id"" END ASC
    LIMIT @pageSize 
    OFFSET @offset;";
}











































