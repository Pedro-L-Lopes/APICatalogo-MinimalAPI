using APICatalogo_MinimalAPI.Models;

namespace APICatalogo_MinimalAPI.Services;

public interface ITokenService
{
    string GerarToken(string key, string issuer, string audience, UserModel user);
}
