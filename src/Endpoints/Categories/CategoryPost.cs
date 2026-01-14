using System;
using Microsoft.AspNetCore.Mvc;
using OrderlyACS.Domain.Products;
using OrderlyACS.Infra.Data;

namespace OrderlyACS.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => [HttpMethod.Post.ToString()];
    public static Delegate Handle => Action;

    public static IResult Action([FromBody] CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var category = new Category
        {
            Name = categoryRequest.Name,
            CreateBy = "testS",
            CreateOn = DateTime.Now,
            EditedBy = "testE",
            EditedOn = DateTime.Now
        };
        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}

