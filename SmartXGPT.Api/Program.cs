using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartXGPT.Infrastructure.APIServices;
using SmartXGPT.Infrastructure.Contexts.SmartXDbContext;
using SmartXGPT.Service.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(Options => {
    Options.UseSqlServer(builder.Configuration.GetConnectionString("SmartXAppDbConectionString"));
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IManageMessageService, ManageMessageService>();
builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<IGPTAPIService, GPTAPIService>();
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddAutoMapper(Assembly.Load("SmartXGPT.Infrastructure"));



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
