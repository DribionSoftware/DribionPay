namespace ExemploCSharp.DTO
{
    public class BoletoDTO
    {
        public DateTime dataEmissao { get; set; }
        public DateTime dataVencimento { get; set; }
        public decimal valorNominal { get; set; }
        public decimal valorAbatimento { get; set; }
        public string seuNumero { get; set; }
        public int diasProtesto { get; set; }
        public int diasNegativacao { get; set; }
        public int numeroDiasLimiteRecebimento { get; set; }
        public EnviarBoletoDTO_Desconto Desconto { get; set; }
        public EnviarBoletoDTO_Multa Multa { get; set; }
        public EnviarBoletoDTO_Juros Juros { get; set; }
        public EnviarBoletoDTO_Pagador Pagador { get; set; }
        public string[] informativos { get; set; }
        public string[] mensagens { get; set; }
        public string banco { get; set; }
        public string ambiente { get; set; }
        public string empresaKey { get; set; }
    }

    public class EnviarBoletoDTO_Desconto
    {
        public string tipo { get; set; }
        public decimal taxa { get; set; }
        public decimal valor { get; set; }
        public DateTime data { get; set; }
    }

    public class EnviarBoletoDTO_Multa
    {
        public string tipo { get; set; }
        public DateTime data { get; set; }
        public decimal porcentagem { get; set; }
        public decimal valor { get; set; }
    }

    public class EnviarBoletoDTO_Juros
    {
        public string tipo { get; set; }
        public decimal taxa { get; set; }
        public decimal valor { get; set; }
        public DateTime data { get; set; }
    }

    public class EnviarBoletoDTO_Pagador
    {
        public string cpfCnpj { get; set; }
        public string nome { get; set; }
        public string endereco { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cep { get; set; }
        public string email { get; set; }
        public string ddd { get; set; }
        public string telefone { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
    }
}
