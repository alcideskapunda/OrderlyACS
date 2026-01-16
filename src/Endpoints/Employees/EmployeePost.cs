using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OrderlyACS.Endpoints.Emploees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => [HttpMethod.Post.ToString()];
    public static Delegate Handle => Action;

    public static IResult Action([FromBody] EmployeeRequest employeeRequest, UserManager<IdentityUser> userManager)
    {
        var user = new IdentityUser
        {
            UserName = employeeRequest.Email,
            Email = employeeRequest.Email
        };

        var result = userManager.CreateAsync(user, employeeRequest.Password).Result;

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim>
        {
            new("EmployeeCode", employeeRequest.EmployeeCode),
            new("Name", employeeRequest.Name)
        };

        var claimsResult = userManager.AddClaimsAsync(user, userClaims).Result;

        if (!claimsResult.Succeeded)
            return Results.ValidationProblem(claimsResult.Errors.ConvertToProblemDetails());

        return Results.Created($"/employees/{user.Id}", user.Id);
    }
}
