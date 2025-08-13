using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DribionPayExemplos.Model
{
    public class GeraPixRetornoDTO
    {
        public string transactionId { get; set; }
        public string transactionDate { get; set; }
        public decimal totalAmount { get; set; }
        public string recipientDescription { get; set; }
        public string expirationDate { get; set; }
        public string imageContentPNG { get; set; }
        public string pixCola { get; set; }
        public string Key { get; set; }
        public string Link { get; set; }
    }
}
