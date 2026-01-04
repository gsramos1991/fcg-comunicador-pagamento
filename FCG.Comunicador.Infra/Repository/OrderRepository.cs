using FCG.Comunicador.Business.Interface;
using FCG.Comunicador.Business.Models;
using FCG.Comunicador.Infra.Data;
using FCG.Comunicador.Service.Services;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FCG.Comunicador.Infra.Repository
{
    public class OrderRepository : IOrderRepository
    {

        
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task UpdateOrder(ResponseGetwayPagamento response)
        {
            try
            {
                var OrderDb = await _context.orders.FirstOrDefaultAsync(x => x.Id == response.orderId);

                if (OrderDb == null)
                {
                    throw new DataException($"OrderId nao encontrado no banco de dados {response.orderId}");
                }

                OrderDb.PaymentId = response.paymentId;
                OrderDb.Status = response.statusProcesso;
                OrderDb.TotalAmount = response.totalValue;

                _context.orders.Update(OrderDb);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            
        }
    }
}
