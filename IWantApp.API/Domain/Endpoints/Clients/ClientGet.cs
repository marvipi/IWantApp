using IWantApp.API.Domain.Endpoints.Users;

namespace IWantApp.API.Domain.Endpoints.Clients;

/// <summary>
/// Representa uma requisição HTTP que busca informações sobre o cliente que está logado neste sistema.
/// </summary>
public static class ClientGet
{
    /// <summary>
    /// O endpoint onde os clientes estão localizados.
    /// </summary>
    public static string Template => "/clients";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    /// <summary>
    /// Uma referência a um método que busca informações sobre clientes.
    /// </summary>
    public static Delegate Handler => Action;

    private static IResult Action(HttpContext httpContext)
    {
        var user = httpContext.User;
        var response = new
        {
            Id = user.FindFirstValue(ClaimTypes.NameIdentifier),
            Email = user.FindFirstValue(ClaimTypes.Email),
            CPF = user.FindFirstValue("CPF")
        };

        return Results.Ok(response);
    }
}
