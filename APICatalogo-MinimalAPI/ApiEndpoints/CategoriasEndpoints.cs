using APICatalogo_MinimalAPI.Context;
using APICatalogo_MinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo_MinimalAPI.ApiEndpoints;
public static class CategoriasEndpoints
{
    public static void MapCategoriasEndpoints(this WebApplication app)
    {
        app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
        {
            db.Categorias.Add(categoria);
            await db.SaveChangesAsync();

            return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
        });

        app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias.ToListAsync()).WithTags("Categorias").RequireAuthorization();

        app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
        {
            return await db.Categorias.FindAsync(id)
            is Categoria categoria ? Results.Ok(categoria) : Results.NotFound("Categoria não encontrada");
        });

        app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
        {
            if (categoria.CategoriaId != id)
            {
                return Results.BadRequest("Categoria não encontrada");
            }

            var categoriaDb = await db.Categorias.FindAsync(id);

            if (categoriaDb is null) return Results.NotFound();

            categoriaDb.Nome = categoria.Nome;
            categoriaDb.Descricao = categoria.Descricao;

            await db.SaveChangesAsync();
            return Results.Ok(categoriaDb);
        });

        app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
        {
            var categoria = await db.Categorias.FindAsync(id);

            if (categoria is null)
            {
                return Results.NotFound("Categoria não encontrada ");
            }

            db.Categorias.Remove(categoria);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
