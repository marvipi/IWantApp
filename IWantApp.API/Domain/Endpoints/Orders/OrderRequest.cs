namespace IWantApp.API.Domain.Endpoints.Orders;

/// <summary>
/// Representa uma requisição HTTP que contém dados sobre um pedido feito por um cliente.
/// </summary>
/// <param name="ProductIds"> Os identificadores únicos de todos os produtos comprados no pedido. </param>
/// <param name="DeliveryAddress"> O endereço onde os produtos serão entregues. </param>
public record OrderRequest(List<Guid> ProductIds, string DeliveryAddress);
