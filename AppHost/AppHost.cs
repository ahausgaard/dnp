var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.WebAPI>("webapi");

builder.AddProject<Projects.BlazorApp>("blazorapp")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
