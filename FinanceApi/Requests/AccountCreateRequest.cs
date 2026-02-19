namespace FinanceApi.Requests
{
    public class AccountCreateRequest
    {
        public string Name { get; set; } = String.Empty;
        public decimal InicialBalance { get; set; }
    }
}
