using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;

namespace LanchesMac.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _dbContext;

    public CategoriaRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Categoria> categorias => _dbContext.Categorias;
}
