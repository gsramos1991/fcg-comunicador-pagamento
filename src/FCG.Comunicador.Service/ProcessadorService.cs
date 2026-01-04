using FCG.Comunicador.Business.Models;
using FCG.Comunicador.Service.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FCG.Comunicador.Service
{
    public class ProcessadorService
    {
        private readonly IOrderService _orderService;
        

        public ProcessadorService(IOrderService orderService)
        {
            _orderService = orderService;
            
        }

        public async Task Processar(string message)
        {
            
            var newPayment = JsonSerializer.Deserialize<NewPayment>(message);
            if (newPayment != null)
            {
                await _orderService.SendToGetway(newPayment);
            }
            
        }
    }
}
