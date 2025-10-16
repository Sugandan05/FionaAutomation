// Pages/LoginPageLocators.cs
using Microsoft.Playwright;
namespace FionaAutomation.Pages
{
    public class ApprovalPage
    {
    public readonly IPage _page;

        public ApprovalPage(IPage page)
        {
            _page = page;
        }


        public ILocator btnApprovalpage => _page.Locator("text=Approvals");
        public ILocator btnApprove => _page.Locator("//button[contains(text(),'Approve')]").Nth(0);
        public ILocator btnReject => _page.Locator("//button[contains(text(),'Reject')]").Nth(0);
        public ILocator btnSelectRow => _page.Locator("//input[@name='select_row']").Nth(0);
        public ILocator btnSelectRow1 => _page.Locator("//input[@name='select_row']").Nth(1);
        public ILocator btnSelectAllRow => _page.Locator("input[aria-label='Select all rows']");
        // Add locator for multi-select checkbox at the top of the grid (if different from btnSelectAllRow)
        public ILocator multiSelectCheckbox => _page.Locator("input[type='checkbox']").First;
        public ILocator btnApproveAll => _page.Locator("button:has-text('Approve All')");
        public ILocator btnRejectAll => _page.Locator("button:has-text('Reject All')");
        public ILocator txtRejectReason => _page.Locator("//textarea[@data-testid='mwl-textbox']");
        //public ILocator btnRejectRequest => _page.Locator("text=REJECT REQUEST");
        public ILocator btnRejectRequest => _page.GetByRole(AriaRole.Button, new() { Name = "Reject Request" });

    // Removed duplicate/incorrect locators
        public ILocator txtBulkRejectReason => _page.Locator("//textarea[@data-testid='mwl-textbox']");
        public ILocator btnProceed => _page.Locator("text=PROCEED");
        public ILocator txtPendingTab => _page.Locator("button:has-text('PENDING')");
        public ILocator txtPendingCount => _page.Locator("span.MuiChip-label");
        //public ILocator btnAdhocPayments => _page.GetByRole(AriaRole.Listitem, new() { Name = "Ad-hoc payments" });

        public ILocator btnAdhocPayments => _page.Locator("text=Ad-hoc payments").Nth(0);
        public ILocator toastMessage => _page.Locator("div.MuiSnackbarContent-message");


    }
}
