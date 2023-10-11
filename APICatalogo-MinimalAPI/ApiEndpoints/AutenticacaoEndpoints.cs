using APICatalogo_MinimalAPI.Models;
using APICatalogo_MinimalAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace APICatalogo_MinimalAPI.ApiEndpoints;
public static class AutenticacaoEndpoints
{
    public static void MapAutenticacaoEndpoint(this WebApplication app)
    {
        app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
        {
            if (userModel == null)
            {
                return Results.BadRequest("Login Inválido");
            }
            if (userModel.UserName == "macoratti" && userModel.Password == "numsey#123")
            {
                var tokenString = tokenService.GerarToken(app.Configuration["Jwt:Key"],
                    app.Configuration["Jwt:Issuer"],
                    app.Configuration["Jwt:Audience"],
                    userModel);
                return Results.Ok(new { token = tokenString });
            }
            else
            {
                return Results.BadRequest("Login Inválido");
            }
        }).Produces(StatusCodes.Status400BadRequest)
              .Produces(StatusCodes.Status200OK)
              .WithName("Login")
              .WithTags("Autenticacao");
    }
}
