namespace TestJrAPI.DTO.Produtos {
    public record ProdutoRequest(string Nome, decimal Preco, int Quatidade, bool? Ativo = true);
}
