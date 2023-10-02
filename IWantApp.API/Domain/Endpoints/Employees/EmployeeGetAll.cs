namespace IWantApp.API.Domain.Endpoints.Employees;

/// <summary>
/// Representa uma requisi��o HTTP que busca todos os empregados registrados no sistema.
/// </summary>
public static class EmployeeGetAll
{
    /// <summary>
    /// O endpoint onde os empregados est�o localizados.
    /// </summary>
    public static string Template => "/employees";

    /// <summary>
    /// Os m�todos HTTP usados para invocar esta requisi��o.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    /// <summary>
    /// Uma refer�ncia a um m�todo que busca empregados.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action([FromQuery] int? page, [FromQuery] int? rows, QueryAllUsersWithClaimName query)
    {
        if (page is null || page < 1) page = 1;
        if (rows is null || rows > 10) rows = 10;

        return Results.Ok(await query.ExecuteAsync(page.Value, rows.Value));
    }
}
