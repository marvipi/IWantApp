using Flunt.Validations;

namespace IWantApp.API.Domain.Products;

/// <summary>
/// Representa um pedido de compra que um cliente pode fazer.
/// </summary>
public class Order : Entity
{
    /// <summary>
    /// O identificador único do cliente que fez este pedido.
    /// </summary>
    public string ClientId { get; private set; }

    /// <summary>
    /// Todos os produtos que o cliente comprou neste pedido.
    /// </summary>
    public List<Product> Products { get; private set; }

    /// <summary>
    /// A soma do preço de todos os produtos comprados neste pedido.
    /// </summary>
    public decimal Total { get; private set; }

    /// <summary>
    /// O endereço onde os produtos serão entregues.
    /// </summary>
    public string DeliveryAddress { get; private set; }

    // Impede que o EF Core mostre um erro na hora de migrar o banco de dados.
    private Order(string createdBy) : base(createdBy) { }

    /// <summary>
    /// Instancia um novo pedido.
    /// </summary>
    /// <param name="clientId"> O identificador único do cliente que fez o pedido. </param>
    /// <param name="products"> Os produtos que o cliente comprou neste pedido. </param>
    /// <param name="deliveryAddress"> O endereço onde os produtos serão entregues. </param>
    public Order(string clientId, List<Product> products, string deliveryAddress) : base(clientId)
    {
        ClientId = clientId;
        Products = products;
        DeliveryAddress = deliveryAddress;
        Total = Products.Sum(p => p.Price);

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Order>()
            .IsNotNullOrEmpty(ClientId, "ClientId")
            .IsTrue(Products is not null && Products.Any(), "Products")
            .IsNotNullOrEmpty(DeliveryAddress, "DeliveryAddress");

        AddNotifications(contract);
    }
}
