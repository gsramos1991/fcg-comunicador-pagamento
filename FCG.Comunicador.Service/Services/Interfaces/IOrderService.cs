using FCG.Comunicador.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Comunicador.Service.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseGetwayPagamento> SendToGetway(NewPayment order);
        Task UpdateOrder(ResponseGetwayPagamento order);
    }
}
