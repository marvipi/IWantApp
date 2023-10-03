using IWantApp.API.Domain.Extensions;

namespace IWantApp.API.Domain.Endpoints.Orders;

/// <summary>
/// Representa uma requisição HTTP usada para fazer um novo pedido.
/// </summary>
public static class OrderPost
{
    /// <summary>
    /// O endpoint onde os pedidos estão localizados.
    /// </summary>
    public static string Template => "/orders";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    /// <summary>
    /// Uma referência a um método que salva novos pedidos no sistema.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "CPFPolicy")]
    private static async Task<IResult> Action(OrderRequest request, HttpContext httpContext, ApplicationDbContext context)
    {
        if (request.ProductIds is null) return Results.Problem(title: "Bad request", detail: "The body of the request is missing the product ids.", statusCode: 400);

        var clientId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var products = context.Products
            .Where(p => request.ProductIds.Contains(p.Id))
            .ToList();

        var order = new Order(clientId, products, request.DeliveryAddress);

        if (!order.IsValid) return Results.ValidationProblem(order.Notifications.ConvertToProblemDetails());

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return Results.Created(Template + $"/{order.Id}", order.Id);
    }
}
