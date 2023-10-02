namespace IWantApp.API.Domain.Services;

/// <summary>
/// Representa um microserviço responsável pela criação dos usuários.
/// </summary>
public class UserCreator
{
    private readonly UserManager<IdentityUser> userManager;

    public UserCreator(UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
    }

    /// <summary>
    /// Cria um novo usuário.
    /// </summary>
    /// <param name="email"> O e-mail do novo usuário. </param>
    /// <param name="password"> A senha do novo usuário. </param>
    /// <param name="userClaims"> As claims que serão atribuidas ao novo usuário. </param>
    /// <returns>
    ///     Um par cujo primeiro valor é o resultado da criação e o segundo é o id atribuido ao novo usuário.
    ///     O id será uma string vazia se a criação falhar.
    /// </returns>
    public async Task<(IdentityResult, string userId)> Create(string email, string password, List<Claim> userClaims)
    {
        var newUser = new IdentityUser { UserName = email, Email = email };
        var result = await userManager.CreateAsync(newUser, password);

        if (!result.Succeeded) return (result, string.Empty);

        return (await userManager.AddClaimsAsync(newUser, userClaims), newUser.Id);
    }
}
