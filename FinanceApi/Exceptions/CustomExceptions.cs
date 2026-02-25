namespace FinanceApi.Exceptions
{
    // Conta não encontrada
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException() : base("Conta não encontrada.") { }
        public AccountNotFoundException(string message) : base(message) { }
        public AccountNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    // Acesso negado a conta de outro usuário
    public class UnauthorizedAccountAccessException : Exception
    {
        public UnauthorizedAccountAccessException() : base("Você não tem permissão para acessar esta conta.") { }
        public UnauthorizedAccountAccessException(string message) : base(message) { }
    }

    // Saldo insuficiente
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base("Saldo insuficiente.") { }
        public InsufficientFundsException(decimal currentBalance, decimal requestedAmount)
            : base($"Saldo insuficiente. Saldo atual: {currentBalance:C}, valor solicitado: {requestedAmount:C}") { }
    }

    // Nome de conta duplicado
    public class DuplicateAccountNameException : Exception
    {
        public DuplicateAccountNameException(string name)
            : base($"Você já possui uma conta chamada '{name}'.") { }
    }

    // Valor inválido para operações financeiras
    public class InvalidOperationAmountException : Exception
    {
        public InvalidOperationAmountException() : base("O valor da operação deve ser positivo.") { }
    }

    // Email ou nome de usuário duplicado (se quiser usar no UserService também)
    public class DuplicateEmailException : Exception
    {
        public DuplicateEmailException() : base("Este email já está cadastrado.") { }
    }

    // Email ou senha inválidos para autenticação

    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("Email ou senha inválidos.") { }
    }
}
