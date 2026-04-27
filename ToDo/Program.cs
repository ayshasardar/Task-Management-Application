using Microsoft.EntityFrameworkCore;
using ToDoDemo.ML.Services;
using ToDoDemo.Models;
using ToDoDemo.Services.Implementations;
using ToDoDemo.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add EF core DI
builder.Services.AddDbContext<ToDoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ToDoContext")));
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddSingleton<MLPriorityService>();//singleton - because model is heavy and should be loaded once

var app = builder.Build();

// Train ML model once at startup
using (var scope = app.Services.CreateScope())
{
    var mlService = scope.ServiceProvider.GetRequiredService<MLPriorityService>();
    mlService.TrainModel();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
