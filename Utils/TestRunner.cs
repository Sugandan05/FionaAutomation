using FionaAutomation.Utils;
using FionaAutomation.Reports;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Playwright;
using System.IO;
using AventStack.ExtentReports;

namespace FionaAutomation
{
    public class TestRunner
    {
        private static ExcelHelper _excel;

        [OneTimeSetUp]
        public void Initialize()
        {
            string filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Config",
                "Testcases.xlsx"
            );

            _excel = new ExcelHelper(filePath);
            TestStepLogger.Initialize(filePath);
        }

        [Test]
        public async Task RunTestsFromExcel()
        {
            var testCases = _excel.GetTests();
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var testCase in testCases.Where(tc => tc.Execute))
            {
                string extentTestName = !string.IsNullOrEmpty(testCase.TestCaseName)
                    ? $"{testCase.TestCaseID} - {testCase.TestCaseName}"
                    : testCase.TestCaseID;

                ExtentTest extentTest = ExtentReportManager.CreateTest(extentTestName);

                object classInstance = null;

                try
                {
                    // Locate test method
                    var method = assembly.GetTypes()
                        .Where(t => t.IsClass && t.Namespace == "FionaAutomation.Tests")
                        .Select(t => t.GetMethod(
                            testCase.MethodName,
                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                        .FirstOrDefault(m => m != null);

                    if (method == null)
                    {
                        extentTest.Fail("Method not found");
                        _excel.WriteResult(testCase.TestCaseID, "FAIL - Method not found");
                        continue;
                    }

                    // Create fresh instance if non-static
                    if (!method.IsStatic)
                    {
                        classInstance = Activator.CreateInstance(method.DeclaringType);

                        var initMethod = classInstance.GetType().GetMethod("InitPageAsync");
                        if (initMethod != null)
                        {
                            var browserProp = classInstance.GetType().GetProperty("Browser");
                            IBrowser browser = browserProp?.GetValue(classInstance) as IBrowser;

                            var initTask = initMethod.Invoke(
                                classInstance,
                                new object[] { browser }) as Task;

                            if (initTask != null)
                                await initTask;
                        }
                    }

                    // Invoke test method
                    var result = method.Invoke(classInstance, null);
                    if (result is Task task)
                        await task;

                    extentTest.Pass($"{testCase.TestCaseID} passed");
                    _excel.WriteResult(testCase.TestCaseID, "PASS");
                }
                catch (TargetInvocationException ex)
                {
                    await HandleTestExceptionAsync(
                        ex.InnerException ?? ex,
                        classInstance,
                        testCase.TestCaseID,
                        extentTest);
                }
                catch (Exception ex)
                {
                    await HandleTestExceptionAsync(
                        ex,
                        classInstance,
                        testCase.TestCaseID,
                        extentTest);
                }
            }
        }

        private static async Task HandleTestExceptionAsync(
            Exception ex,
            object classInstance,
            string testCaseID,
            ExtentTest extentTest)
        {
            IPage page = null;

            if (classInstance != null)
            {
                var pageProp = classInstance.GetType().GetProperty("Page");
                page = pageProp?.GetValue(classInstance) as IPage;
            }

            string screenshotPath = page != null
                ? await ScreenshotHelper.CaptureScreenshotAsync(page, testCaseID)
                : null;

            if (ex is AssertionException assertionEx)
            {
                extentTest.Fail("Assertion failed: " + assertionEx.Message);
                _excel.WriteResult(testCaseID, "FAIL - " + assertionEx.Message);
            }
            else
            {
                extentTest.Fail("Unexpected error: " + ex.Message);
                _excel.WriteResult(testCaseID, "FAIL - " + ex.Message);
            }

            if (!string.IsNullOrEmpty(screenshotPath))
            {
                ExtentReportManager.AttachScreenshot(extentTest, screenshotPath);
            }
        }
    }
}
