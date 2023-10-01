namespace IWantApp.API.Domain.Endpoints.Employees;

/// <summary>
/// Representa uma requisição HTTP que remove um empregado do sistema.
/// </summary>
public static class EmployeeDelete
{
    /// <summary>
    /// O endpoint onde o empregado está registrado.
    /// </summary>
    public static string Template => "/employees/{id}";

    /// <summary>
    /// Os métodos HTTPs usados para invocar esta requisiçào.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };

    /// <summary>
    /// Uma referência a um método que remove empregados do sistema.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action([FromRoute] string id, UserManager<IdentityUser> userManager)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return Results.NotFound();
        }
        else
        {
            await userManager.DeleteAsync(user);
            return Results.NoContent();
        }
    }
}
