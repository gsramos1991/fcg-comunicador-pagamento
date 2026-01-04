using FCG.Comunicador.Business.Interface;
using FCG.Comunicador.Business.Models;
using FCG.Comunicador.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FCG.Comunicador.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderService(IOrderRepository orderRepository, IHttpClientFactory httpClientFactory)
        {
            _orderRepository = orderRepository;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseGetwayPagamento> SendToGetway(NewPayment order)
        {
            var cliente = _httpClientFactory.CreateClient("ApiPagamento");
            
            var jsonRequest = JsonSerializer.Serialize(order);
            try
            {
                var response = await cliente.PostAsJsonAsync("/api/Payment/SolicitacaoCompra", order);
                if (response.IsSuccessStatusCode)
                {
                    var valor = await response.Content.ReadAsStringAsync();
                    var reponseGetway =  JsonSerializer.Deserialize<ResponseGetwayPagamento>(valor);
                    return reponseGetway;
                }
                else
                {
                    var valor = await response.Content.ReadAsStringAsync();
                    var reponseGetway = JsonSerializer.Deserialize<ResponseGetwayPagamento>(valor);
                    return reponseGetway;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Falha no processo", ex);
            }
        }

        public async Task UpdateOrder(ResponseGetwayPagamento order)
        {
            await _orderRepository.UpdateOrder(order);
        }
    }
}

