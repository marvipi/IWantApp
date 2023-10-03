using Flunt.Notifications;
using Flunt.Validations;

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
    /// <param name="createdBy"> O identificador do usuário que a criou. </param>
    public Entity(string createdBy)
    {
        Id = Guid.NewGuid();
        CreatedBy = createdBy;
        CreatedOn = DateTime.Now;

        Validate();
    }

    /// <summary>
    /// Atualiza o nome desta entidade.
    /// </summary>
    /// <param name="modifiedBy"> O identificador do usuário que está atualizando esta entidade. </param>
    protected void Update(string modifiedBy)
    {
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.Now;
    }

    private void Validate()
    {
        var contract = new Contract<Entity>()
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
            .IsNotNull(CreatedOn, "CreatedOn");

        AddNotifications(contract);
    }
}
