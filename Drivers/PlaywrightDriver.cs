using Microsoft.Playwright;
using System.Threading.Tasks;

namespace FionaAutomation.Drivers
{
    public static class PlaywrightDriver
    {
        private static IPlaywright _playwright;
        private static IBrowser _browser;
        private static IBrowserContext _context;
        private static IPage _page;

        public static async Task<IPage> GetPageAsync(
            bool headed = true,
            bool useProfile = false,
            string profilePath = "C:\\Temp\\ChromeProfile")
        {
            if (_page != null) 
                return _page; // ✅ Reuse existing page

            if (_playwright == null)
                _playwright = await Playwright.CreateAsync();

            if (useProfile)
            {
                // ✅ Launch persistent context only once
                _context = await _playwright.Chromium.LaunchPersistentContextAsync(profilePath,
                    new BrowserTypeLaunchPersistentContextOptions
                    {
                        Headless = !headed,
                        Channel = "chrome",
                        ViewportSize = null
                    });

                _page = _context.Pages.Count > 0 ? _context.Pages[0] : await _context.NewPageAsync();
            }
            else
            {
                if (_browser == null)
                {
                    _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                    {
                        Headless = !headed,
                        Channel = "chrome",
                        Args = new[] { "--window-size=1920,1080" }
                    });
                }

                if (_context == null)
                {
                    _context = await _browser.NewContextAsync(new BrowserNewContextOptions
                    {
                        ViewportSize = null
                    });
                }

                _page = await _context.NewPageAsync();
            }

            // ✅ Apply zoom only once when creating page
            await _page.EvaluateAsync("() => { document.body.style.zoom = '60%'; }");

            return _page;
        }

        public static IPage CurrentPage => _page;

        public static bool IsInitialized => _page != null;

        public static async Task CloseBrowserAsync()
        {
            if (_context != null)
            {
                await _context.CloseAsync();
                _context = null;
            }

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

            _page = null;
        }
    }
}
