namespace IWantApp.API.Domain.Endpoints.Orders;

/// <summary>
/// Representa uma requisição HTTP usada para buscar um pedido específico.
/// </summary>
public static class OrderGet
{
    /// <summary>
    /// O endpoint onde o pedido está localizado.
    /// </summary>
    public static string Template => "/orders/{id:guid}";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    /// <summary>
    /// Uma referência a um método que busca pedidos registrados no sistema.
    /// </summary>
    public static Delegate Handler => Action;

    private static async Task<IResult> Action([FromRoute] Guid id, HttpContext httpContext, ApplicationDbContext context)
    {
        var order = await context.Orders.FindAsync(id);
        if (order is null) return Results.NotFound();

        var orderProducts = order.Products
            .Select(p => new OrderProduct(p.Id, p.Name))
            .ToList();

        var response = new OrderResponse(orderProducts, order.DeliveryAddress, order.Total);
        var user = httpContext.User;
        var employeeClaim = user.FindFirst("EmployeeCode");

        IResult result;
        if (employeeClaim is not null)
        {
            result = Results.Ok(response);
        }
        else
        {
            var userId = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            result = order.ClientId == userId ? Results.Ok(response) : Results.NotFound();
        }

        return result;
    }
}
