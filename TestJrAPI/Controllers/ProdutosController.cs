using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestJrAPI.Data;
using TestJrAPI.DTO.Produtos;
using TestJrAPI.Enums;
using TestJrAPI.Models;
using TestJrAPI.Services;

namespace TestJrAPI.Controllers {
    [Route("/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase {

        private readonly DatabaseService databaseService;
        public ProdutosController(DatabaseService databaseService) {
            this.databaseService = databaseService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProdutoRequest request, DatabaseSelection databaseSelection = 0){

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

            databaseService.CreateProduto(produto, databaseSelection);

            var response = new ProdutoResponse(produto.Id, produto.Nome, produto.Preco, produto.Quantidade, produto.ValorTotal);

            return Created($"Produto {response.Nome} criado com sucesso", response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int rows = 10, DatabaseSelection databaseSelection = 0) {

            if(rows > 30) {
                return BadRequest("The number of rows cannot exceed 10");
            }

            var produtos = await databaseService.GetAllProduto(databaseSelection, page, rows);

            if(produtos == null) {
                return NotFound("Nenhum Produto válido encontrado");
            }

            var response = produtos.Select(p => new ProdutoResponse(p.Id, p.Nome, p.Preco, p.Quantidade, p.ValorTotal));

            return Ok(response);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, DatabaseSelection databaseSelection = 0) {

            var produto = await databaseService.GetById(id, databaseSelection);

            if(produto == null) {
                return NotFound("Id não encontrado");
            }

            var response = new ProdutoResponse(produto.Id, produto.Nome, produto.Preco, produto.Quantidade, produto.ValorTotal);

            return Ok(produto);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProdutoRequest request, DatabaseSelection databaseSelection = 0) {

            var produto = await databaseService.GetById(id, databaseSelection);

            if (produto == null) {
                return NotFound("Id não encontrado");
            }

            produto.Update(request.Nome, request.Preco, request.Quatidade, request.Ativo ?? false);

            databaseService.UpdateProduto(produto, databaseSelection);

            return Ok($"Produto {produto.Nome} editado com sucesso");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, DatabaseSelection databaseSelection = 0) {

            var produto = await databaseService.GetById(id, databaseSelection);

            if (produto == null) {
                return NotFound("Id não encontrado");
            }

            databaseService.DeleteProduto(produto, databaseSelection);

            return Ok($"Produto {produto.Nome} deletado com sucesso");

        }



    }
}
