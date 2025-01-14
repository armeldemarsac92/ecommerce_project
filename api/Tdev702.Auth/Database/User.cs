using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Tdev702.Auth.Models;

namespace Tdev702.Auth.Database;

public class User : IdentityUser
{
    public TwoFactorType? PreferredTwoFactorProvider { get; set; }
    
    [StringLength(50)]
    public required string FirstName { get; set; }
    
    [StringLength(50)]
    public required string LastName { get; set; }
}