using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Comunicador.Service.Services
{
    public interface IRabbitMqService : IDisposable
    {
        Task Consume();
    }
}
