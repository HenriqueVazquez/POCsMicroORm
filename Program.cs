using POCMicroORM.services;
using POCMicroORM;

internal class Program
{
    private static void Main(string[] args)
    {
        var produto = new Produto { Nome = "Produto MIBR", Preco = 7.99m };
        var produtoEditar = new Produto { Nome = "Produto Furia", Preco = 1.99m };

        var produtoService = new ProdutoService();
        
        produtoService.InserirProduto(produto);
        produtoService.UpdateProduto(2, produtoEditar);
        var prod = produtoService.SelecionarProdutoID(3);

        Console.WriteLine($"ID: {prod.Id}");
        Console.WriteLine($"Nome: {prod.Nome}");
        Console.WriteLine($"Preço: {prod.Preco}");

        produtoService.DeleteUltimoProduto();

    }
}
