namespace DribionPayAPI.DTO
{
    public class LoginResultDTO
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public int expiresIn { get; set; }
        public int expiresRefreshTokenIn { get; set; }
        public string role { get; set; }
        public bool politicaPrivacidade { get; set; }
        public bool admin { get; set; }
        public string empresaKey { get; set; }
    }
}
