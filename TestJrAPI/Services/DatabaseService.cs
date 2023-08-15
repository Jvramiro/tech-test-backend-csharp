using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using TestJrAPI.Data;
using TestJrAPI.Enums;
using TestJrAPI.Models;

namespace TestJrAPI.Services {
    public class DatabaseService {

        private readonly SqlContext sqlContext;
        private readonly MongoDBContext mongoDBContext;
        private readonly FileContext fileContext;
        public DatabaseService(SqlContext sqlContext, MongoDBContext mongoDBContext, FileContext fileContext) {
            this.sqlContext = sqlContext;
            this.mongoDBContext = mongoDBContext;
            this.fileContext = fileContext;
        }

        public async void CreateProduto(Produto produto, DatabaseSelection database) {

            if (database == DatabaseSelection.SqlServer) {
                await sqlContext.Produtos.AddAsync(produto);
                await sqlContext.SaveChangesAsync();
            }
            else if (database == DatabaseSelection.MongoDB) {
                await mongoDBContext.Produtos.InsertOneAsync(produto);
            }
            else if (database == DatabaseSelection.FileText) {
                await fileContext.CreateAsync(produto);
            }

        }

        public async Task<List<Produto>> GetAllProduto(DatabaseSelection database, int page = 1, int rows = 10) {

            if (database == DatabaseSelection.SqlServer) {
                var produtos = await sqlContext.Produtos.AsNoTracking().Skip((page - 1) * rows).Take(rows).ToListAsync();
                return produtos;
            }
            else if (database == DatabaseSelection.MongoDB) {
                var produtos = await mongoDBContext.Produtos.Find(_ => true).Skip((page - 1) * rows).Limit(rows).ToListAsync();
                return produtos;
            }
            else if (database == DatabaseSelection.FileText) {
                var produtos = await fileContext.GetAllAsync<Produto>();
                return produtos.Skip((page - 1) * rows).Take(rows).ToList();
            }
            else {
                return null;
            }

        }

        public async Task<Produto> GetById(Guid id, DatabaseSelection database) {

            if (database == DatabaseSelection.SqlServer) {
                var produto = await sqlContext.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                return produto;
            }
            else if (database == DatabaseSelection.MongoDB) {
                var produto = await mongoDBContext.Produtos.Find(p => p.Id == id).FirstOrDefaultAsync();
                return produto;
            }
            else if (database == DatabaseSelection.FileText) {
                var produto = await fileContext.GetByIdAsync<Produto>(p => p.Id == id);
                return produto;
            }
            else {
                return null;
            }

        }

        public async void UpdateProduto(Produto produto, DatabaseSelection database) {

            if (database == DatabaseSelection.SqlServer) {
                sqlContext.Produtos.Update(produto);
                await sqlContext.SaveChangesAsync();
            }
            else if (database == DatabaseSelection.MongoDB) {
                await mongoDBContext.Produtos.ReplaceOneAsync(p => p.Id == produto.Id, produto);
            }
            else if (database == DatabaseSelection.FileText) {
                await fileContext.UpdateAsync<Produto>(p => p.Id == produto.Id, produto);
            }

        }

        public async void DeleteProduto(Produto produto, DatabaseSelection database) {

            if (database == DatabaseSelection.SqlServer) {
                sqlContext.Produtos.Remove(produto);
                await sqlContext.SaveChangesAsync();
            }
            else if (database == DatabaseSelection.MongoDB) {
                await mongoDBContext.Produtos.DeleteOneAsync(p => p.Id == produto.Id);
            }
            else if (database == DatabaseSelection.FileText) {
                await fileContext.DeleteAsync<Produto>(p => p.Id == produto.Id);
            }

        }

    }
}
