using Xunit;
using System;
using System.Collections.Generic;
using FCG.Comunicador.Business.Models; // Import the namespace for NewPayment and Item

namespace FCG.Comunicador.Getway.Pagamento.Tests
{
    public class NewPaymentTests
    {
        [Fact]
        public void NewPayment_PropertiesCanBeAssignedAndRead()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var currency = "BRL";
            var items = new List<Item>
            {
                new Item { JogoId = Guid.NewGuid(), Description = "Game 1", UnitPrice = 100.00m, Quantity = 1 }
            };

            // Act
            var newPayment = new NewPayment
            {
                OrderId = orderId,
                UserId = userId,
                currency = currency,
                Items = items
            };

            // Assert
            Assert.Equal(orderId, newPayment.OrderId);
            Assert.Equal(userId, newPayment.UserId);
            Assert.Equal(currency, newPayment.currency);
            Assert.Equal(items, newPayment.Items);
            Assert.Single(newPayment.Items);
        }

        [Fact]
        public void Item_PropertiesCanBeAssignedAndRead()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            var description = "Test Game";
            var unitPrice = 50.00m;
            var quantity = 2;

            // Act
            var item = new Item
            {
                JogoId = jogoId,
                Description = description,
                UnitPrice = unitPrice,
                Quantity = quantity
            };

            // Assert
            Assert.Equal(jogoId, item.JogoId);
            Assert.Equal(description, item.Description);
            Assert.Equal(unitPrice, item.UnitPrice);
            Assert.Equal(quantity, item.Quantity);
        }
    }
}
