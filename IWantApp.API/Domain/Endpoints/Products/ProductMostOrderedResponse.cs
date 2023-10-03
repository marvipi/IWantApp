namespace IWantApp.API.Domain.Endpoints.Products;

/// <summary>
/// Representa uma resposta HTTP que contém dados sobre o produto mais vendido.
/// </summary>
/// <param name="Id"> O id do produto mais vendido. </param>
/// <param name="Name"> O nome do produto mais vendido. </param>
/// <param name="Amount"> A quantidade de vezes que o produto foi vendido. </param>
public record ProductMostOrderedResponse(Guid Id, string Name, int Amount);
