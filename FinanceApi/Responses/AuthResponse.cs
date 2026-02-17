namespace FinanceApi.Responses
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; }
    }
}
