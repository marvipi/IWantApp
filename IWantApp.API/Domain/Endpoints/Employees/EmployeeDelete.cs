namespace IWantApp.API.Domain.Endpoints.Employees;

/// <summary>
/// Representa uma requisi��o HTTP que remove um empregado do sistema.
/// </summary>
public static class EmployeeDelete
{
    /// <summary>
    /// O endpoint onde o empregado est� registrado.
    /// </summary>
    public static string Template => "/employees/{id}";

    /// <summary>
    /// Os m�todos HTTPs usados para invocar esta requisi��o.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };

    /// <summary>
    /// Uma refer�ncia a um m�todo que remove empregados do sistema.
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
