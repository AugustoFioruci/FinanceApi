namespace FinanceApi.Requests
{
    public class RegisterRequest
    {
        public string AccountName { get; set; } = "Principal";
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public decimal InicialBalance { get; set; }
    }
}
