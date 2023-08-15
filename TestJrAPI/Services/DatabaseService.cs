using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestJrAPI.Data;
using TestJrAPI.Models;

namespace TestJrAPI.Services {
    public class DatabaseService {

        private readonly SqlContext sqlContext;
        public DatabaseService(SqlContext sqlContext) {
            this.sqlContext = sqlContext;
        }

        public async void CreateProduto(Produto produto) {
            await sqlContext.Produtos.AddAsync(produto);
            await sqlContext.SaveChangesAsync();
        }

        public async Task<List<Produto>> GetAllProduto(int page = 1, int rows = 10) {
            var produtos = await sqlContext.Produtos.AsNoTracking().Skip((page - 1) * rows).Take(rows).ToListAsync();
            return produtos;
        }

        public async Task<Produto> GetById(Guid id) {
            var produto = await sqlContext.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return produto;
        }

        public async void UpdateProduto(Produto produto) {
            sqlContext.Produtos.Update(produto);
            await sqlContext.SaveChangesAsync();
        }

        public async void DeleteProduto(Produto produto) {
            sqlContext.Produtos.Remove(produto);
            await sqlContext.SaveChangesAsync();
        }

    }
}
