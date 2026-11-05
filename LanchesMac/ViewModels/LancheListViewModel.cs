using LanchesMac.Models;

namespace LanchesMac.ViewModels;

public class LancheListViewModel
{
    public IEnumerable<Lanche> Lanche { get; set; }

    public string CategoriaAtual { get; set; }
}
