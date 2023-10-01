namespace IWantApp.API.Domain.Products;

/// <summary>
/// Representa uma categoria à qual um produto pode pertencer.
/// </summary>
public class Category : Entity
{
    /// <summary>
    /// Indica se esta categoria pode ser usada para marcar produtos.
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    /// Produz uma nova categoria.
    /// </summary>
    /// <param name="name"> O nome da nova categoria. </param>
    /// <param name="createdBy"> O identificador do usuário que a criou. </param>
    public Category(string name, string createdBy) : base(name, createdBy)
    {
        Active = true;
    }

    /// <summary>
    /// Atualiza esta categoria.
    /// </summary>
    /// <param name="name"> O novo nome para esta categoria. </param>
    /// <param name="active"> Indica se esta categoria pode ser usada para marcar produtos. </param>
    /// <param name="modifiedBy"> O identificador do usuário que esta modificando esta categoria. </param>
    public void Update(string name, bool active, string modifiedBy)
    {
        Active = active;
        base.Update(name, modifiedBy);
    }
}
