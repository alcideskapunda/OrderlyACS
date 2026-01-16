using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderlyACS.Endpoints.Emploees;

namespace OrderlyACS.Endpoints.Employees;

public class EmployeePostAll
{
    public static string Template => "/employeesAll";
    public static string[] Methods => [HttpMethod.Post.ToString()];
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(
        [FromBody] List<EmployeeRequest> employees,
        UserManager<IdentityUser> userManager)
    {
        var createdUsers = new List<object>();
        var errors = new List<object>();

        foreach (var employee in employees)
        {
            var user = new IdentityUser
            {
                UserName = employee.Email,
                Email = employee.Email
            };

            var result = await userManager.CreateAsync(user, employee.Password);

            if (!result.Succeeded)
            {
                errors.Add(new
                {
                    employee.Email,
                    Errors = result.Errors.Select(e => e.Description)
                });
                continue;
            }

            var claims = new List<Claim>
            {
                new("EmployeeCode", employee.EmployeeCode),
                new("Name", employee.Name)
            };

            var claimsResult = await userManager.AddClaimsAsync(user, claims);

            if (!claimsResult.Succeeded)
            {
                errors.Add(new
                {
                    employee.Email,
                    Errors = claimsResult.Errors.Select(e => e.Description)
                });
                continue;
            }

            createdUsers.Add(new
            {
                user.Id,
                user.Email
            });
        }

        if (errors.Any())
        {
            return Results.BadRequest(new
            {
                Created = createdUsers,
                Errors = errors
            });
        }

        return Results.Created("/employees", createdUsers);
    }
}
