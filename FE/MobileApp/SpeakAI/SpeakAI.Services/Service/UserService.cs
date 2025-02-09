using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SpeakAI.Services.DTO;
using SpeakAI.Services.Interfaces;
using SpeakAI.Services.Models;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseDTO> SignUpCustomer(UserDTO userDTO)
    {
        try
        {
            var jsonContent = JsonSerializer.Serialize(userDTO);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/auth/sign-up-customer", content);

            // Ensure response is not null
            if (response == null)
                return new ResponseDTO { StatusCode = -1, Message = "No response from server", IsSuccess = false };

            var responseData = await response.Content.ReadAsStringAsync();

            // Ensure response content is not empty
            if (string.IsNullOrWhiteSpace(responseData))
                return new ResponseDTO { StatusCode = -1, Message = "Empty response from server", IsSuccess = false };

            // Deserialize response safely
            var result = JsonSerializer.Deserialize<ResponseDTO>(responseData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? new ResponseDTO { StatusCode = -1, Message = "Invalid response from server", IsSuccess = false };
        }
        catch (Exception ex)
        {
            return new ResponseDTO { StatusCode = -1, Message = $"Error: {ex.Message}", IsSuccess = false };
        }
    }
}
