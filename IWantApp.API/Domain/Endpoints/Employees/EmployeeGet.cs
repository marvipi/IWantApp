namespace IWantApp.API.Domain.Endpoints.Employees;

/// <summary>
/// Representa uma requisi��o HTTP que busca um empregado espec�fico.
/// </summary>
public static class EmployeeGet
{
    /// <summary>
    /// O endpoint onde o empregado est� localizado.
    /// </summary>
    public static string Template => "/employees/{id}";

    /// <summary>
    /// Os m�todos HTTP usados para invocar esta requisi��o.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    /// <summary>
    /// Uma refer�ncia a um m�todo que busca empregados.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action([FromRoute] string id, UserManager<IdentityUser> userManager)
    {
        var employee = await userManager.FindByIdAsync(id);
        if (employee is null) return Results.NotFound();

        var employeeClaims = await userManager.GetClaimsAsync(employee);
        var employeeName = employeeClaims.First(c => c.Type == ClaimTypes.Name).Value;

        var result = employee is null ? Results.NotFound() : Results.Ok(new EmployeeResponse(employee.Email, employeeName));
        return result;
    }
}
