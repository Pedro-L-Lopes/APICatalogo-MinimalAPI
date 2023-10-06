using APICatalogo_MinimalAPI.Context;
using APICatalogo_MinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options
    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

var app = builder.Build();

app.MapGet("/", () => "Hello world");

app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});

app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias.ToListAsync());

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

app.MapPost("/produtos", async (Produto produto, AppDbContext db) =>
{
    db.Produtos.Add(produto);
    await db.SaveChangesAsync();

    return Results.Created($"produtos/{produto.ProdutoId}", produto);
});

app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync());

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


// Configure
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
