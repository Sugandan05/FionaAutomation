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
        // Locators for tab, counts, request id, and request row
        private ILocator Tab(string tabName) => _locators._page.Locator($"tab:has-text('{tabName}')");
        private ILocator TabCount(string tabName) => _locators._page.Locator($"tab:has-text('{tabName}')");
        private ILocator RequestIdCell => _locators._page.Locator("[data-column='Req. ID']").First;
        private ILocator RequestRow(string requestId) => _locators._page.Locator($"text={requestId}");

        public async Task ClickApprove() => await _locators.btnApprove.ClickAsync();

        public async Task ClickTab(string tabName)
        {
            await Tab(tabName).ClickAsync();
        }

        public async Task<int> GetTabCountAsync(string tabName)
        {
            var text = await TabCount(tabName).InnerTextAsync();
            var match = System.Text.RegularExpressions.Regex.Match(text, "\\d+");
            return match.Success ? int.Parse(match.Value) : 0;
        }

        public async Task<string> GetFirstRequestIdAsync()
        {
            return await RequestIdCell.InnerTextAsync();
        }

        public async Task<bool> IsRequestPresentAsync(string requestId)
        {
            return await RequestRow(requestId).IsVisibleAsync();
        }
        public async Task ClickReject() => await _locators.btnReject.ClickAsync();
        public async Task ClickSelectRow() => await _locators.btnSelectRow.ClickAsync();
        public async Task ClickSelectAllRows() => await _locators.btnSelectAllRow.ClickAsync();
        public async Task EnterRejectReason() => await _locators.txtRejectReason.FillAsync("Reject Request");
        public async Task ClickRejectRequest() => await _locators.btnRejectRequest.ClickAsync();
        public async Task ClickApproveAll() => await _locators.btnApproveAll.ClickAsync();
        public async Task ClickRejectAll() => await _locators.btnRejectAll.ClickAsync();
        public async Task EnterBulkRejectReason(string reason) => await _locators.txtBulkRejectReason.FillAsync(reason);
        public async Task ClickProceed() => await _locators.btnProceed.ClickAsync();
        public async Task<int> GetDisplayedCount() => int.Parse(await _locators.txtPendingCount.InnerTextAsync());
        
        public ILocator ToastMessage => _locators.toastMessage;

        // Click the multi-select checkbox at the top of the grid
        public async Task ClickMultiSelectCheckbox() => await _locators.multiSelectCheckbox.ClickAsync();

        // Check if Approve All button is visible
        public async Task<bool> IsApproveAllVisible() => await _locators.btnApproveAll.IsVisibleAsync();

        // Check if Reject All button is visible
        public async Task<bool> IsRejectAllVisible() => await _locators.btnRejectAll.IsVisibleAsync();

        
    }
}