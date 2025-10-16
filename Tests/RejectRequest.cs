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
    public class RejectRequest
    {

        private string filePath;
        private IPage _page;
        private ApprovalPageActions _approvalActions;
        private CreateRequestData testData;


        public RejectRequest()
        {

            _page = PlaywrightDriver.CurrentPage;
            var approvalPage = new ApprovalPage(_page);
            _approvalActions = new ApprovalPageActions(approvalPage);

            
        }
        public async Task RequestReject()
        {

            await _approvalActions.ClickAdhocPayments();
            await _approvalActions.ClickApprovalspage();
            await _approvalActions.ClickReject();
            await Task.Delay(2000);
            await _approvalActions.EnterRejectReason();
            await _approvalActions.ClickRejectRequest();

            var toastMessage = _approvalActions.ToastMessage;
            string actualtoaster = await toastMessage.InnerTextAsync();
            string expectedtoaster = "Request Rejected Successfully";
            Assert.That(actualtoaster, Is.EqualTo(expectedtoaster), "Toast message mismatch");

        }
    }
}