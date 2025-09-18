using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FionaAutomation.Tests
{
    public class BaseTest
    {
        protected static IPlaywright _playwright;
        protected static IBrowser _browser;
        protected static IBrowserContext _context;
        protected static IPage _page;

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        [OneTimeTearDown]
        public async Task GlobalTearDown()
        {
            await _context.CloseAsync();
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
