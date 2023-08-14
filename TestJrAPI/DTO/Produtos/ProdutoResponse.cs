namespace TestJrAPI.DTO.Produtos {
    public record ProdutoResponse(Guid Id, string Nome, decimal Preco, int Quantidade);
}
