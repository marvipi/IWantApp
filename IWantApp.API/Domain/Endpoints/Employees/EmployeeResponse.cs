namespace IWantApp.API.Domain.Endpoints.Employees;

/// <summary>
/// Representa o corpo de uma resposta HTTP que contém dados sobre um empregado.
/// </summary>
/// <param name="Email"> O e-mail do empregado. </param>
/// <param name="Name"> O nome do empregado. </param>
public record EmployeeResponse(string Email, string Name);
