namespace TaxZone.DTO
{
    public class RetornoOperacao
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public object? Dados { get; set; }
        public RetornoOperacao(bool sucesso, string mensagem, object? dados = null)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
            Dados = dados;
        }
    }
}
