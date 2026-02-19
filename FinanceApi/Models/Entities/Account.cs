namespace FinanceApi.Models.Entities
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public String Name { get; set; } = String.Empty;
        public decimal Balance { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
}
