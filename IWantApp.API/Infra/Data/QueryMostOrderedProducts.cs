using Dapper;
using Microsoft.Data.SqlClient;

namespace IWantApp.API.Infra.Data;

/// <summary>
/// Representa uma consulta que retorna os produtos mais vendidos.
/// </summary>
public class QueryMostOrderedProducts
{
    private readonly IConfiguration configuration;

    private const string QUERY =
        @"
            WITH SoldProducts AS
            (SELECT p.Id AS Id, p.Name AS [Name], COUNT(p.Id) as Amount
            FROM Products p 
            INNER JOIN [Orders&Products] op
            ON p.Id = op.ProductsId 
            INNER JOIN Orders o
            ON o.Id = op.OrdersId
            GROUP BY p.Id, p.Name)

            SELECT Id, [Name], Amount
            FROM SoldProducts
            GROUP BY Id, [Name], Amount
            HAVING Amount = (SELECT MAX(Amount) FROM SoldProducts);
        ";

    public QueryMostOrderedProducts(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// Produz todos os produtos mais vendidos.
    /// </summary>
    /// <returns> Uma coleção enumerável de <see cref="ProductMostOrderedResponse"/> que contém todos os produtos mais vendidos. </returns>
    public async Task<IEnumerable<ProductMostOrderedResponse>> Execute()
    {
        var db = new SqlConnection(configuration.GetConnectionString("IWantDb"));
        return await db.QueryAsync<ProductMostOrderedResponse>(QUERY);
    }
}
