namespace TestJrAPI.Models {
    public class Produto : Entity {
        public string Nome { get; private set; }
        public decimal Preco { get; private set; }
        public int Quantidade { get; private set; }

        public Produto(string Nome, decimal Preco, int Quantidade) {
            this.Nome = Nome;
            this.Preco = Preco;
            this.Quantidade = Quantidade;
            this.Ativo = true;
            this.CriadoEm = DateTime.Now;
        }

        public void Update(string Nome, decimal Preco, int Quantidade, bool Ativo = true) {
            this.Nome = Nome;
            this.Preco = Preco;
            this.Quantidade = Quantidade;
            this.Ativo = Ativo;
        }

    }
}
