using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Tdev702.Contracts.Auth;

namespace Tdev702.Contracts.Database;

public class User : IdentityUser
{
    public TwoFactorType? PreferredTwoFactorProvider { get; set; }
    
    [StringLength(50)]
    public string? StripeCustomerId { get; set; }
    
    [StringLength(50)]
    public required string FirstName { get; set; }
    
    [StringLength(50)]
    public required string LastName { get; set; }    
    
    [StringLength(50)]
    public string? ProfilePicture { get; set; }
}