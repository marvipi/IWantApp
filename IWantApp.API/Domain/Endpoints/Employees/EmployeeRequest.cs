namespace IWantApp.API.Domain.Endpoints.Employees;

/// <summary>
/// Representa o corpo de uma requisição HTTP que contém informaçõe sobre um empregado.
/// </summary>
/// <param name="Email"> O e-mail do empregado. </param>
/// <param name="Password"> A senha que o empregado usa para logar no sistema. </param>
/// <param name="Name"> O nome do empregado. </param>
/// <param name="EmployeeCode"> Um identificador do empregado. </param>
public record EmployeeRequest(string Email, string Password, string Name, string EmployeeCode);
