using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace FionaAutomation.Reports
{
    public static class ExtentReportManager
    {
        private static ExtentReports? _extent;
        private static ExtentTest? _test;

        // Initialize Extent Report with custom file path
        public static void InitReport(string reportPath)
        {
            // Get the directory part only
            string dir = Path.GetDirectoryName(reportPath);

            // Ensure directory exists
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var sparkReporter = new ExtentSparkReporter(reportPath);
            _extent = new ExtentReports();
            _extent.AttachReporter(sparkReporter);
        }


        // Create a new test case node in the report
        public static ExtentTest CreateTest(string testName)
        {
            _test = _extent?.CreateTest(testName);
            return _test;
        }


        // Log a passed step
        public static void LogPass(string message)
        {
            _test?.Pass(message);
        }

         public static void LogInfo(string message)
        {
            _test?.Info(message);
        }

        // Log a failed step
        public static void LogFail(string message)
        {
            _test?.Fail(message);
        }

        // Save the report after execution 

        public static void FlushReport()
        {
            _extent?.Flush();
        }
        public static void AttachScreenshot(string relativePath)
        {
            if (_test != null && !string.IsNullOrEmpty(relativePath))
            {
                _test.AddScreenCaptureFromPath(relativePath);
            }
        }


    }
}
