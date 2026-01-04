using FCG.Comunicador.Business.Models;
using FCG.Comunicador.Service.Services.Interfaces;
using MassTransit;

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
            // O MassTransit já converte o JSON para o objeto 'context.Message' automaticamente
            var dados = context.Message;

            Console.WriteLine($"[Processando MassTransit]: {dados.OrderId}");

            
            var dadosGetway = await _orderService.SendToGetway(dados);

            if(dadosGetway != null)
            {
                if(dadosGetway.statusProcesso == -2)
                {
                    Console.WriteLine($"[Ordem de serviço já existe no banco de dados (recompra indevida)]: {dados.OrderId}");
                    return;
                }
            }

            await _orderService.UpdateOrder(dadosGetway);
        }
    }
}
