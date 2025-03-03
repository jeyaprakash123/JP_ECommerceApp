using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("EcommerceServer");

var authDb= sqlServer.AddDatabase("ECommerceAuth");

var productDb = sqlServer.AddDatabase("ECommerceProduct");

var orderDb = sqlServer.AddDatabase("ECommerceOrder");

var redis = builder.AddRedis("cache");


var apiService = builder.AddProject<Projects.ECommerceApp_ApiService>("apiservice");

builder.AddProject<Projects.ECommerceApp_Auth_Api>("authservice");
//.WithReference(authDb);

var productService =builder.AddProject<Projects.ECommerceApp_Product_Api>("productService");
   // .WithReference(productDb);

builder.AddProject<Projects.ECommerceApp_Cart_Api>("cartService");

builder.AddProject<Projects.EcommerceApp_Order_Api>("orderService");
   
    

builder.AddProject<Projects.ECommerceApp_Payment_Api>("paymentService");

builder.AddProject<Projects.ECommerceApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(redis)
    .WaitFor(redis)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
