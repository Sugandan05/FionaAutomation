using FionaAutomation.Utils;
using FionaAutomation.Reports;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
                // ✅ Create a single extent test for this case
                var extentTestName = !string.IsNullOrEmpty(testCase.TestCaseName)
                    ? $"{testCase.TestCaseID} - {testCase.TestCaseName}"
                    : testCase.TestCaseID;

                var extentTest = ExtentReportManager.CreateTest(extentTestName);

                try
                {
                    var method = assembly.GetTypes()
                        .Where(t => t.IsClass && t.Namespace == "FionaAutomation.Tests")
                        .Select(t => t.GetMethod(testCase.MethodName, BindingFlags.Public | BindingFlags.Static))
                        .FirstOrDefault(m => m != null);

                    if (method == null)
                    {
                        extentTest.Fail("Method not found");
                        _excel.WriteResult(testCase.TestCaseID, "FAIL - Method not found");
                        continue;
                    }

                    var result = method.Invoke(null, null);

                    if (result is Task task)
                        await task;

                    extentTest.Pass($"{testCase.TestCaseID} passed");
                    _excel.WriteResult(testCase.TestCaseID, "PASS");
                }
                catch (Exception ex)
                {
                    extentTest.Fail("Test case failed: " + ex.GetBaseException().Message);
                    _excel.WriteResult(testCase.TestCaseID, "FAIL - " + ex.GetBaseException().Message);
                }
            }
        }
    }
}
