using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DribionPayExemplos.Model
{
    public class ProcessaPagamentoResponseDTO
    {
        public bool Aprovado { get; set; }
        public string ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public string CardNumber { get; set; }
        public string AutorizationCode { get; set; }
        public string Provider { get; set; }
        public string PaymentId { get; set; }
    }
}
