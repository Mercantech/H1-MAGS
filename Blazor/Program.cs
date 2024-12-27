using Blazor.Components;
using Blazor.Services;

namespace Blazor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddScoped<DBService>(sp => new DBService(builder.Configuration.GetConnectionString("DefaultConnection")));

        var dbService = builder.Services.BuildServiceProvider().GetService<DBService>();
        if (!dbService.TestConnection())
        {
            Console.WriteLine("Kunne ikke oprette forbindelse til databasen");
        } else {
            Console.WriteLine("Forbindelse til database oprettet succesfuldt");
        }


        // Opsæt tabellerne
        dbService.ExecuteSqlFileAsync("SQL-Scripts/User.sql");

        builder.Services.AddBlazorBootstrap();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
