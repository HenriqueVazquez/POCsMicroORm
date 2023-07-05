using PetaPoco;

[TableName("Produtos")] // Especifica o nome da tabela no banco de dados
[PrimaryKey("Id")] // Especifica a chave primária
public class Produto
{
    [Column("Id")] // Especifica o nome da coluna correspondente à propriedade
    public int Id { get; set; }

    [Column("Nome")]
    public string? Nome { get; set; }

    [Column("Preco")]
    public decimal Preco { get; set; }
}
