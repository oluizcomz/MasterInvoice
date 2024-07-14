using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Collections.Generic;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<ContextBase>(options =>
               options.UseSqlServer(
                   builder.Configuration.GetConnectionString("DefaultConnection")));

// Interface and Repository
builder.Services.AddSingleton(typeof(IGeneric<>), typeof(RepositoryGenerics<>));



// Servece and Domain
builder.Services.AddSingleton<IInvoice, RepositoryInvoice>();

var app = builder.Build();
DatabaseManagementService.MigrationInitialisation(app);
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
    pattern: "{controller=Invoices}/{action=Index}/{id?}");

app.Run();
