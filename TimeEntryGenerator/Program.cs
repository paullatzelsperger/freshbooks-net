using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FreshbooksApiClient.Api;
using FreshbooksApiClient.Auth;
using FreshbooksApiClient.Contracts;
using FreshbooksApiClient.Rest;
using FreshbooksTimeEntryGenerator.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RedirectServer.Browser;

namespace FreshbooksTimeEntryGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var host = new HostBuilder()
                    .ConfigureHostConfiguration(configHost =>
                    {
                        configHost.SetBasePath(Directory.GetCurrentDirectory());
                        configHost.AddJsonFile("hostsettings.json", optional: true);

                        if (args != null)
                        {
                            configHost.AddCommandLine(args);
                        }
                    })
                    .ConfigureAppConfiguration((hostContext, configApp) =>
                    {
                        configApp.SetBasePath(Directory.GetCurrentDirectory());
                        configApp.AddJsonFile("appsettings.json", optional: false);
                        configApp.AddJsonFile("appsettings.local.json", optional: true);

                        if (args != null)
                        {
                            configApp.AddCommandLine(args);
                        }
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddOptions();

                        var freshbooksSection = hostContext.Configuration.GetSection("Freshbooks");
                        services.Configure<FreshbooksAuthSettings>(freshbooksSection.GetSection("Auth"));
                        services.Configure<FreshbooksAppSettings>(freshbooksSection.GetSection("App"));

                        services.AddSingleton<IFreshbooksAuthorizer, FreshbooksAuthorizerImpl>();

                        services.AddSingleton<IRestClientFactory, RestClientFactoryImpl>();
                        services.AddSingleton<IFreshbooksApi, FreshbooksApiImpl>();
                        services.AddSingleton<SystemBrowser, SystemBrowser>();
                        services.AddSingleton<ITokenStore<FreshbooksTokens>, FileBasedTokenStore<FreshbooksTokens>>();
                    })
                    .ConfigureLogging((hostContext, configLogging) =>
                    {
                        configLogging.AddConsole();
                        configLogging.AddDebug();
                    })
                    .UseConsoleLifetime()
                    .Build();


                // authentication credentials for Canto should be stored in appsettings.XXX.json
                var freshbooksApi = host.Services.GetService<IFreshbooksApi>();
                await freshbooksApi.Authenticate();
                var id = await freshbooksApi.GetIdentityInfo();
                var businessId = id.response.business_memberships.ElementAt(0).business.id.ToString();
                var timeEntryFilter = TimeEntryFilter.CurrentMonth;
                var entries = (await freshbooksApi.GetTimeEntries(timeEntryFilter, businessId)).TimeEntries;

                foreach (var entry in entries)
                {
                    var start = entry.StartedAt.ToLocalTime();
                    var end = entry.StartedAt.Add(TimeSpan.FromSeconds((double) entry.Duration));
                    var dur = TimeSpan.FromSeconds((double) entry.Duration);
                    Console.WriteLine($"From {start:R} to {end:R}, took {dur:g} for Project {entry.ProjectId}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured: {e}");
            }
        }
    }
}