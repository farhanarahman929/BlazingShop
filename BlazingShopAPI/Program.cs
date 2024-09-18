using BlazingShop.Shared.Services; // Update this to match your shared services namespace
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
       .AddNewtonsoftJson(); // Add JSON serialization with Newtonsoft if needed

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure BlazingShopServices with dependency injection
builder.Services.AddScoped<IBlazingShopServices, BlazingShopServices>();

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();

                          // Uncomment and use specific origins if needed
                          // policy.WithOrigins("https://example.com", "http://example.net")
                          //       .AllowAnyHeader()
                          //       .AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlazingShop API v1"));
}

app.UseCors("AllowAll"); // Apply CORS policy
app.UseHttpsRedirection();
app.UseAuthorization(); // Enable if you plan to use authorization

app.MapControllers(); // Map API controllers
// Add this if you have Razor Pages in your project

app.Run();
