using TestJrAPI.Enums;
using TestJrAPI.Models;

namespace TestJrAPI.Interfaces {
    public interface IDatabaseService {
        void CreateProduto(Produto produto, DatabaseSelection database);
        Task<List<Produto>> GetAllProduto(DatabaseSelection database, int page = 1, int rows = 10);
        Task<Produto> GetById(Guid id, DatabaseSelection database);
        void UpdateProduto(Produto produto, DatabaseSelection database);
        void DeleteProduto(Produto produto, DatabaseSelection database);
    }
}
