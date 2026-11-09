using LanchesMac.Models;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Componentes;

public class CarrinhoCompraResumo : ViewComponent
{
    private readonly CarinhoCompra _carinhoCompra;

    public CarrinhoCompraResumo(CarinhoCompra carinhoCompra)
    {
        _carinhoCompra = carinhoCompra;
    }

    public IViewComponentResult Invoke()
    {
        var items = _carinhoCompra.GetCarrinhoCompraItens();
        _carinhoCompra.CarrinhoCompraItens = items;

        var carrinhoCompraVm = new CarrinhoCompraViewModel
        {
            CarrinhoCompra = _carinhoCompra,
            CarrinhoCompraTotal = _carinhoCompra.GetCarrinhoCompraTotal()
        };

        return View(carrinhoCompraVm);
    }
}
