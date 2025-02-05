using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.SQL.Response;

namespace Tdev702.Contracts.Mapping;

public static class CustomerMapping
{
    public static CustomerResponse MapToCustomer(this CustomerSQLResponse customerSqlResponse)
    {
        return new CustomerResponse
        {
            Id = customerSqlResponse.Id,
            Username = customerSqlResponse.Username,
            Email = customerSqlResponse.Email,
            EmailConfirmed = customerSqlResponse.EmailConfirmed,
            PhoneNumber = customerSqlResponse.PhoneNumber,
            StripeId = customerSqlResponse.StripeId,
            Role = customerSqlResponse.Role,
            LockoutEnabled = customerSqlResponse.LockoutEnabled
        };
    }

    public static List<CustomerResponse> MapToCustomers(this List<CustomerSQLResponse> customerSqlResponses)
    {
        return customerSqlResponses.Select(MapToCustomer).ToList();
    }
}