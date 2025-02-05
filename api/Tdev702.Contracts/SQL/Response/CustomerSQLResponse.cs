using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response;

public class CustomerSQLResponse
{
    [Column("Id")]
    public string Id { get; set; }
    
    [Column("UserName")]
    public string Username { get; set; }
    
    [Column("Email")]
    public string Email { get; set; }
    
    [Column("EmailConfirmed")]
    public bool EmailConfirmed { get; set; }
    
    [Column("PhoneNumber")]
    public string PhoneNumber { get; set; }
    
    [Column("StripeId")]
    public string StripeId { get; set; }
    
    [Column("Role")]
    public string Role { get; set; }
    
    [Column("LockoutEnabled")]
    public string LockoutEnabled { get; set; }
}