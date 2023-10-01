using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace IWantApp.API.Domain.Endpoints.Security;

/// <summary>
/// Representa uma requisição HTTP que gera um novo token JWT.
/// </summary>
public class TokenPost
{
    /// <summary>
    /// O endpoint onde tokens são gerados.
    /// </summary>
    public static string Template => "/token";

    /// <summary>
    /// Os métodos HTTP usadosp para invocar esta requisição
    /// </summary>
    public static string[] Methods => new string[] { HttpMethods.Post.ToString() };

    /// <summary>
    /// Uma referência a um método que gera um novo token JWT.
    /// </summary>
    public static Delegate Handler => Action;

    [AllowAnonymous]
    private static async Task<IResult> Action(LoginRequest request, UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null) return Results.BadRequest();
        if (!await userManager.CheckPasswordAsync(user, request.Password)) return Results.BadRequest();

        var secretKey = configuration["JwtTokenSettings:SecretKey"];
        if (secretKey is null) return Results.Problem();

        var claims = await userManager.GetClaimsAsync(user);
        var subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            });

        subject.AddClaims(claims);

        var securityKey = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtTokenSettings:Audience"],
            Issuer = configuration["JwtTokenSettings:Issuer"],
            Expires = DateTime.UtcNow.AddSeconds(Convert.ToDouble(configuration["JwtTokenSettings:ExpiryTimeInSeconds"]))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Results.Ok(new { token = tokenHandler.WriteToken(token) });
    }
}
