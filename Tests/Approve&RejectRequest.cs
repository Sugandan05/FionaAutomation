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
    public class ApproveRequest
    {
        private CreateRequestPageActions _createRequestActions;
        private ApprovalPageActions _approvalActions;
        private CreateRequestData testData;
        private string filePath;
        private IPage _page;

        public ApproveRequest()
        {
            _page = PlaywrightDriver.CurrentPage;
            var approvalPage = new ApprovalPage(_page);
            _approvalActions = new ApprovalPageActions(approvalPage);
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));
            filePath = Path.Combine(projectRoot, "Config", "TestData.xlsx");
            testData = ExcelDataHelper.GetCreateRequestData(filePath, "Sheet1").First();
        }

        public async Task<bool> ApproveRequestAndVerifyTabAsync(string requestId)
        {
            // Open Approvals page
            await _approvalActions.ClickAdhocPayments();
            await _approvalActions.ClickApprovalspage();

            // Approve the request
            await _approvalActions.ClickApprove();
            await Task.Delay(2000);
            await _approvalActions.ClickApprove();

            // Assert toast message
            var toastMessage = _approvalActions.ToastMessage;
            string actualtoaster = await toastMessage.InnerTextAsync();
            string expectedtoaster = "Request Approved Successfully";
            Assert.That(actualtoaster, Is.EqualTo(expectedtoaster), "Toast message mismatch");

            // Switch to Approved tab (assuming tab text is 'APPROVALS')
            var approvedTab = _page.Locator("button:has-text('APPROVALS')");
            await approvedTab.ClickAsync();
            await Task.Delay(1000);

            // Check if the requestId is present in the Approved tab grid
            var approvedRequest = _page.Locator($"text={requestId}");
            bool isPresent = await approvedRequest.IsVisibleAsync();
            return isPresent;
        }

        public async Task RequestApprove()
        {
            await _approvalActions.ClickAdhocPayments();
            await _approvalActions.ClickApprovalspage();

            // Get initial Pending count
            int initialPendingCount = await _approvalActions.GetTabCountAsync("PENDING");
            // Approve the first request
            await _approvalActions.ClickApprove();
            await Task.Delay(2000);

            // Assert toast message
            var toastMessage = _approvalActions.ToastMessage;
            string actualtoaster = await toastMessage.InnerTextAsync();
            string expectedtoaster = "Request Approved Successfully";
            Assert.That(actualtoaster, Is.EqualTo(expectedtoaster), "Toast message mismatch");

            // Switch to Approved tab
            await _approvalActions.ClickTab("APPROVALS");
            await Task.Delay(1000);

            // Check if the request is present in Approved tab (assume first request's ID is available)
            string requestId = await _approvalActions.GetFirstRequestIdAsync();
            bool isPresent = await _approvalActions.IsRequestPresentAsync(requestId);
            Assert.That(isPresent, Is.True, $"Request {requestId} not found in APPROVALS tab");

            // Switch back to Pending tab and verify count reduced
            // await _approvalActions.ClickTab("PENDING");
            // await Task.Delay(1000);
            int updatedPendingCount = await _approvalActions.GetTabCountAsync("PENDING");
            Assert.That(updatedPendingCount, Is.EqualTo(initialPendingCount - 1), "Pending count did not reduce after approval");
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

        public async Task RejectRequest()
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

        public async Task<bool> VerifyApproveRejectAllButtons()
        {
            // Navigate to Ad-hoc payments > Approvals
            await _approvalActions.ClickAdhocPayments();
            await _approvalActions.ClickApprovalspage();
            // Click the multi-select checkbox at the top of the grid using action
            await _approvalActions.ClickMultiSelectCheckbox();
            // Check for Approve All and Reject All buttons using actions
            bool approveAllVisible = await _approvalActions.IsApproveAllVisible();
            bool rejectAllVisible = await _approvalActions.IsRejectAllVisible();
            return approveAllVisible && rejectAllVisible;
        }
    }
}