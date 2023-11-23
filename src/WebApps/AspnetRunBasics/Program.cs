using AspnetRunBasics.Data;
using AspnetRunBasics.Extensions;
using AspnetRunBasics.Repositories;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//// use in-memory database
//services.AddDbContext<AspnetRunContext>(c =>
//    c.UseInMemoryDatabase("AspnetRunConnection"));

// add database dependecy
builder.Services.AddDbContext<AspnetRunContext>(c =>
    c.UseSqlServer(builder.Configuration.GetConnectionString("AspnetRunConnection")));

builder.Services.SeedDatabase();

// add repository dependecy
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
