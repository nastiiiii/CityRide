using System.Net.Http.Json;
using ClientConsoleApp.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ClientConsoleApp.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private static Task<string> TokenDeserializer(string token)
    {
        return Task.FromResult(JsonConvert.DeserializeObject<TokenModel>(token)!.JWTToken);
    }

    public async Task<string> GetTokenAsync(LogInCredentials loginRequest)
    {
        var content = JsonContent.Create(loginRequest);
        var token = string.Empty;
        try
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(_configuration["LoginUrl"], content);
                if (response.IsSuccessStatusCode)
                    token = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine("Authorization connection failed.");
        }

        return await TokenDeserializer(token);
    }
}