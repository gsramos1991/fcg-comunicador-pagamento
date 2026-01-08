using FCG.Comunicador.Business.Models;
using FCG.Comunicador.Service.Services.Interfaces;
using MassTransit;
using Serilog;

namespace FCG.Comunicador.Service.Services
{
    public class PaymentRequestConsumer : IConsumer<NewPayment> // PaymentRequest é sua classe/DTO da mensagem
    {
        private readonly IOrderService _orderService;
        private readonly IGameService _gameService;

        public PaymentRequestConsumer(IOrderService orderService, IGameService gameService)
        {
            _orderService = orderService;
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<NewPayment> context)
        {
            
            var dados = context.Message;

            Log.Information("[Processando MassTransit]: {OrderId}", dados.OrderId);

            
            var dadosGetway = await _orderService.SendToGetway(dados);

            if(dadosGetway != null)
            {
                if(dadosGetway.statusProcesso == -2)
                {
                    Log.Information("[Ordem de serviço já existe no banco de dados (recompra indevida)]: {OrderId}", dados.OrderId);
                    return;
                }
            }

            await _orderService.UpdateOrder(dadosGetway);
        }
    }
}
