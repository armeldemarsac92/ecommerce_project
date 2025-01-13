using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tdev702.Auth.Database;
using Tdev702.Auth.Routes;

namespace Tdev702.Auth.Endpoints;

public static class RoleEndpoints
{
    public static IEndpointRouteBuilder MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.Roles.GetAll, GetRoles)
            .RequireAuthorization("AdminPolicy")
            .WithName("GetRoles");

        app.MapPost(ApiRoutes.Roles.Create, CreateRole)
            .RequireAuthorization("AdminPolicy")
            .WithName("CreateRole");

        app.MapDelete(ApiRoutes.Roles.Delete, DeleteRole)
            .RequireAuthorization("AdminPolicy")
            .WithName("DeleteRole");

        app.MapPost(ApiRoutes.Roles.AddUserToRole, AddUserToRole)
            .RequireAuthorization("AdminPolicy")
            .WithName("AddUserToRole");

        app.MapDelete(ApiRoutes.Roles.RemoveUserFromRole, RemoveUserFromRole)
            .RequireAuthorization("AdminPolicy")
            .WithName("RemoveUserFromRole");

        app.MapGet(ApiRoutes.Roles.GetUserRoles, GetUserRoles)
            .RequireAuthorization("AdminPolicy")
            .WithName("GetUserRoles");
        
        return app;
    }


    private static async Task<IResult> GetRoles(RoleManager<Role> roleManager)
    {
        var roles = await roleManager.Roles.ToListAsync();
        return Results.Ok(roles);
    }

    private static async Task<IResult> CreateRole(RoleManager<Role> roleManager, [FromBody] string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
            return Results.BadRequest($"Role {roleName} already exists");

        var result = await roleManager.CreateAsync(new Role { Name = roleName });
        if (result.Succeeded)
            return Results.Ok();

        return Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> DeleteRole(RoleManager<Role> roleManager, string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
            return Results.NotFound();

        var result = await roleManager.DeleteAsync(role);
        if (result.Succeeded)
            return Results.Ok();

        return Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> AddUserToRole(UserManager<User> userManager, string userId,
        [FromBody] string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return Results.NotFound();

        var result = await userManager.AddToRoleAsync(user, roleName);
        if (result.Succeeded)
            return Results.Ok();

        return Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> RemoveUserFromRole(UserManager<User> userManager, string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return Results.NotFound();

        var result = await userManager.RemoveFromRoleAsync(user, roleName);
        if (result.Succeeded)
            return Results.Ok();

        return Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> GetUserRoles(UserManager<User> userManager, string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return Results.NotFound();

        var roles = await userManager.GetRolesAsync(user);
        return Results.Ok(roles);
    }
}