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
    public string? FirstName { get; set; }
    
    [StringLength(50)]
    public string? LastName { get; set; }    
    
    [StringLength(255)]
    public string? ProfilePicture { get; set; }
    
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CreatedAt { get; set; }
}