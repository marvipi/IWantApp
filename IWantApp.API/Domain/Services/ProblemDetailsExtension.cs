namespace IWantApp.API.Domain.Services;

public static class ProblemDetailsExtension
{
    /// <summary>
    /// Converte uma coleção de notificações do Flunt para um dicionário de string arrays.
    /// </summary>
    /// <param name="notifications"> A coleção de notificações que será convertida. </param>
    /// <returns> Um dicionário cujos valores correspondem às mensagens das notificações. </returns>
    public static Dictionary<string, string[]> ConvertToProblemDetails(this IReadOnlyCollection<Notification> notifications)
    {
        return notifications
            .GroupBy(notifications => notifications.Key)
            .ToDictionary(notifications => notifications.Key,
                          notifications => notifications.Select(notification => notification.Message).ToArray());
    }

    /// <summary>
    /// Converte um enumerável de erros de identidade para um dicionário de string arrays.
    /// </summary>
    /// <param name="errors"> Os erros de identidade que serão convertidos. </param>
    /// <returns> Um dicionário que contém uma única chave e cujo valor é uma array de mensagens de erro. </returns>
    public static Dictionary<string, string[]> ConvertToProblemDetails(this IEnumerable<IdentityError> errors)
    {
        var errorMsgs = new Dictionary<string, string[]>()
        {
            { "Error", errors.Select(error => error.Description).ToArray() }
        };
        return errorMsgs;
    }
}
