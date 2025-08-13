using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DribionPayExemplos.Model
{
    public class PixDTO
    {
        public string IdPagamento { get; set; } // Um Id para referência interna da cobrança
        public string ReferenciaPagamento { get; set; } // Comentário sobre a referência interna da cobrança
        public string Observacao { get; set; } // Observação sobre a referência interna da cobrança
        public decimal Valor { get; set; } // Valor da cobrança Pix
        public int? DuracaoEmMinutos { get; set; } // Duração do Pix em minutos
        public string Ambiente { get; set; } // Ambiente de operação (Homologacao ou Producao)
        public string Plataforma { get; set; } // Plataforma de pagamento utilizada para gerar o Pix
        public string EmpresaKey { get; set; } // Chave da empresa 
    }
}
