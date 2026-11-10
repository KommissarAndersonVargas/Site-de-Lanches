using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Controllers;

public class LancheController : Controller
{
    private readonly ILancheRepository _lancheRepository;

    public LancheController(ILancheRepository lancheRepository)
    {
        _lancheRepository = lancheRepository;
    }

    public IActionResult List(string nomeCategoria)
    {
        IEnumerable<Lanche> lanches;
        string categoriaAtual = string.Empty;

        if (string.IsNullOrEmpty(nomeCategoria))
        {
            lanches = _lancheRepository.Lanches.OrderBy(l => l.LancheId);
            categoriaAtual = "Todos os lanches";
        }
        else
        {
            lanches = _lancheRepository.Lanches
                      .Where(l=> l.Categoria.CategoriaNome.Equals(nomeCategoria, StringComparison.OrdinalIgnoreCase))
                      .OrderBy(l => l.Nome);
        }

        var lanchesListViewModel = new LancheListViewModel
        {
            Lanche = lanches,
            CategoriaAtual = categoriaAtual
        };

        return View(lanchesListViewModel);

    }
}
