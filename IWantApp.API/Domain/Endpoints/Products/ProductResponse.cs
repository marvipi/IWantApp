namespace IWantApp.API.Domain.Endpoints.Products;

/// <summary>
/// Representa o corpo de uma resposta HTTP que cont�m dados sobre um produto.
/// </summary>
/// <param name="Name"> O nome do produto. </param>
/// <param name="Description"> Uma breve descri��o do produto. </param>
/// <param name="CategoryName"> O nome da categoria � qual o produto pertence. </param>
/// <param name="HasStock"> Indica se o produto est� em estoque. </param>
/// <param name="Active"> Indica se o produto est� ativo no website. </param>
public record ProductResponse(string Name, string? Description, string CategoryName, bool HasStock, bool Active);
