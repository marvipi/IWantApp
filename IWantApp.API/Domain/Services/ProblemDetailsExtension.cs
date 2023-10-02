namespace IWantApp.API.Domain.Services;

public static class ProblemDetailsExtension
{
    /// <summary>
    /// Converte uma cole��o de notifica��es do Flunt para um dicion�rio de string arrays.
    /// </summary>
    /// <param name="notifications"> A cole��o de notifica��es que ser� convertida. </param>
    /// <returns> Um dicion�rio cujos valores correspondem �s mensagens das notifica��es. </returns>
    public static Dictionary<string, string[]> ConvertToProblemDetails(this IReadOnlyCollection<Notification> notifications)
    {
        return notifications
            .GroupBy(notifications => notifications.Key)
            .ToDictionary(notifications => notifications.Key,
                          notifications => notifications.Select(notification => notification.Message).ToArray());
    }

    /// <summary>
    /// Converte um enumer�vel de erros de identidade para um dicion�rio de string arrays.
    /// </summary>
    /// <param name="errors"> Os erros de identidade que ser�o convertidos. </param>
    /// <returns> Um dicion�rio que cont�m uma �nica chave e cujo valor � uma array de mensagens de erro. </returns>
    public static Dictionary<string, string[]> ConvertToProblemDetails(this IEnumerable<IdentityError> errors)
    {
        var errorMsgs = new Dictionary<string, string[]>()
        {
            { "Error", errors.Select(error => error.Description).ToArray() }
        };
        return errorMsgs;
    }
}
