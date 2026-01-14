using System;
using Microsoft.AspNetCore.Mvc;
using OrderlyACS.Domain.Products;
using OrderlyACS.Infra.Data;

namespace OrderlyACS.Endpoints.Categories;

public class CategoryPut
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => [HttpMethod.Put.ToString()];
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id, [FromBody] CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var category = context.Categories.FirstOrDefault(c => c.Id == id);

        if (category == null)
            return Results.NotFound();

        category.EditInfo(categoryRequest.Name, categoryRequest.Active);

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());

        context.SaveChanges();

        return Results.NoContent();
    }
}
