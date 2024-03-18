using ECommerce.Stock.Worker;
using ECommerce.Stock.Worker.Consumers;
using ECommerce.Stock.Worker.Infra;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;
        config.AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
        config.AddEnvironmentVariables();

    }).ConfigureServices((context, services) =>
    {
        services.AddDbContext<ProductDBContext>();
        services.AddScoped<IProductStockConsumer, ProductStockConsumer>();

        services.AddCap(x =>
         {
             var configuration = context.Configuration;
             x.UseSqlServer(configuration.GetConnectionString("CatalogConnection"));
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
