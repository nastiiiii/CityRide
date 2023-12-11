using CityRide.DriverConsoleApp.Client.Requests;
using CityRide.DriverConsoleApp.Client.Responses;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CityRide.DriverConsoleApp.Client.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetTokenAsync(LoginRequest loginRequest)
        {
            var content = JsonContent.Create(loginRequest);
            string token = string.Empty;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(_configuration["LoginUrl"], content);
                    if (response.IsSuccessStatusCode)
                    {
                        var deserializeObject = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());
                        token = deserializeObject.JwtToken;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Authorization connection failed.");
            }
            return token;
        }
    }
}
