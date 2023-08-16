namespace TestJrAPI.DTO.Produtos {
    public record ProdutoRequest(string Nome, decimal Preco, int Quantidade, bool? Ativo = true);
}
