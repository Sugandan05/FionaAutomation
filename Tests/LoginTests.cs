using FionaAutomation.Drivers;
using FionaAutomation.Reports;
using FionaAutomation.Utils;
using FionaAutomation.Actions;
using FionaAutomation.Pages;
using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FionaAutomation.Tests
{


    public class LoginTests : BaseTest
    {
        private LoginPageActions _loginActions;

        public LoginTests()
        {
            var page = PlaywrightDriver.GetPageAsync().Result;
            var loginPage = new LoginPage(page);
            _loginActions = new LoginPageActions(loginPage);
        }

        public async Task TestValidLogin()
        {
            //var test = ExtentReportManager.CreateTest("Login Test");
            var config = ConfigReader.GetConfig();
            string baseUrl = config["baseUrl"]!;
            string browser = config["browser"]!;
            bool headed = bool.Parse(config["headed"]!);
            string username = config["TestSettings:ValidUser:Username"]!;
            string password = config["TestSettings:ValidUser:Password"]!;

            var page = await PlaywrightDriver.GetPageAsync(headed: true);
            var loginPage = new LoginPage(page);
            var loginActions = new LoginPageActions(loginPage);

            // await TestStepHelper.RunValidation(
            //     "58023",
            //     "Check if the user can navigate to the ad hoc payment requests section of the web application.",
            //     async () =>
            //     {
            //         await page.GotoAsync(baseUrl);
            //         ExtentReportManager.LogInfo("Browser launched successfully");

            //         bool isMicrosoftLogin = await loginActions.IsMicrosoftLoginPageAsync();
            //         Assert.That(isMicrosoftLogin, Is.True, "Microsoft login page did not appear.");

            //         ExtentReportManager.LogPass("Microsoft login page appeared successfully");
            //     });
            await page.GotoAsync(baseUrl);
            await loginActions.ClickLogin();
            ExtentReportManager.LogInfo("Clicked login button");
            await loginActions.EnterUsername(username);
            await loginActions.ClickNextButton();
            await loginActions.EnterPassword(password);
            await loginActions.ClickSigninButton();
            await loginActions.ClickBackButton();


            //ScreenshotHelper.CaptureScreenshotAsync(page, "TestValidLogin").Wait();
            await Task.Delay(1000);
            // try
            // {
            //     string actualtitle = await page.TitleAsync();
            //     string expectedtitle = "Fiona";

            //     Assert.That(actualtitle, Is.EqualTo(expectedtitle), "Title mismatch");

            //     // If we reach here, test passed
            //     ExtentReportManager.LogPass("Login successful");
            // }
            // catch (AssertionException ex)
            // {
            //     // If assertion fails, log fail
            //     string screenshotPath = await ScreenshotHelper.CaptureScreenshotAsync(page, "TestValidLogin");
            //     ExtentReportManager.LogFail("Login failed: " + ex.Message);
            //     ExtentReportManager.AttachScreenshot(screenshotPath);

            //     throw; // rethrow so NUnit marks it as failed
            // }
        }
    }
}
