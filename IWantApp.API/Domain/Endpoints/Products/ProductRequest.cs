namespace IWantApp.API.Domain.Endpoints.Products;

/// <summary>
/// Representa o corpo de uma requisição HTTP que contém dados sobre um produtos.
/// </summary>
/// <param name="Name"> O nome do produto. </param>
/// <param name="CategoryId"> O identificador da categoria à qual o produto pertence. </param>
/// <param name="HasStock"> Indica se o produto está em estoque. </param>
/// <param name="Description"> Uma breve descrição do produto. </param>
public record ProductRequest(string Name, Guid CategoryId, bool HasStock, string? Description);
