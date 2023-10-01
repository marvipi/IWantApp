namespace IWantApp.API.Domain.Products;

/// <summary>
/// Representa um produto vendido no website.
/// </summary>
public class Product : Entity
{
    /// <summary>
    /// A categoria à qual o produto pertence.
    /// </summary>
    public Category Category { get; private set; }

    /// <summary>
    /// Indica se o produto está em estoque.
    /// </summary>
    public bool HasStock { get; private set; }

    /// <summary>
    /// Indica se o produto pode ser listado no website.
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    /// Uma breve descrição do produto.
    /// </summary>
    public string? Description { get; private set; }

    // Usado para que o entity framework possa mapear esta entidade no banco de dados.
    private Product(string name, string createdBy) : base(name, createdBy) { }

    /// <summary>
    /// Instancia uma novo produto.
    /// </summary>
    /// <param name="name"> O nome do novo produto. </param>
    /// <param name="category"> A categoria à qual o novo produto pertence. </param>
    /// <param name="hasStock"> Indica se o novo produto está em estoque. </param>
    /// <param name="createdBy"> O identificado do usuário que registrou o novo produto. </param>
    /// <param name="description"> Uma breve descrição do novo produto. </param>
    public Product(string name, Category category, bool hasStock, string createdBy, string? description) : base(name, createdBy)
    {
        Category = category;
        HasStock = hasStock;
        Active = true;
        Description = description;

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
    public void Update(string name, Category category, bool hasStock, string modifiedBy, string? description)
    {
        base.Update(name, modifiedBy);
        Category = category;
        HasStock = hasStock;
        Description = description;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Product>()
            .IsNotNull(Category, "Category")
            .IsNotNull(HasStock, "HasStock");

        AddNotifications(contract);
    }
}
