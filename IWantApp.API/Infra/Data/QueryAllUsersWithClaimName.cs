using Dapper;
using IWantApp.API.Domain.Endpoints.Employees;
using Microsoft.Data.SqlClient;

namespace IWantApp.API.Infra.Data;

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

    public async Task<IEnumerable<EmployeeResponse>> ExecuteAsync(int page, int rows)
    {
        var db = new SqlConnection(configuration["ConnectionStrings:IWantDb"]);
        return await db.QueryAsync<EmployeeResponse>(USER_QUERY, new { page, rows, type = CLAIM_TYPE });
    }
}
