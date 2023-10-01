namespace IWantApp.API.Domain.Endpoints.Categories;

/// <summary>
/// Representa o corpo de uma resposta HTTP que contém dados sobre uma categoria de produto.
/// </summary>
/// <param name="Name"> O nome da categoria. </param>
/// <param name="Active"> Indica se a categoria pode ser usada para marcar produtos. </param>
public record CategoryResponse(string Name, bool Active);
