namespace IWantApp.API.Domain.Endpoints.Products;

/// <summary>
/// Representa uam requisição HTTP que os busca os produtos mais vendidos.
/// </summary>
public class ProductGetMostOrdered
{
    /// <summary>
    /// O endpoint onde os produtos estão localizados.
    /// </summary>
    public static string Template => "/products/mostordered";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    /// <summary>
    /// Uma referência a um método que busca produtos.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action(QueryMostOrderedProducts query)
    {
        var mostOrderedProducts = await query.Execute();
        return Results.Ok(mostOrderedProducts);
    }
}
