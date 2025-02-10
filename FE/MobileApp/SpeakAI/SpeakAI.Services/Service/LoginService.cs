using SpeakAI.Services.DTO;
using SpeakAI.Services.Interfaces;
using SpeakAI.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SpeakAI.Services.Service
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;
        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(loginRequestDTO);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/auth/sign-in", content);
                if (response == null)
                    return new LoginResponseDTO { StatusCode = -1, Message = "No response from server", IsSuccess = false };
                var responseData = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseData))
                    return new LoginResponseDTO { StatusCode = -1, Message = "Empty response from server", IsSuccess = false };
                var result = JsonSerializer.Deserialize<LoginResponseDTO>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (result != null && result.IsSuccess && result.Result != null)
                {
                    string accessToken = result.Result.AccessToken;
                    SaveToken(accessToken);
                }
                return result ?? new LoginResponseDTO { StatusCode = -1, Message = "Invalid response from server", IsSuccess = false };
            }
            catch (Exception ex)
            {
                return new LoginResponseDTO { StatusCode = -1, Message = $"Error: {ex.Message}", IsSuccess = false };
            }
        }
        private void SaveToken(string token)
        {
            SecureStorage.SetAsync("AccessToken", token);
        }

    }
}
