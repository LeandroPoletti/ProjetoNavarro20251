namespace ProjetoEntidades.Models
{
    public class Pedido
    {
        public string NomeCliente { get; set; }
        public string NomeProduto { get; set; }
        public double Valor { get; set; }
        public int Quantidade { get; set; }
    }
}