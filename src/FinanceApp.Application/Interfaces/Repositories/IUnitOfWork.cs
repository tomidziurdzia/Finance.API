namespace FinanceApp.Application.Repositories;

public interface IUnitOfWork : IDisposable
{
    IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> Complete();
}