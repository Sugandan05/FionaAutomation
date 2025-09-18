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

            return filePath;
        }
    }
}
