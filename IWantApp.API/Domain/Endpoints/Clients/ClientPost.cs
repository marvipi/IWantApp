using IWantApp.API.Domain.Services;

namespace IWantApp.API.Domain.Endpoints.Clients;

/// <summary>
/// Representa uma requisição HTTP usada para registrar um novo cliente.
/// </summary>
public static class ClientPost
{
    /// <summary>
    /// O endpoint onde os clientes estão localizados.
    /// </summary>
    public static string Template => "/clients";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    /// <summary>
    /// Uma referência a um método que registra clientes no sistema.
    /// </summary>
    public static Delegate Handler => Action;

    [AllowAnonymous]
    private static async Task<IResult> Action(ClientRequest request, UserCreator userCreator)
    {
        var userClaims = new List<Claim>() {
            new(ClaimTypes.Name, request.Name),
            new("CPF", request.CPF)
        };

        (var result, var newUserId) = await userCreator.Create(request.Email, request.Password, userClaims);

        if (!result.Succeeded) return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        return Results.Created(Template + $"/{newUserId}", newUserId);


    }
}
