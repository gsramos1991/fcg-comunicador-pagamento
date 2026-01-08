using FCG.Comunicador.Business.Interface;
using FCG.Comunicador.Business.Models;
using FCG.Comunicador.Service.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
namespace FCG.Comunicador.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IHttpClientFactory httpClientFactory, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task<ResponseGetwayPagamento> SendToGetway(NewPayment order)
        {
            _logger.LogInformation("Request para api de pagamento");
            var cliente = _httpClientFactory.CreateClient("ApiPagamento");
            
            var jsonRequest = JsonSerializer.Serialize(order);
            try
            {
                var response = await cliente.PostAsJsonAsync("/api/Payment/SolicitacaoCompra", order);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Resposta obtida com sucesso");
                    var valor = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Valor resposta: {valor}");
                    var reponseGetway =  JsonSerializer.Deserialize<ResponseGetwayPagamento>(valor);
                    return reponseGetway;
                }
                else
                {
                    _logger.LogWarning("Resposta obtida com erros");
                    var valor = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Valor resposta: {valor}");
                    var reponseGetway = JsonSerializer.Deserialize<ResponseGetwayPagamento>(valor);
                    return reponseGetway;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Falha no request da api, verifique excecao");
                throw new ArgumentException("Falha no processo", ex);
            }
        }

        public async Task UpdateOrder(ResponseGetwayPagamento order)
        {
            _logger.LogInformation("Atualizar ordem de pagamento com o codigo do pagamento {orderId}, codigo pagamento {paymentId}", order.orderId, order.paymentId);
            await _orderRepository.UpdateOrder(order);
        }
    }
}

