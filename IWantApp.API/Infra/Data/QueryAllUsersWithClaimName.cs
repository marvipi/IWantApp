using Dapper;
using Microsoft.Data.SqlClient;

namespace IWantApp.API.Infra.Data;

/// <summary>
/// Representa uma consulta que retorna o e-mail e o nome dos usuários.
/// </summary>
public class QueryAllUsersWithClaimName
{
    private readonly IConfiguration configuration;

    private const string CLAIM_TYPE = ClaimTypes.Name;

    private const string USER_QUERY = @"
            SELECT u.Email, c.ClaimValue as Name
            FROM AspNetUsers u INNER JOIN AspNetUserClaims c
            ON u.Id = c.UserId AND c.ClaimType = @type
            ORDER BY Name
            OFFSET (@page - 1) * @rows ROWS FETCH NEXT @rows ROWS ONLY;
            ";

    public QueryAllUsersWithClaimName(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// Produz o e-mail e nome de uma quantidade paginada de usuários.
    /// </summary>
    /// <param name="page"> O número da página. </param>
    /// <param name="rows"> A quantidade de elementos que a consulta deve retornar. </param>
    /// <returns> Uma coleção enumerável que contém um número de <see cref="EmployeeResponse"/> igual ou menor a <paramref name="rows"/> </returns>
    public async Task<IEnumerable<EmployeeResponse>> ExecuteAsync(int page, int rows)
    {
        var db = new SqlConnection(configuration["ConnectionStrings:IWantDb"]);
        return await db.QueryAsync<EmployeeResponse>(USER_QUERY, new { page, rows, type = CLAIM_TYPE });
    }
}
