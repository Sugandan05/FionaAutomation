using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace FionaAutomation.Reports
{

    public static class ExtentReportManager
    {
        private static ExtentReports? _extent;

        public static void InitReport(string reportPath)
        {
            var dir = Path.GetDirectoryName(reportPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var spark = new ExtentSparkReporter(reportPath);
            _extent = new ExtentReports();
            _extent.AttachReporter(spark);
        }

        public static ExtentTest CreateTest(string testName)
        {
            return _extent.CreateTest(testName);
        }
        public static void LogPass(ExtentTest test, string message)
        {
            test.Pass(message);
        }

        public static void LogInfo(ExtentTest test, string message)
        {
            test.Info(message);
        }

        public static void LogFail(ExtentTest test, string message)
        {
            test.Fail(message);
        }


        public static void AttachScreenshot(ExtentTest test, string relativePath)
        {
            test.AddScreenCaptureFromPath(relativePath);
        }

        public static void FlushReport()
        {
            _extent.Flush();
        }
    }
}