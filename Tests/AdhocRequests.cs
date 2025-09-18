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
    public class AdhocRequests
    {
        private CreateRequestPageActions _createRequestActions;

        public AdhocRequests()
        {
            var page = PlaywrightDriver.GetPageAsync().Result;
            var createRequestPage = new CreateRequestPage(page);
            _createRequestActions = new CreateRequestPageActions(createRequestPage);
        }

        public static async Task CreateNewRequest()
        {
            var config = ConfigReader.GetConfig();
            //var test = ExtentReportManager.CreateTest("Create New Request Test");

            var page = PlaywrightDriver.CurrentPage; // Reuse existing page

            var createRequestPage = new CreateRequestPage(page);
            var createRequestActions = new CreateRequestPageActions(createRequestPage);

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));
            var filePath = Path.Combine(projectRoot, "Config", "TestData.xlsx");
            var testData = ExcelDataHelper.GetCreateRequestData(filePath, "Sheet1").First();

            await createRequestActions.ClickCreateRequest();
            await createRequestActions.EnterDate(testData.PaymentDueBy);
            await createRequestActions.SelectPaymentType(testData.PaymentType);
            await Task.Delay(1000);
            await createRequestActions.SelectPaymentRegion(testData.PaymentRegion);
            await Task.Delay(1000);
            await createRequestActions.SelectAccountType(testData.AccountType);
            await createRequestActions.SelectAccount(testData.AccountName);
            await Task.Delay(3000);
            await createRequestActions.EnterBeneficiaryName(testData.BeneficiaryName);
            await createRequestActions.EnterSortCode(testData.SortCode);
            await createRequestActions.EnterAccountNumber(testData.AccountNumber);
            await createRequestActions.EnterNetValue(testData.NetValue);
            await Task.Delay(1000);
            await createRequestActions.SelectCurrency(testData.Currency);
            await Task.Delay(3000);
            await createRequestActions.SelectEntity(testData.Entity);
            await Task.Delay(1000);
            await createRequestActions.EnterNominalAccount(testData.NominalAccount);
            await createRequestActions.EnterDescription(testData.Description);
            await createRequestActions.ClickSubmitRequest();
try
{
            var toastMessage = createRequestPage.toastMessage;
            string actualtoaster = await toastMessage.InnerTextAsync();
            string expectedtoaster = "Request Created Successfully..";

                Assert.That(actualtoaster, Is.EqualTo(expectedtoaster), "Toast message mismatch");

                // If we reach here, test passed
                ExtentReportManager.LogPass("Request Created Successfully");
            }
            catch (AssertionException ex)
            {
                // If assertion fails, log fail
                string screenshotPath = await ScreenshotHelper.CaptureScreenshotAsync(page, "CreateNewRequest");
                //ExtentReportManager.LogFail("Create Request failed: " + ex.Message);
                ExtentReportManager.AttachScreenshot(screenshotPath);
                throw; 
            }
           
        }

    }
}