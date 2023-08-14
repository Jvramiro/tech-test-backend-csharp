namespace TestJrAPI.Models {
    public class Produto : Entity {
        public string Nome { get; private set; }
        public decimal Preco { get; private set; }
        public int Quantidade { get; private set; }

        public Produto(string Nome, decimal Preco, int Quatidade) {
            this.Nome = Nome;
            this.Preco = Preco;
            this.Quantidade = Quatidade;
            this.CriadoEm = DateTime.Now;
        }

    }
}
