using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestJrAPI.Data;
using TestJrAPI.DTO.Produtos;
using TestJrAPI.Models;

namespace TestJrAPI.Controllers {
    [Route("/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase {

        private readonly SqlContext sqlContext;
        public ProdutosController(SqlContext sqlContext) {
            this.sqlContext = sqlContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProdutoRequest request){

            if (!ModelState.IsValid) {
                return BadRequest("Data inválida");
            }

            if(request.Preco < 0) {
                return BadRequest("O valor de Preço não deve ser negativo");
            }

            Produto produto = new Produto(
                request.Nome,
                request.Preco,
                request.Quatidade
            );

            await sqlContext.Produtos.AddAsync(produto);
            await sqlContext.SaveChangesAsync();

            var response = new ProdutoResponse(produto.Id, produto.Nome, produto.Preco, produto.Quantidade);

            return Created($"Produto {response.Nome} criado com sucesso", response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int rows = 10) {

            if(rows > 30) {
                return BadRequest("The number of rows cannot exceed 10");
            }

            var produtos = await sqlContext.Produtos.AsNoTracking().Skip((page - 1) * rows).Take(rows).ToListAsync();

            if(produtos == null) {
                return NotFound("Nenhum Produto válido encontrado");
            }

            var response = produtos.Select(p => new ProdutoResponse(p.Id, p.Nome, p.Preco, p.Quantidade));

            return Ok(response);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {

            var produto = await sqlContext.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if(produto == null) {
                return NotFound("Id não encontrado");
            }

            var response = new ProdutoResponse(produto.Id, produto.Nome, produto.Preco, produto.Quantidade);

            return Ok(produto);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id) {

            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {

            var produto = await sqlContext.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null) {
                return NotFound("Id não encontrado");
            }

            sqlContext.Produtos.Remove(produto);
            await sqlContext.SaveChangesAsync();

            return Ok($"Produto {produto.Nome} deletado com sucesso");

        }



    }
}
