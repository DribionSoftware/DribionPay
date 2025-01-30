namespace ExemploCSharp.DTO
{
    public class BoletoConsultaResultDTO
    {
        public string nossoNumero { get; set; }
        public string? linhaDigitavel { get; set; }
        public string? codigoBarras { get; set; }
        public string? codigoInterno { get; set; }
        public string? status { get; set; }
        public decimal valor { get; set; }
        public DateTime vencimento { get; set; }
        public DateTime? dataPagamento { get; set; }
        public decimal? valorPago { get; set; }
        public decimal? valorDesconto { get; set; }
        public decimal? valorMulta { get; set; }
        public decimal? valorJuros { get; set; }
    }
}
