using ECommerce.Order.Worker;
using ECommerce.Order.Worker.Consumer;
using ECommerce.Order.Worker.Domain.Services;
using ECommerce.Order.Worker.Domain.Services.Contracts;
using ECommerce.Order.Worker.Infra;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;
        config.AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
        config.AddEnvironmentVariables();

    }).ConfigureServices((context, services) =>
    {
        services.AddDbContext<OrderDbContext>();
        services.AddScoped<IOrderStatusOrderStatusPaymentConsumer, OrderStatusOrderStatusPaymentConsumer>();
        services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();


        services.AddCap(x =>
        {
            var configuration = context.Configuration;
            x.UseSqlServer(configuration.GetConnectionString("OrderConnection"));
            x.UseRabbitMQ(o =>
            {
                o.HostName = configuration["RabbitMQ:Host"];
                o.Password = configuration["RabbitMQ:UserName"];
                o.UserName = configuration["RabbitMQ:Password"];
                o.Port = 5672;
            });
        });

        services.AddHostedService<Worker>();

    }).Build();

builder.Run();
