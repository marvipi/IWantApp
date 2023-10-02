namespace IWantApp.API.Domain.Endpoints.Products;

/// <summary>
/// Representa o corpo de uma resposta HTTP que contém dados sobre um produto.
/// </summary>
/// <param name="Name"> O nome do produto. </param>
/// <param name="Description"> Uma breve descrição do produto. </param>
/// <param name="CategoryName"> O nome da categoria à qual o produto pertence. </param>
/// <param name="HasStock"> Indica se o produto está em estoque. </param>
/// <param name="Price"> O preço do produto. </param>
public record ProductResponse(string Name, string? Description, string CategoryName, bool HasStock, decimal Price);
