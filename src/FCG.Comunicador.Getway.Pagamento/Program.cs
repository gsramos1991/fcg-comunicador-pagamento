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
    // Adiciona o seu consumidor
    x.AddConsumer<PaymentRequestConsumer>();

    // Troca de UsingRabbitMq para UsingAzureServiceBus
    x.UsingAzureServiceBus((context, cfg) =>
    {
        // Pega a string de conexão do appsettings.json
        var connectionString = builder.Configuration.GetSection("ConfigFila:ConnectionString").Value;
        var nomeFila = builder.Configuration.GetSection("ConfigFila:NomeFila").Value!;
        cfg.Host(connectionString);

        // Configura o endpoint (Fila no Azure Service Bus)
        cfg.ReceiveEndpoint(nomeFila, e =>
        {
            e.ConfigureConsumeTopology = false;
            
            // O Azure Service Bus lida bem com JSON, mas se precisar manter o Raw:
            e.ClearSerialization();
            e.UseRawJsonSerializer();

            // Resiliência: Tenta 3 vezes com intervalo de 5 segundos
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

            e.MaxDeliveryCount = 2;

            // Define o consumidor para esta fila
            e.ConfigureConsumer<PaymentRequestConsumer>(context);

            // Opcional: No Azure, você pode configurar o tempo de lock da mensagem
            // e.LockDuration = TimeSpan.FromMinutes(5);
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
