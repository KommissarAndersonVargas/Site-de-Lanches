using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class CarrinhoCompraController : Controller
    {

        private readonly ILancheRepository lancheRepository;

        private readonly CarinhoCompra carinhoCompra;

        public CarrinhoCompraController(ILancheRepository lancheRepository, CarinhoCompra carinhoCompra)
        {
            this.lancheRepository = lancheRepository;
            this.carinhoCompra = carinhoCompra;
        }

        public IActionResult Index()
        {
            var items = carinhoCompra.GetCarrinhoCompraItens();
            carinhoCompra.CarrinhoCompraItens = items;

            var carrinhoCompraVm = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = carinhoCompra,
                CarrinhoCompraTotal = carinhoCompra.GetCarrinhoCompraTotal()
            };

            return View(carrinhoCompraVm);
        }

        public IActionResult AdicionarItemNoCarrinhoCompra(int lancheId)
        {
            var lancheSelecionado = lancheRepository.Lanches.
                FirstOrDefault(p => p.LancheId == lancheId);

            if(lancheSelecionado != null)
            {
                carinhoCompra.AdicionarAoCarrinho(lancheSelecionado);
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoverItemDoCarrinhoCompra(int lancheId)
        {
            var lancheSelecionado = lancheRepository.Lanches.
               FirstOrDefault(p => p.LancheId == lancheId);

            if (lancheSelecionado != null)
            {
                carinhoCompra.RemoverDoCarrinho(lancheSelecionado);
            }

            return RedirectToAction("Index");
        }
    }
}
