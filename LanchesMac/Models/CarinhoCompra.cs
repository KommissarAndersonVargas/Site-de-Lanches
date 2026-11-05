using LanchesMac.Context;
using LanchesMac.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Models;

public class CarinhoCompra
{
    private readonly AppDbContext _context;

    public CarinhoCompra(AppDbContext context)
    {
        _context = context;
    }

    public string CarrinhoCompraId { get; set; }

    public List<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }

    public static CarinhoCompra GetCarinho(IServiceProvider services)
    {
        //Define uma sessão
        ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

        var context = services.GetService<AppDbContext>();

        //obtem ou gera o Id do carrinho
        var carrinhoId = session.GetString("CarrinhoId") ?? Guid.NewGuid().ToString();

        //atribui o id do carrinho na sessão
        session.SetString("CarrinhoId", carrinhoId);

        // retrona o carrinho com o contexto e o Id atribuido ou obtido

        return new CarinhoCompra(context)
        {
            CarrinhoCompraId = carrinhoId
        };

    }

    public void AdicionarAoCarrinho(Lanche lanche)
    {
        var carrinhoCompraItem =
            _context.CarrinhoCompraItens
            .SingleOrDefault(s => s.Lanche.LancheId == lanche.LancheId
            && s.CarinhoCompraId == CarrinhoCompraId);

        if (carrinhoCompraItem == null)
        {
            carrinhoCompraItem = new CarrinhoCompraItem
            {
                CarinhoCompraId = CarrinhoCompraId,
                Lanche = lanche,
                Quantidade = 1
            };

            _context.CarrinhoCompraItens.Add(carrinhoCompraItem);
        }

        else
        {
            carrinhoCompraItem.Quantidade++;
        }

        _context.SaveChanges();
    }

    public int RemoverDoCarrinho(Lanche lanche)
    {
        var carrinhoCompraItem =
            _context.CarrinhoCompraItens
            .SingleOrDefault(s => s.Lanche.LancheId == lanche.LancheId
            && s.CarinhoCompraId == CarrinhoCompraId);

        var quantidadeLocal = 0;

        if (carrinhoCompraItem != null)
        {
            if (carrinhoCompraItem.Quantidade > 1)
            {
                carrinhoCompraItem.Quantidade--;
                quantidadeLocal = carrinhoCompraItem.Quantidade;
            }
            else
            {
                _context.CarrinhoCompraItens.Remove(carrinhoCompraItem);
            }
        }

        _context.SaveChanges();

        return quantidadeLocal;
    }

    public List<CarrinhoCompraItem> GetCarrinhoCompraItens()
    {
        return CarrinhoCompraItens ?? (CarrinhoCompraItens = _context.CarrinhoCompraItens
      .Where(c => c.CarinhoCompraId == CarrinhoCompraId)
      .Include(s => s.Lanche)
      .ToList());
    }

    public void LimparCarrinho()
    {
        var carrinhoItens = 
            _context.CarrinhoCompraItens
            .Where(carrinho => carrinho.CarinhoCompraId == CarrinhoCompraId);

        _context.CarrinhoCompraItens
            .RemoveRange(carrinhoItens);

        _context.SaveChanges();
    }

    public decimal GetCarrinhoCompraTotal()
    {
        var total = _context.CarrinhoCompraItens
            .Where(c => c.CarinhoCompraId == CarrinhoCompraId)
            .Select(c => c.Lanche.Preco * c.Quantidade).Sum();

        return total;
    }
}
