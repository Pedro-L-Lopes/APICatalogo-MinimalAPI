using APICatalogo_MinimalAPI.Context;
using APICatalogo_MinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo_MinimalAPI.ApiEndpoints;

public static class ProdutosEndpoints
{
    public static void MapProdutosEndpoints(this WebApplication app)
    {
        app.MapPost("/produtos", async (Produto produto, AppDbContext db) =>
        {
            db.Produtos.Add(produto);
            await db.SaveChangesAsync();

            return Results.Created($"produtos/{produto.ProdutoId}", produto);
        });

        app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync()).WithTags("Produtos").RequireAuthorization();

        app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
        {
            return await db.Produtos.FindAsync(id)
            is Produto produto ? Results.Ok(produto) : Results.NotFound("Produto não encontrado");
        });

        app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContext db) =>
        {
            if (produto.ProdutoId != id)
            {
                return Results.BadRequest("Produto não encontrado");
            }

            var produtoDb = await db.Produtos.FindAsync(id);

            if (produtoDb == null) return Results.NotFound();

            produtoDb.Nome = produto.Nome;
            produtoDb.Descricao = produto.Descricao;
            produtoDb.Imagem = produto.Imagem;
            produtoDb.Preco = produto.Preco;
            produtoDb.DataCompra = produto.DataCompra;
            produtoDb.Estoque = produto.Estoque;

            await db.SaveChangesAsync();
            return Results.Ok(produtoDb);
        });

        app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext db) =>
        {
            var produto = await db.Produtos.FindAsync(id);

            if (produto is null)
            {
                return Results.NotFound("Produto não encontrado");
            }

            db.Produtos.Remove(produto);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}

