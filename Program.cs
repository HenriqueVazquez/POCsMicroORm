using POCMicroORM.services;
using POCMicroORM;

internal class Program
{
    private static void Main(string[] args)
    {
        var produto = new Produto { Nome = "Produto kuu", Preco = 99.99m };
        var produtoEditar = new Produto { Nome = "Produto zD", Preco = 1.99m };

        var produtoService = new ProdutoService();
        
        produtoService.InserirProduto(produto);
        produtoService.UpdateProduto(2, produtoEditar);
        var prod = produtoService.SelecionarProdutoID(2);

        Console.WriteLine($"ID: {prod.Id}");
        Console.WriteLine($"Nome: {prod.Nome}");
        Console.WriteLine($"Preço: {prod.Preco}");

        produtoService.DeleteUltimoProduto();

    }
}
