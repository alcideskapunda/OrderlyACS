using OrderlyACS.Infra.Data;

namespace OrderlyACS.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => [HttpMethod.Get.ToString()];
    public static Delegate Handle => Action;

    public static IResult Action(int? page, int? rows, QueryAllUsersAndClaimName query)
    {
        return Results.Ok(query.Execute(page.Value, rows.Value));
    }
}