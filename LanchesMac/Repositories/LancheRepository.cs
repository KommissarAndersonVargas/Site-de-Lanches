using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Repositories;

public class LancheRepository : ILancheRepository
{
    private readonly AppDbContext _dbContext;

    public IEnumerable<Lanche> Lanches => _dbContext.Lanches.Include(categoria=> categoria.Categoria);

    public IEnumerable<Lanche> LanchesPreferido => _dbContext.Lanches.Where(lanche=> lanche.IsLanchePreferido)
                                                                     .Include(cat=> cat.Categoria);

    public LancheRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Lanche GetLancheById(int id)
    {
        var lanche = _dbContext.Lanches.FirstOrDefault(lanche => lanche.LancheId == id);

        return lanche;
    }
}
