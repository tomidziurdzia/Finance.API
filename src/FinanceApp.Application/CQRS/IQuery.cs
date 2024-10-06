namespace FinanceApp.Application.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>  
    where TResponse : notnull
{
}