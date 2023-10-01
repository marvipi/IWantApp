namespace IWantApp.API.Domain.Products;

/// <summary>
/// Representa um elemento que pode ser armazenado em um banco de dados.
/// </summary>
public abstract class Entity : Notifiable<Notification>
{
    /// <summary>
    /// Um identificador único para esta entidade.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// O nome desta entidade.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// O identificador do usuário que criou esta entidade.
    /// </summary>
    public string CreatedBy { get; private set; }

    /// <summary>
    /// A data e hora na qual esta entidade foi criada.
    /// </summary>
    public DateTime CreatedOn { get; private set; }

    /// <summary>
    /// O identificador do último usuário que modificou esta entidade, ou nulo, se ela nunca foi modificada.
    /// </summary>
    public string? ModifiedBy { get; private set; }

    /// <summary>
    /// A data e hora da última vez que esta entidade foi modificada, ou nulo, se ela nunca foi modificada.
    /// </summary>
    public DateTime? ModifiedOn { get; private set; }

    /// <summary>
    /// Produz uma nova entidade.
    /// </summary>
    /// <param name="name"> O nome da nova entidade. </param>
    /// <param name="createdBy"> O identificador do usuário que a criou. </param>
    public Entity(string name, string createdBy)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreatedBy = createdBy;
        CreatedOn = DateTime.Now;

        Validate();
    }

    /// <summary>
    /// Atualiza o nome desta entidade.
    /// </summary>
    /// <param name="name"> O novo nome que esta entidade terá. </param>
    /// <param name="modifiedBy"> O identificador do usuário que está atualizando esta entidade. </param>
    protected void Update(string name, string modifiedBy)
    {
        Name = name;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Entity>()
                    .IsNotNullOrEmpty(Name, "Name")
                    .IsGreaterOrEqualsThan(Name, 3, "Name")
                    .IsNotNullOrEmpty(CreatedBy, "CreatedBy");

        AddNotifications(contract);
    }
}
