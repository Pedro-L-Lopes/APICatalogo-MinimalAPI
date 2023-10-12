using APICatalogo_MinimalAPI.ApiEndpoints;
using APICatalogo_MinimalAPI.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJtw();

// Configure services
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapAutenticacaoEndpoint();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();

var enviroment = app.Environment;

app.UseExceptionHandling(enviroment)
    .UseSwaggerMiddleware()
    .UseAppCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
