namespace IWantApp.API.Domain.Endpoints.Orders;

/// <summary>
/// Representa uma requisição HTTP que contém informações sobre um pedido feito por um usuário.
/// </summary>
/// <param name="Products"> Os identificadores únicos e nomes de todos os produtos comprados no pedido. </param>
/// <param name="DeliveryAddres"> O endereço de entrega salvo no pedido. </param>
/// <param name="Total"> O valor total do pedido. </param>
public record OrderResponse(List<OrderProduct> Products, string DeliveryAddres, decimal Total);
