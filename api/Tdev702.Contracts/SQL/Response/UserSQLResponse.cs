using System.ComponentModel.DataAnnotations.Schema;

namespace Tdev702.Contracts.SQL.Response;

public class UserSQLResponse
{
    [Column("username")]
    public required string Username { get; init; }
    
    [Column("email")]
    public required string Email { get; init; }
    
    [Column("emailConfirmed")]
    public required bool EmailConfirmed { get; init; }
    
    [Column("lockoutEnabled")]
    public required bool LockoutEnabled { get; init; }
    
    [Column("phoneNumber")]
    public required string PhoneNumber { get; init; }
    
    [Column("role")]
    public required string Role { get; init; }
}

