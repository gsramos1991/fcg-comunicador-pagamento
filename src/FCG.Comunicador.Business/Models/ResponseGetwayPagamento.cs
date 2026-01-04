using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Comunicador.Business.Models
{

    public class ResponseGetwayPagamento
    {
        public Guid orderId { get; set; }
        public Guid paymentId { get; set; }
        public string statusPayment { get; set; } = string.Empty;
        public int statusProcesso { get; set; }
        public decimal totalValue { get; set; }
        public bool success { get; set; }
        public string message { get; set; } = string.Empty;
        public DateTime processedAt { get; set; }
    }

}
