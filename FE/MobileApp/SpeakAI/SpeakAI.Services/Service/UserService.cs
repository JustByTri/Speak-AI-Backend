using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SpeakAI.Services.DTO;
using SpeakAI.Services.Interfaces;
using SpeakAI.Services.Models;
using SpeakAI.Services.Service;

public class UserService : IUserService
{
    private readonly HttpService _httpService;
    public UserService(HttpService httpService)
    {
        _httpService = httpService;
    }
    public async Task<ResponseDTO> SignUpCustomer(UserDTO userDTO)
    {
        return await _httpService.PostAsync<UserDTO, ResponseDTO>("api/auth/sign-up-customer", userDTO);
    }
}
