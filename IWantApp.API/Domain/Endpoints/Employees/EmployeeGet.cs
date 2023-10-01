namespace IWantApp.API.Domain.Endpoints.Employees;

/// <summary>
/// Representa uma requisição HTTP que busca um empregado específico.
/// </summary>
public static class EmployeeGet
{
    /// <summary>
    /// O endpoint onde o empregado está localizado.
    /// </summary>
    public static string Template => "/employees/{id}";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    /// <summary>
    /// Uma referência a um método que busca empregados.
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
