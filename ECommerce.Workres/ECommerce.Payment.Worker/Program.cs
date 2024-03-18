using ECommerce.Payment.Domain.Services;
using ECommerce.Payment.Domain.Services.Contracts;
using ECommerce.Payment.Worker;
using ECommerce.Payment.Worker.Consumers;
using ECommerce.Payment.Worker.Domain.Services;
using ECommerce.Payment.Worker.Domain.Services.Contracts;
using ECommerce.Payment.Worker.Infra;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;
        config.AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
        config.AddEnvironmentVariables();

    }).ConfigureServices((context, services) =>
    {
        services.AddDbContext<PaymentDbContext>();
        services.AddScoped<IPaymentConsumer, PaymentConsumer>();
        services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
        services.AddScoped<ICieloService, CieloService>();
        services.AddScoped<IStoneService, StoneService>();

        services.AddCap(x =>
        {
            var configuration = context.Configuration;
            x.UseSqlServer(configuration.GetConnectionString("PaymentConnection"));
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

