using System.ComponentModel.DataAnnotations;

namespace TestJrAPI.Models {
    public class Produto : Entity {
        [Required]
        [StringLength(50)]
        public string Nome { get; private set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Preco { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorTotal { get; private set; }

        public Produto(string Nome, decimal Preco, int Quantidade) {
            this.Nome = Nome;
            this.Preco = Preco;
            this.Quantidade = Quantidade;
            this.Ativo = true;
            this.ValorTotal = Quantidade * Preco;

            this.CriadoEm = DateTime.Now;
            this.EditadoEm = DateTime.Now;
        }

        public void Update(string Nome, decimal Preco, int Quantidade, bool Ativo = true) {
            this.Nome = Nome;
            this.Preco = Preco;
            this.Quantidade = Quantidade;
            this.Ativo = Ativo;
            this.ValorTotal = Quantidade * Preco;

            this.EditadoEm = DateTime.Now;
        }

    }
}
