namespace IWantApp.API.Domain.Endpoints.Orders;

/// <summary> Representa o identificado único e o nome de um produto. </summary>
/// <param name="ProductId"> O identificador único do produto. </param>
/// <param name="ProductName"> O nome do produto. </param>
public record OrderProduct(Guid ProductId, string ProductName);
