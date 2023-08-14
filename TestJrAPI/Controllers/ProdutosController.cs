using Microsoft.AspNetCore.Mvc;
using TestJrAPI.DTO.Produtos;
using TestJrAPI.Models;

namespace TestJrAPI.Controllers {
    [Route("/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase {

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

            var response = new ProdutoResponse(produto.Id, produto.Nome, produto.Preco, produto.Quantidade);

            return Created($"Produto {response.Nome} criado com sucesso", response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int rows = 10) {

            if(rows > 30) {
                return BadRequest("The number of rows cannot exceed 10");
            }

            List<Produto> produtos = new List<Produto>();

            var response = produtos.Select(p => new ProdutoResponse(p.Id, p.Nome, p.Preco, p.Quantidade));

            return Ok(response);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {

            Produto produto = new Produto("Teste", 10, 10);

            var response = new ProdutoResponse(produto.Id, produto.Nome, produto.Preco, produto.Quantidade);

            return Ok(produto);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id) {

            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {

            Produto produto = new Produto("Teste", 10, 10);

            var response = new ProdutoResponse(produto.Id, produto.Nome, produto.Preco, produto.Quantidade);

            return Ok($"Produto {response.Nome} criado com sucesso");

        }



    }
}
