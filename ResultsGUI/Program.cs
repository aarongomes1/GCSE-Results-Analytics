using ResultsGUI.Components;
using Syncfusion.Blazor;

namespace ResultsGUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Expected 2 parameters:");
                Console.WriteLine("1) Path to the db");
                Console.WriteLine("2) Path to the licence file");
                return;
            }

            var databaseFilePath = args[0];
            var licenceFilePath = args[1];

            var syncfusionLicence = File.ReadLines(licenceFilePath).Single();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicence);

            var builder = WebApplication.CreateBuilder(args);

            var results = ResultsModels.Structure.Results.Load(databaseFilePath);

            builder.Services.AddScoped(provider => results);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddSyncfusionBlazor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
