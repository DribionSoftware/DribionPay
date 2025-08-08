using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

class Program
{
    private static HttpClient client = new HttpClient();
    static async Task Main(string[] args)
    {
        var cultureInfo = new CultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        Console.WriteLine("Iniciando a geração do token para o cartão...");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = await Loadkey();

        var enc = new EncryptingCredentials(key, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes128CbcHmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "DribionPay",
            Audience = "DribionPay",
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim("CardNumber", "5458382364609501"),
                    new Claim("Holder", "Marcos Accept"),
                    new Claim("ExpirationDate", "05/2025"), //MM/yyyy
                    new Claim("SecurityCode", "157")

            }),
            EncryptingCredentials = enc
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        Console.WriteLine($"Key: {enc.Key.KeyId}");
        Console.WriteLine($"Token:\n{tokenHandler.WriteToken(token)}");

        Console.WriteLine("Cartão, critografado com sucesso!");
    }

    private static async Task<SecurityKey> Loadkey()
    {
        var publicKey = await client.GetStringAsync("https://api.dribionpay.com.br/jwks");
        var key = JsonWebKeySet.Create(publicKey);
        return key.Keys.FirstOrDefault();
    }
}


