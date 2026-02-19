namespace FinanceApi.Responses
{
    public class AccountResponse
    {
        public Guid AccountID { get; set; }
        public String Name { get; set; } = String.Empty;
        public decimal Balance { get; set; }
    }
}
