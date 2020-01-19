using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using IdentityModel.OidcClient.Browser;

namespace RedirectServer.Browser
{
    /// <summary>
    /// https://github.com/IdentityModel/IdentityModel.OidcClient.Samples/blob/master/NetCoreConsoleClient/src/NetCoreConsoleClient/SystemBrowser.cs
    /// </summary>
    public class SystemBrowser 
    {
        public int Port { get; set; }

        private readonly string _path;

        public SystemBrowser(int? port = null, string path = null)
        {
            _path = path;

            if (!port.HasValue)
            {
                Port = GetRandomUnusedPort();
            }
            else
            {
                Port = port.Value;
            }
        }

        private int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);

            listener.Start();

            var port = ((IPEndPoint) listener.LocalEndpoint).Port;

            listener.Stop();

            return port;
        }

        public async Task<IdentityModel.OidcClient.Browser.BrowserResult> InvokeAsync(BrowserOptions options)
        {
            var listener = new LoopbackHttpListener(Port, _path);
            OpenBrowser(options.StartUrl);

            try
            {
                var result = await listener.WaitForCallbackAsync();

                if (string.IsNullOrWhiteSpace(result))
                {
                    return new IdentityModel.OidcClient.Browser.BrowserResult
                    {
                        ResultType = BrowserResultType.UnknownError,
                        Error = "Empty response"
                    };
                }

                return new IdentityModel.OidcClient.Browser.BrowserResult
                {
                    Response = result,
                    ResultType = BrowserResultType.Success
                };
            }
            catch (TaskCanceledException ex)
            {
                return new IdentityModel.OidcClient.Browser.BrowserResult
                {
                    ResultType = BrowserResultType.Timeout,
                    Error = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new IdentityModel.OidcClient.Browser.BrowserResult
                {
                    ResultType = BrowserResultType.UnknownError,
                    Error = ex.Message
                };
            }
        }

        public static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");

                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") {CreateNoWindow = true});
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}