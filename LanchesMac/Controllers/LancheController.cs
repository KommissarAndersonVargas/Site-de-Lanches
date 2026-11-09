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

        var naoExisteLanche = !_lancheRepository.Lanches
            .Any(l => l.Categoria.CategoriaNome
            .Equals(nomeCategoria, StringComparison.OrdinalIgnoreCase));

        if (string.IsNullOrEmpty(nomeCategoria) || naoExisteLanche)
        {
            lanches = _lancheRepository.Lanches.OrderBy(l => l.LancheId);
            categoriaAtual = "Todos os lanches";
        }
        else
        {
            if (string.Equals("Normal", nomeCategoria, StringComparison.OrdinalIgnoreCase))
            {
                lanches = _lancheRepository.Lanches
                    .Where(l => l.Categoria.CategoriaNome.Equals("Normal"))
                    .OrderBy(l => l.Nome);

                categoriaAtual = "Normal";
            }
            else
            {

                lanches = _lancheRepository.Lanches
                    .Where(l => l.Categoria.CategoriaNome.Equals("Natural"))
                    .OrderBy(l => l.Nome);

                categoriaAtual = "Natural";
            }
        }

        var lanchesListViewModel = new LancheListViewModel
        {
            Lanche = lanches,
            CategoriaAtual = categoriaAtual
        };

        return View(lanchesListViewModel);

    }
}
