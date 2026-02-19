using FinanceApi.Models.Enums;

namespace FinanceApi.Requests
{
    public class UserUpdateRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
