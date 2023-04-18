using AutotestWeb.Models;
using AutotestWeb.Models.Services;
using AutotestWeb.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<TicketRepository>();

builder.Services.AddScoped<CorrectAnswerRepository>();
builder.Services.AddScoped<InCorrectAnswerRepository>();

builder.Services.AddTransient<UsersService>();
builder.Services.AddTransient<TicketsService>();

builder.Services.AddSingleton<QuestionsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
