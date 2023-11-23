using AspnetRunBasics.Services;
using AspnetRunBasics.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

Uri gatewayUri = new(builder.Configuration["ApiSettings:GatewayAddress"]);
builder.Services.AddHttpClient<ICatalogService, CatalogService>(config =>
{
    config.BaseAddress = gatewayUri;
});
builder.Services.AddHttpClient<IBasketService, BasketService>(config =>
{
    config.BaseAddress = gatewayUri;
});
builder.Services.AddHttpClient<IOrderService, OrderService>(config =>
{
    config.BaseAddress = gatewayUri;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
