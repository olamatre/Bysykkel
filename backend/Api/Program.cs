using Api.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi("v1");
builder.Services
    .AddOptions<BikeshareClientOptions>()
    .BindConfiguration("Bikeshare")
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddHttpClient<IBikeshareClient, BikeshareClient>((serviceProvider, httpClient) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<BikeshareClientOptions>>().Value;
    httpClient.BaseAddress = new Uri(options.BaseUrl);
    httpClient.DefaultRequestHeaders.Add("Client-Identifier", options.ClientIdentifier);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Bysykkel API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

var bikeshare = app.MapGroup("/bikeshare");
bikeshare.MapGet("/stasjoner", (IBikeshareClient client, CancellationToken cancellationToken) =>
    client.GetStationInformationAsync(cancellationToken));
bikeshare.MapGet("/stasjon-status", (IBikeshareClient client, CancellationToken cancellationToken) =>
    client.GetStationStatusAsync(cancellationToken));

app.Run();
