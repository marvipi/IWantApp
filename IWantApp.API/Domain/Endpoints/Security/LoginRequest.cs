namespace IWantApp.API.Domain.Endpoints.Security;

/// <summary>
/// Representa o corpo de uma requisi��o HTTP que cont�m informa��es de login de um usu�rio.
/// </summary>
/// <param name="Email"> O e-mail do usu�rio. </param>
/// <param name="Password"> A senha do usu�rio.</param>
public record LoginRequest(string Email, string Password);
