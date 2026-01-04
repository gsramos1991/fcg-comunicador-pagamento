using FCG.Comunicador.Business.Interface;

using FCG.Comunicador.Infra.Data;
using FCG.Comunicador.Infra.Repository;
using FCG.Comunicador.Service.Services;
using FCG.Comunicador.Service.Services.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddHttpClient<IOrderRepository, OrderRepository>();

builder.Services.AddMassTransit(x =>
{
    // Adiciona o seu consumidor (o código que processa a mensagem)
    x.AddConsumer<PaymentRequestConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetSection("RabbitMq:HostName").Value, h =>
        {
            h.Username(builder.Configuration.GetSection("RabbitMq:UserName").Value!);
            h.Password(builder.Configuration.GetSection("RabbitMq:Password").Value!);
        });

        // Configura o endpoint (Fila)
        cfg.ReceiveEndpoint("payment-requests", e =>
        {
            
            e.ClearSerialization();
            e.UseRawJsonSerializer();
            // Tenta 3 vezes com intervalo de 5 segundos antes de mandar para o erro
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

            // Define o consumidor
            e.ConfigureConsumer<PaymentRequestConsumer>(context);
            
        });
    });
});

builder.Services.AddHttpClient("ApiPagamento", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("ApiPagamento:url").Value!.TrimEnd('/'));
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
var host = builder.Build();
host.Run();
