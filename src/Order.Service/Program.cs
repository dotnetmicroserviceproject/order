using common.Authentication;
using common.Filters.Swashbuckle;
using common.HealthChecks;
using common.Identity;
using common.MassTransit;
using common.MongoDB;
using common.Swagger;
using MassTransit;
using Order.Service.Entities;
using Order.Service.Exceptions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddMongo()
    .AddMongoRepository<CartItem>("CartItem")
    .AddMongoRepository<OrderDetails>("OrderDetails")
    .AddMongoRepository<OrderItem>("OrderItem")
    .AddMongoRepository<ProductItem>("ProductItem")
    .AddMongoRepository<UserItem>("UserItem");

    // ? Register MassTransit with RabbitMQ
builder.Services.AddMassTransitWithMessageBroker(builder.Configuration, retryConfigurator =>
{
    retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
    retryConfigurator.Ignore(typeof(UnknownItemException));
})
.AddJwtBearerAuthentication();



//MongoDB Health Check
builder.Services.AddHealthChecks()
                    .AddMongoDb();

builder.Services.AddApplicationConfig(builder.Configuration);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocs(
    title: "Order Microservice API",
    description: "Handles order management",
    version: "v1"
);
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<SnakeCaseDictionaryFilter>();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.CustomSchemaIds(type => type.FullName);

    c.SchemaFilter<SnakeCaseDictionaryFilter>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(
     options =>
     {
         options.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Service API");
     });


app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapPlayEconomyHealthChecks();


app.Run();




































//Random jitterer = new Random();

//builder.Services.AddHttpClient<ProductClient>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7240");
//})

//.AddTransientHttpErrorPolicy(builderR => builderR.Or<TimeoutRejectedException>().WaitAndRetryAsync(
//    5,
//        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
//                        +TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),
//        onRetry: (outcome, timespan, retryAttempt) =>
//        {
//            var serviceProvider = builder.Services.BuildServiceProvider();
//            serviceProvider.GetService<ILogger<ProductClient>>()?
//            .LogWarning($"Delaying for {timespan.TotalSeconds} seconds, tehn making retry {retryAttempt}");
//        }

//    ))
//.AddTransientHttpErrorPolicy(builderR => builderR.Or<TimeoutRejectedException>().CircuitBreakerAsync(
//    3,
//    TimeSpan.FromSeconds(15),
//    onBreak: (outcome, timespan) =>
//    {
//        var serviceProvider = builder.Services.BuildServiceProvider();
//        serviceProvider.GetService<ILogger<ProductClient>>()?
//        .LogWarning($"Opening the Circuit for {timespan.TotalSeconds} seconds...");
//    },
//     onReset:() =>
//     {
//         var serviceProvider = builder.Services.BuildServiceProvider();
//         serviceProvider.GetService<ILogger<ProductClient>>()?
//         .LogWarning($"Closing the circuit");
//     }
// ))
//.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));