using NUnit.Framework;
using FionaAutomation.Reports;
using System;
using System.IO;

[SetUpFixture]
public class TestSuiteSetup
{
   [OneTimeSetUp]
public static void GlobalSetup()
{
    string folder = @"C:\Users\Sugandan\FionaAutomation\ExtentReports";
    if (!Directory.Exists(folder))
        Directory.CreateDirectory(folder);

    string reportPath = Path.Combine(folder, "TestReport.html");
    ExtentReportManager.InitReport(reportPath);
}


    [OneTimeTearDown]
    public void GlobalTearDown()
    {
        ExtentReportManager.FlushReport();
    }
}
