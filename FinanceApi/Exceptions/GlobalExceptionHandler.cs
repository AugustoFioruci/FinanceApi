using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace FinanceApi.Exceptions
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Exceção não tratada: {Message}", exception.Message);

            var (statusCode, title, detail) = exception switch
            {
                AccountNotFoundException ex => ((int)HttpStatusCode.NotFound, "Conta não encontrada", ex.Message),
                UnauthorizedAccountAccessException ex => ((int)HttpStatusCode.Forbidden, "Acesso negado", ex.Message),
                InsufficientFundsException ex => ((int)HttpStatusCode.BadRequest, "Saldo insuficiente", ex.Message),
                InvalidOperationAmountException ex => ((int)HttpStatusCode.BadRequest, "Operação inválida", ex.Message),
                DuplicateAccountNameException ex => ((int)HttpStatusCode.Conflict, "Nome duplicado", ex.Message),
                ArgumentException ex => ((int)HttpStatusCode.BadRequest, "Argumento inválido", ex.Message),
                KeyNotFoundException ex => ((int)HttpStatusCode.NotFound, "Recurso não encontrado", ex.Message),
                UnauthorizedAccessException ex => ((int)HttpStatusCode.Unauthorized, "Não autenticado", ex.Message),
                InvalidCredentialsException ex => ((int)HttpStatusCode.BadRequest, "Credenciais inválidas", ex.Message),

                _ => ((int)HttpStatusCode.InternalServerError, "Erro interno do servidor", "Ocorreu um erro inesperado. Tente novamente mais tarde.")
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            var json = JsonSerializer.Serialize(problemDetails);
            await httpContext.Response.WriteAsync(json, cancellationToken);

            return true; // exceção tratada
        }
            
    }
}
