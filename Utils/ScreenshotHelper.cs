using Microsoft.Playwright;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FionaAutomation.Utils
{
    public static class ScreenshotHelper
    {
        public static async Task<string> CaptureScreenshotAsync(IPage page, string testName)
        {
            if (page == null)
            {
                Console.WriteLine($"[ScreenshotHelper] Page is null, cannot capture screenshot for {testName}");
                return string.Empty;
            }

            try
            {
                // Go up from bin/Debug/netX.Y to project root
                string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));

                // Create Screenshots folder in project root
                string screenshotsDir = Path.Combine(projectRoot, "Screenshots");
                Directory.CreateDirectory(screenshotsDir);

                // Unique file name with timestamp
                string filePath = Path.Combine(screenshotsDir, $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");

                await page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = filePath,
                    FullPage = true
                });

                Console.WriteLine($"[ScreenshotHelper] Screenshot saved at: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ScreenshotHelper] Failed to capture screenshot for {testName}: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
