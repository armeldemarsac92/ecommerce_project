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
        CASE WHEN @orderBy = 2 THEN ""Id"" END DESC,
        CASE WHEN @orderBy = 1 THEN ""Id"" END ASC
    LIMIT @pageSize 
    OFFSET @offset;";

     public static string GetCustomersCountByDateRange = @"
     SELECT count(*) 
    FROM backoffice.vw_customers cust
    WHERE cust.""CreatedAt"" BETWEEN @StartDate AND @EndDate;";
 }
