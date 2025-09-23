using Microsoft.Playwright;
using System.Threading.Tasks;

namespace FionaAutomation.Drivers
{
    public static class PlaywrightDriver
    {
        private static IPlaywright _playwright;
        private static IBrowser _browser;
        private static IPage _page;

        public static async Task<IPage> GetPageAsync(bool headed = true, bool useProfile = false, string profilePath = "C:\\Temp\\ChromeProfile")
        {
            if (_playwright == null)
                _playwright = await Playwright.CreateAsync();

            if (_browser == null && !useProfile)
            {
                // Launch browser maximized
                _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = !headed,
                    Channel = "chrome",
                   // Args = new[] { "--start-maximized" }
                    Args = new[] { "--window-size=1920,1080" }
                });
            }

            if (useProfile)
            {
                var context = await _playwright.Chromium.LaunchPersistentContextAsync(profilePath,
                    new BrowserTypeLaunchPersistentContextOptions
                    {
                        Headless = !headed,
                        Channel = "chrome",
                        ViewportSize = null
                        //ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
                    });

                _page = context.Pages.Count > 0 ? context.Pages[0] : await context.NewPageAsync();
            
            }
            else
            {
                var newContext = await _browser.NewContextAsync(new BrowserNewContextOptions
                {
                    ViewportSize = null
                    //ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
                });

                _page = await newContext.NewPageAsync();
            }

            // Reset zoom level to 60%
            await _page.EvaluateAsync("() => { document.body.style.zoom = '60%'; }");

            return _page;
        }

        public static IPage CurrentPage => _page;

        public static async Task CloseBrowserAsync()
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
                _browser = null;
            }

            if (_playwright != null)
            {
                _playwright.Dispose();
                _playwright = null;
            }
        }
    }
}
