using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using FCG.Comunicador.Business.Interface;
using FCG.Comunicador.Business.Models;
using FCG.Comunicador.Service.Services;
using FCG.Comunicador.Service.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;

namespace FCG.Comunicador.Getway.Pagamento.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<ILogger<OrderService>> _mockLogger;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockLogger = new Mock<ILogger<OrderService>>();

            // Setup a default HttpClient for IHttpClientFactory, though not directly used by UpdateOrder
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
                                  .Returns(new HttpClient(new MockHttpMessageHandler()));


            _orderService = new OrderService(
                _mockOrderRepository.Object,
                _mockHttpClientFactory.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task UpdateOrder_CallsRepositoryUpdateOrder()
        {
            // Arrange
            var responseGetwayPagamento = new ResponseGetwayPagamento
            {
                orderId = Guid.NewGuid(),
                paymentId = Guid.NewGuid(), // Corrected from string to Guid
                statusPayment = "Approved", // Corrected property name from status to statusPayment
                message = "Payment Approved"
            };

            // Act
            await _orderService.UpdateOrder(responseGetwayPagamento);

            // Assert
            _mockOrderRepository.Verify(repo => repo.UpdateOrder(responseGetwayPagamento), Times.Once);
        }

        // Helper class for HttpClientFactory mock, for methods not directly under test
        private class MockHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"orderId\":\"" + Guid.NewGuid() + "\",\"paymentId\":\"mockId\",\"status\":\"Approved\",\"message\":\"Mocked Response\"}")
                });
            }
        }
    }
}
