namespace FinanceApp.Application.Exceptions;

public abstract class NotFoundException(string name, object key)
    : ApplicationException($"Entity \"{name}\" ({key}) not found ");