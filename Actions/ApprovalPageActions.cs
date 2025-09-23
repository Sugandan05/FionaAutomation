using FionaAutomation.Pages;
using Microsoft.Playwright;

namespace FionaAutomation.Actions
{
    public class ApprovalPageActions
    {
        private readonly ApprovalPage _locators;

        public ApprovalPageActions(ApprovalPage locators)
        {
            _locators = locators;
        }
        public async Task ClickAdhocPayments() => await _locators.btnAdhocPayments.ClickAsync();
        public async Task ClickApprovalspage() => await _locators.btnApprovalpage.ClickAsync();
        public async Task ClickApprove() => await _locators.btnApprove.ClickAsync();
        public async Task ClickReject() => await _locators.btnReject.ClickAsync();
        public async Task ClickSelectRow() => await _locators.btnSelectRow.ClickAsync();
        public async Task ClickSelectAllRows() => await _locators.btnSelectAllRow.ClickAsync();
        public async Task EnterRejectReason(string reason) => await _locators.txtRejectReason.FillAsync(reason);
        public async Task ClickRejectRequest() => await _locators.btnRejectRequest.ClickAsync();
        public async Task ClickApproveAll() => await _locators.btnApproveAll.ClickAsync();
        public async Task ClickRejectAll() => await _locators.btnRejectAll.ClickAsync();
        public async Task EnterBulkRejectReason(string reason) => await _locators.txtBulkRejectReason.FillAsync(reason);
        public async Task ClickProceed() => await _locators.btnProceed.ClickAsync();
        public async Task<int> GetDisplayedCount() => int.Parse(await _locators.txtPendingCount.InnerTextAsync());
        public ILocator ToastMessage => _locators.toastMessage;

    }
}