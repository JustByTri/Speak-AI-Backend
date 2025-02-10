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
        private readonly HttpService _httpService;
        public LoginService(HttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var result = await _httpService.PostAsync<LoginRequestDTO, LoginResponseDTO>("api/auth/sign-in", loginRequestDTO);

            if (result != null && result.IsSuccess && result.Result != null)
            {
                await SecureStorage.SetAsync("AccessToken", result.Result.AccessToken);
            }

            return result;
        }
    }
}
