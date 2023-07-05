using Dapper;
using System.Data.SqlClient;

namespace POCMicroORM.services
{
    public class ProdutoService
    {
        private string connectionString = "Server=./;Database=DB_POCMicroORM; User Id=sa;Password=123456;TrustServerCertificate=true";


        private void CriarBancoDeDadosETabelaSeNaoExistirem()
        {
            string masterConnectionString = "Server=./;Database=master; User Id=sa;Password=123456;TrustServerCertificate=true";
            string createDatabaseQuery = "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'DB_POCMicroORM') CREATE DATABASE DB_POCMicroORM;";

            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();

                connection.Execute(createDatabaseQuery);
            }

            string createTableQuery = @"
                USE DB_POCMicroORM;
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Produtos')
                CREATE TABLE Produtos
                (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Nome NVARCHAR(100) NOT NULL,
                    Preco DECIMAL(18,2) NOT NULL
                );";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                connection.Execute(createTableQuery);
            }
        }

        public void InserirProduto(Produto produto)
        {
            CriarBancoDeDadosETabelaSeNaoExistirem();

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            connection.Execute("INSERT INTO Produtos (Nome, Preco) VALUES (@Nome, @Preco)", produto);
        }

        public void UpdateProduto(int idProduto, Produto produto)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            produto.Id = idProduto;

            connection.Execute("UPDATE Produtos SET Nome = @Nome, Preco = @Preco WHERE Id = @Id", produto);
        }

        public Produto SelecionarProdutoID(int idProduto)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            return connection.QueryFirstOrDefault<Produto>("SELECT * FROM Produtos WHERE Id = @Id", new { Id = idProduto });
        }

        public void DeleteProduto(int idProduto)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            connection.Execute("DELETE FROM Produtos WHERE Id = @Id", new { Id = idProduto });
        }

        public void DeleteUltimoProduto()
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            // Consulta para selecionar o último produto inserido
            var query = "SELECT TOP 1 Id FROM Produtos ORDER BY Id DESC";
            var ultimoProdutoId = connection.QueryFirstOrDefault<int>(query);

            if (ultimoProdutoId > 0)
            {
                DeleteProduto(ultimoProdutoId);
            }
        }

    }
}
