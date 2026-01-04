using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Comunicador.Business.Models
{
  
    public class NewPayment
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string currency { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public Guid JogoId { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

}
