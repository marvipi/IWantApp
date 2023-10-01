namespace IWantApp.API.Domain.Endpoints.Categories;

/// <summary>
/// Representa o corpo de uma requisição HTTP que contém dados sobre as categorias de produto.
/// </summary>
/// <param name="Name"> O nome da categoria. </param>
/// <param name="Active"> Indica se a categoria pode ser usada para marcar produtos. </param>
public record CategoryRequest(string Name, bool Active);
