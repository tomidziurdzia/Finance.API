using FinanceApp.Application.Exceptions;
using FinanceApp.Domain.Models;
using FinanceApp.Domain.Models.Enums;
using FinanceApp.Domain.Repositories;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public async Task AddCategoriesToUser(IEnumerable<Category> categories)
    {
        try
        {
            context.Categories!.AddRange(categories);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error adding categories to user: {ex.Message}");
        }
    }

    public async Task<Category> Get(string userId, Guid? id, CancellationToken cancellationToken)
    {
        try
        {
            var category = await context.Categories!.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId, cancellationToken);
            if(category == null) throw new NotFoundException(nameof(Category), id);

            return category;
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }
    public async Task<List<Category>> GetAll(string userId, CancellationToken cancellationToken)
    {
        try
        {
            var categories = context.Categories!.Where(u => u.User!.Id == userId);

            return await categories.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task Create(Category category, CancellationToken cancellationToken)
    {
        try
        {
            await context.Categories!.AddAsync(category, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating category: {ex.Message}");
        }
    }

    public async Task Update(Category category, CancellationToken cancellationToken)
    {
        try
        {
            context.Categories!.Update(category);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating category: {ex.Message}");
        }
    }

    public async Task Delete(Category category, CancellationToken cancellationToken)
    {
        try
        {
            context.Categories!.Remove(category);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting category: {ex.Message}");
        }
    }

    public async Task<Category> GetByName(string userId, string name, CancellationToken cancellationToken)
    {
        try
        {
            var category = await context.Categories!.FirstOrDefaultAsync(c => c.Name == name && c.UserId == userId, cancellationToken);
            if(category == null) throw new NotFoundException(nameof(Category), name);

            return category;
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<List<Category>> GetDefaultCategoriesAsync()
    {
       var categories = new List<Category>
        {
            // Income
            new Category { Name = "Otros nómina y prestaciones", ParentType = CategoryParent.NóminaYOtrasPrestaciones, Type = CategoryType.Income },
            new Category { Name = "Pensión alimenticia", ParentType = CategoryParent.NóminaYOtrasPrestaciones, Type = CategoryType.Income },
            new Category { Name = "Prestación por desempleo", ParentType = CategoryParent.NóminaYOtrasPrestaciones, Type = CategoryType.Income },
            new Category { Name = "Abono de financiación", ParentType = CategoryParent.OtrosIngresos, Type = CategoryType.Income },
            new Category { Name = "Ingreso Bizum", ParentType = CategoryParent.OtrosIngresos, Type = CategoryType.Income },
            new Category { Name = "Ingresos de cheques", ParentType = CategoryParent.OtrosIngresos, Type = CategoryType.Income },
            new Category { Name = "Ingresos de efectivo", ParentType = CategoryParent.OtrosIngresos, Type = CategoryType.Income },
            new Category { Name = "Ingresos de impuestos", ParentType = CategoryParent.OtrosIngresos, Type = CategoryType.Income },
            new Category { Name = "Ingresos de otras entidades", ParentType = CategoryParent.OtrosIngresos, Type = CategoryType.Income },
            new Category { Name = "Ingresos por alquiler", ParentType = CategoryParent.OtrosIngresos, Type = CategoryType.Income },
            new Category { Name = "Otros ingresos (otros)", ParentType = CategoryParent.OtrosIngresos, Type = CategoryType.Income },

            // Expense
            new Category { Name = "Agua", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Alquiler trastero y garaje", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Alquiler vivienda", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Comunidad", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Decoración y mobiliario", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Hipoteca", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Hogar (otros)", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Impuestos hogar", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Limpieza", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Luz y gas", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Mantenimiento del hogar", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Seguridad y alarmas", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Seguro de hogar", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Teléfono, TV e internet", ParentType = CategoryParent.Hogar, Type = CategoryType.Expense },
            new Category { Name = "Gasolina y combustible", ParentType = CategoryParent.VehículoYTransporte, Type = CategoryType.Expense },
            new Category { Name = "Impuestos del vehículo", ParentType = CategoryParent.VehículoYTransporte, Type = CategoryType.Expense },
            new Category { Name = "Mantenimiento de vehículo", ParentType = CategoryParent.VehículoYTransporte, Type = CategoryType.Expense },
            new Category { Name = "Multas", ParentType = CategoryParent.VehículoYTransporte, Type = CategoryType.Expense },
            new Category { Name = "Parking y garaje", ParentType = CategoryParent.VehículoYTransporte, Type = CategoryType.Expense },
            new Category { Name = "Peajes", ParentType = CategoryParent.VehículoYTransporte, Type = CategoryType.Expense },
            new Category { Name = "Préstamo de vehículo", ParentType = CategoryParent.VehículoYTransporte, Type = CategoryType.Expense },
            new Category { Name = "Recarga vehículo eléctrico", ParentType = CategoryParent.VehículoYTransporte, Type = CategoryType.Expense },
            new Category { Name = "Seguro de coche y moto", ParentType = CategoryParent.VehículoYTransporte, Type = CategoryType.Expense },
            new Category { Name = "Taxi y Carsharing", ParentType = CategoryParent.VehículoYTransporte, Type = CategoryType.Expense },
            new Category { Name = "Transporte público", ParentType = CategoryParent.VehículoYTransporte, Type = CategoryType.Expense },
            new Category { Name = "Vehículo y transporte (otros)", ParentType = CategoryParent.VehículoYTransporte, Type = CategoryType.Expense },

            // Investment
            new Category { Name = "Acciones", ParentType = CategoryParent.Inversión, Type = CategoryType.Investment },
            new Category { Name = "Fondos de inversión", ParentType = CategoryParent.Inversión, Type = CategoryType.Investment },
            new Category { Name = "Otras inversiones", ParentType = CategoryParent.Inversión, Type = CategoryType.Investment },
            new Category { Name = "Planes de pensiones", ParentType = CategoryParent.Inversión, Type = CategoryType.Investment },

            new Category { Name = "Otros ahorros", ParentType = CategoryParent.Ahorro, Type = CategoryType.Investment },
            new Category { Name = "Productos de ahorro", ParentType = CategoryParent.Ahorro, Type = CategoryType.Investment },
            
            // Transfer
            new Category { Name = "Transfer", ParentType = CategoryParent.Transfer, Type = CategoryType.Transfer },

        };
        return await Task.FromResult(categories); 
    }

}
