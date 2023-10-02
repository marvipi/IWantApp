namespace IWantApp.API.Domain.Endpoints.Clients;

/// <summary>
/// Representa o corpo de uma requisição HTTP que contém dados de um cliente do site.
/// </summary>
/// <param name="Email"> O e-mail do cliente. </param>
/// <param name="Password"> A senha do cliente. </param>
/// <param name="Name"> O nome do cliente. </param>
/// <param name="CPF"> O CPF do cliente. </param>
public record ClientRequest(string Email, string Password, string Name, string CPF);
