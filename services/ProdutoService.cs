using PetaPoco;

namespace POCMicroORM.services
{
    public class ProdutoService
    {
        private string connectionString = "Server=./;Database=DB_POCMicroORM;User Id=sa;Password=123456;TrustServerCertificate=true";

        private void CriarBancoDeDadosETabelaSeNaoExistirem()
        {
            string masterConnectionString = "Server=./;Database=master;User Id=sa;Password=123456;TrustServerCertificate=true";
            string createDatabaseQuery = "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'DB_POCMicroORM') CREATE DATABASE DB_POCMicroORM;";

            using (var db = new Database(masterConnectionString, "System.Data.SqlClient"))
            {
                db.Execute(createDatabaseQuery);
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

            using (var db = new Database(connectionString, "System.Data.SqlClient"))
            {
                db.Execute(createTableQuery);
            }
        }

        public void InserirProduto(Produto produto)
        {
            CriarBancoDeDadosETabelaSeNaoExistirem();

            using var db = new Database(connectionString, "System.Data.SqlClient");
            db.Insert(produto);
        }

        public void UpdateProduto(int idProduto, Produto produto)
        {
            using var db = new Database(connectionString, "System.Data.SqlClient");
            produto.Id = idProduto;
            db.Update(produto);
        }

        public Produto SelecionarProdutoID(int idProduto)
        {
            using var db = new Database(connectionString, "System.Data.SqlClient");
            return db.SingleOrDefault<Produto>("SELECT * FROM Produtos WHERE Id = @0", idProduto);
        }

        public void DeleteProduto(int idProduto)
        {
            using var db = new Database(connectionString, "System.Data.SqlClient");
            db.Delete<Produto>(idProduto);
        }

        public void DeleteUltimoProduto()
        {
            using var db = new Database(connectionString, "System.Data.SqlClient");
            // Consulta para selecionar o último produto inserido
            var query = "SELECT TOP 1 Id FROM Produtos ORDER BY Id DESC";
            var ultimoProdutoId = db.ExecuteScalar<int>(query);

            if (ultimoProdutoId > 0)
            {
                db.Delete<Produto>(ultimoProdutoId);
            }
        }
    }
}
