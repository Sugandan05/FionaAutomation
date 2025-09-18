using Microsoft.Playwright;
using System.Threading.Tasks;

namespace FionaAutomation.Drivers
{
    public static class PlaywrightDriver
    {
        private static IPlaywright _playwright;
        private static IBrowser _browser;
        private static IPage _page;

        // Launch Chrome with full-screen rendering using --start-maximized
        public static async Task<IPage> GetPageAsync(bool headed = true, bool useProfile = false, string profilePath = "C:\\Temp\\ChromeProfile")
        {
            if (_playwright == null)
            {
                _playwright = await Playwright.CreateAsync();
            }

            if (_browser == null)
            {
                var launchOptions = new BrowserTypeLaunchOptions
                {
                    Headless = !headed,
                    Channel = "chrome",  // Ensures real Chrome, not Chromium
                    //Args = new[] { "--start-maximized" },
                    SlowMo = 50
                };

                // If you want persistent profile data
                if (useProfile)
                {
                    // Launch with persistent context (saves cookies, sessions, etc.)
                    var context = await _playwright.Chromium.LaunchPersistentContextAsync(profilePath, new BrowserTypeLaunchPersistentContextOptions
                    {
                        Headless = !headed,
                        Channel = "chrome",
                       // Args = new[] { "--start-maximized" }
                    });

                    _page = await context.NewPageAsync();
                    return _page;
                }

                // Regular browser (fresh profile each run)
                _browser = await _playwright.Chromium.LaunchAsync(launchOptions);
            }

            // Create context with no fixed viewport for true maximized mode
            var newContext = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = null
            });

            _page = await newContext.NewPageAsync();
            return _page;
        }

        public static IPage CurrentPage => _page;
        // Close the browser after tests
        public static async Task CloseBrowserAsync()
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
                _browser = null;
                _playwright.Dispose();
                _playwright = null;
            }
        }
    }
}
