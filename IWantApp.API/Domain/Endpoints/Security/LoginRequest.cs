namespace IWantApp.API.Domain.Endpoints.Security;

/// <summary>
/// Representa o corpo de uma requisição HTTP que contém informações de login de um usuário.
/// </summary>
/// <param name="Email"> O e-mail do usuário. </param>
/// <param name="Password"> A senha do usuário.</param>
public record LoginRequest(string Email, string Password);
