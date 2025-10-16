using System;

namespace FionaAutomation.Utils
{
    public static class TestStepLogger
    {
        private static ExcelHelper _excelHelper;

        // Initialize once before all tests (we'll show where in Step 4)
        public static void Initialize(string relativeFilePath)
        {
            _excelHelper = new ExcelHelper(relativeFilePath);
        }

        // Log individual test case results
        public static void LogResult(string testCaseId, string description, bool isPass, string message = "")
        {
            if (_excelHelper == null)
                throw new InvalidOperationException("TestStepLogger not initialized. Call Initialize() before logging results.");

            string result = isPass ? "Pass" : $"Fail - {message}";
            _excelHelper.WriteResult(testCaseId, result);

            Console.WriteLine($"{testCaseId}: {description} => {result}");
        }
    }
}
