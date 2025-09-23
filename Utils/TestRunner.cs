using FionaAutomation.Utils;
using FionaAutomation.Reports;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Playwright;
using System.Collections.Generic;

namespace FionaAutomation
{
    public class TestRunner
    {
        private static ExcelHelper _excel;

        [OneTimeSetUp]
        public void Initialize()
        {
            _excel = new ExcelHelper(@"Config\Testcases.xlsx");
        }

        [Test]
        public async Task RunTestsFromExcel()
        {
            var testCases = _excel.GetTests();
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var testCase in testCases.Where(tc => tc.Execute))
            {
                var extentTestName = !string.IsNullOrEmpty(testCase.TestCaseName)
                    ? $"{testCase.TestCaseID} - {testCase.TestCaseName}"
                    : testCase.TestCaseID;

                var extentTest = ExtentReportManager.CreateTest(extentTestName);

                object classInstance = null;

                try
                {
                    // Find the test method dynamically
                    var method = assembly.GetTypes()
                        .Where(t => t.IsClass && t.Namespace == "FionaAutomation.Tests")
                        .Select(t => t.GetMethod(testCase.MethodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                        .FirstOrDefault(m => m != null);

                    if (method == null)
                    {
                        extentTest.Fail("Method not found");
                        _excel.WriteResult(testCase.TestCaseID, "FAIL - Method not found");
                        continue;
                    }

                    if (!method.IsStatic)
                    {
                        // Create fresh instance per test
                        classInstance = Activator.CreateInstance(method.DeclaringType);

                        // Initialize Playwright page if test class has InitPageAsync
                        var initMethod = classInstance.GetType().GetMethod("InitPageAsync");
                        if (initMethod != null)
                        {
                            var browserProp = classInstance.GetType().GetProperty("Browser");
                            IBrowser browser = browserProp?.GetValue(classInstance) as IBrowser;
                            var initTask = initMethod.Invoke(classInstance, new object[] { browser }) as Task;
                            if (initTask != null) await initTask;
                        }
                    }

                    // Invoke test method
                    var result = method.Invoke(classInstance, null);
                    if (result is Task methodTask) await methodTask;

                    extentTest.Pass($"{testCase.TestCaseID} passed");
                    _excel.WriteResult(testCase.TestCaseID, "PASS");
                }
                catch (TargetInvocationException ex)
                {
                    await HandleTestExceptionAsync(ex.InnerException ?? ex, classInstance, testCase.TestCaseID, extentTest);
                }
                catch (Exception ex)
                {
                    await HandleTestExceptionAsync(ex, classInstance, testCase.TestCaseID, extentTest);
                }
            }
        }

        private static async Task HandleTestExceptionAsync(Exception ex, object classInstance, string testCaseID, dynamic extentTest)
        {
            IPage page = null;

            // Retrieve the IPage from test class
            if (classInstance != null)
            {
                var pageProp = classInstance.GetType().GetProperty("Page");
                if (pageProp != null)
                    page = pageProp.GetValue(classInstance) as IPage;
            }

            string screenshotPath = null;

            if (page != null)
            {
                // Use ScreenshotHelper to capture screenshot
                screenshotPath = await ScreenshotHelper.CaptureScreenshotAsync(page, testCaseID);

                // Attach to ExtentReports
                ExtentReportManager.AttachScreenshot(screenshotPath);
            }

            if (ex is AssertionException assertionEx)
            {
                extentTest.Fail("Assertion failed: " + assertionEx.Message);
                _excel.WriteResult(testCaseID, "FAIL - " + assertionEx.Message);

                // Use ScreenshotHelper to capture screenshot
                screenshotPath = await ScreenshotHelper.CaptureScreenshotAsync(page, testCaseID);

                // Attach to ExtentReports
                ExtentReportManager.AttachScreenshot(screenshotPath);
            }
            else
            {
                extentTest.Fail("Unexpected error: " + ex.Message);
                _excel.WriteResult(testCaseID, "FAIL - " + ex.Message);

                // Use ScreenshotHelper to capture screenshot
                screenshotPath = await ScreenshotHelper.CaptureScreenshotAsync(page, testCaseID);

                // Attach to ExtentReports
                ExtentReportManager.AttachScreenshot(screenshotPath);
            }
        }
    }
}
