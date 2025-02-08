namespace Common.DTO
{
    public class LoginResponseDTO
    {
        public LocalUserDTO? User { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
