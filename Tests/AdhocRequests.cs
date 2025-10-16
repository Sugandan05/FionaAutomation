using FionaAutomation.Drivers;
using FionaAutomation.Reports;
using FionaAutomation.Utils;
using FionaAutomation.Actions;
using FionaAutomation.Pages;
using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Playwright;


namespace FionaAutomation.Tests

{
    public class AdhocRequests
    {
        private CreateRequestPageActions _createRequestActions;
        private ApprovalPageActions _approvalActions;
        private CreateRequestData testData;
        private string filePath;
        private IPage _page;

        public AdhocRequests()
        {

            _page = PlaywrightDriver.CurrentPage;
            var createRequestPage = new CreateRequestPage(_page);
            _createRequestActions = new CreateRequestPageActions(createRequestPage, _page);
            var approvalPage = new ApprovalPage(_page);
            _approvalActions = new ApprovalPageActions(approvalPage);

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));
            filePath = Path.Combine(projectRoot, "Config", "TestData.xlsx");
            testData = ExcelDataHelper.GetCreateRequestData(filePath, "Sheet1").First();
        }

        public async Task CreateNewRequest()
        {


            await _createRequestActions.ClickCreateRequest();
            await _createRequestActions.ValidateRequestDetailsAsync();
            await _createRequestActions.EnterDate(testData.PaymentDueBy);
            await _createRequestActions.SelectPaymentType(testData.PaymentType);
            await Task.Delay(1000);
            await _createRequestActions.SelectPaymentRegion(testData.PaymentRegion);
            //await Task.Delay(1000);
            await _createRequestActions.SelectAccountType(testData.AccountType);
            await _createRequestActions.SelectAccount(testData.AccountName);
            //await Task.Delay(3000);
            await _createRequestActions.EnterBeneficiaryName(testData.BeneficiaryName);
            await _createRequestActions.EnterSortCode(testData.SortCode);
            await _createRequestActions.EnterAccountNumber(testData.AccountNumber);
            await _createRequestActions.EnterNetValue(testData.NetValue);
            //await Task.Delay(1000);
            await _createRequestActions.SelectCurrency(testData.Currency);
            //await Task.Delay(3000);
            await _createRequestActions.SelectEntity(testData.Entity);
            //await Task.Delay(1000);
            //await _createRequestActions.EnterNominalAccount(testData.NominalAccount1);

            // await _createRequestActions.EnterNominalAccount(testData.NominalAccount);
            await _createRequestActions.EnterDescription(testData.Description);
            await _createRequestActions.ClickSubmitRequest();

          await _createRequestActions.ValidateToastMessageAsync("Request Created Successfully");



        }

        public async Task EditRequest()
        {
            var testData = ExcelDataHelper.GetCreateRequestData(filePath, "Sheet2").First();
            await Task.Delay(3000);
            await _createRequestActions.ClickEditRequest();
            //await Task.Delay(1000);
            await _createRequestActions.SelectAccountType(testData.AccountType);
            await _createRequestActions.SelectAccount(testData.AccountName);
            //await Task.Delay(3000);
            await _createRequestActions.EnterBeneficiaryName(testData.BeneficiaryName);


        }
      

        

        
    }
}