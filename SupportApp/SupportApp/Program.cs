using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SupportApp.Services;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Add services to the container
// --------------------
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Register ISupportService as singleton
builder.Services.AddSingleton<ISupportService, SupportService>();

// --------------------
// Optional: If you want to use CosmosClient directly elsewhere
// --------------------
builder.Services.AddSingleton(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration["CosmosDb:ConnectionString"];
    return new CosmosClient(connectionString);
});

var app = builder.Build();

// --------------------
// Configure the HTTP request pipeline
// --------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
