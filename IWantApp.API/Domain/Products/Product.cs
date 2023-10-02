namespace IWantApp.API.Domain.Products;

/// <summary>
/// Representa um produto vendido no website.
/// </summary>
public class Product : Entity
{
    /// <summary>
    /// O nome deste produto.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// A categoria à qual o produto pertence.
    /// </summary>
    public Category Category { get; private set; }

    /// <summary>
    /// Indica se o produto está em estoque.
    /// </summary>
    public bool HasStock { get; private set; }

    /// <summary>
    /// Uma breve descrição do produto.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// O preço do produto no intervalo de R$1,00 a R$9.999.999.999,99.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Todas os pedidos aos quais este produto pertence.
    /// </summary>
    public ICollection<Order> Orders { get; private set; }

    // Usado para que o entity framework possa mapear esta entidade no banco de dados.
    private Product(string createdBy) : base(createdBy) { }

    /// <summary>
    /// Instancia uma novo produto.
    /// </summary>
    /// <param name="name"> O nome do novo produto. </param>
    /// <param name="category"> A categoria à qual o novo produto pertence. </param>
    /// <param name="hasStock"> Indica se o novo produto está em estoque. </param>
    /// <param name="createdBy"> O identificado do usuário que registrou o novo produto. </param>
    /// <param name="description"> Uma breve descrição do novo produto. </param>
    /// <param name="price"> O preço do novo produto. </param>
    public Product(string name, Category category, bool hasStock, string createdBy, string? description, decimal price) : base(createdBy)
    {
        Name = name;
        Category = category;
        HasStock = hasStock;
        Description = description;
        Price = price;

        Validate();
    }

    /// <summary>
    /// Atualiza as informações sobre este produto.
    /// </summary>
    /// <param name="name"> O novo nome deste produto. </param>
    /// <param name="category"> A nova categoria à qual este produto pertence. </param>
    /// <param name="hasStock"> Indica se este produto esta em estoque. </param>
    /// <param name="modifiedBy"> O identificador do usuário que esta modificando este produto. </param>
    /// <param name="description"> Uma nova descrição para este produto. </param>
    /// <param name="price"> O novo preço deste produto. </param>
    public void Update(string name, Category category, bool hasStock, string modifiedBy, string? description, decimal price)
    {
        base.Update(modifiedBy);
        Name = name;
        Category = category;
        HasStock = hasStock;
        Description = description;
        Price = price;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Product>()
            .IsNotNullOrEmpty(Name, "Name")
            .IsGreaterOrEqualsThan(Name, 3, "Name")
            .IsNotNull(Category, "Category", "Category not found")
            .IsNotNull(HasStock, "HasStock")
            .IsNotNull(Price, "Price")
            .IsGreaterOrEqualsThan(Price, 1, "Price")
            .IsLowerThan(Price, 10000000000, "Price");

        AddNotifications(contract);
    }
}
