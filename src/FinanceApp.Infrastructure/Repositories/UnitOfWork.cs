using System.Collections;
using FinanceApp.Application.Interfaces.Repositories;
using FinanceApp.Infrastructure.Data;

namespace FinanceApp.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private Hashtable? _repositories;

    public async Task<int> Complete()
    {
        try 
        {
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception("Error transaction", e);
        }
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories is null)
        {
            _repositories = new Hashtable();
        }
        
        var type = typeof(TEntity).Name;
        if(!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)));
            _repositories.Add(type, repositoryInstance);
        }
        return (IGenericRepository<TEntity>)_repositories[type]!;
    }
}