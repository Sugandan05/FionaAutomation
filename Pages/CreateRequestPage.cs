// Pages/LoginPageLocators.cs
using Microsoft.Playwright;

namespace FionaAutomation.Pages
{
    public class CreateRequestPage
    {
        private readonly IPage _page;

        public CreateRequestPage(IPage page)
        {
            _page = page;
        }
        private ILocator AllDropdowns => _page.Locator("input[placeholder='Select']");
        public ILocator btnCreateRequest => _page.Locator("text=CREATE NEW REQUEST");
        public ILocator txtDate => _page.Locator("input[placeholder='DD/MM/YYYY']");
        public ILocator ddlPaymentType => AllDropdowns.Nth(0);
        public ILocator ddlPaymentRegion => AllDropdowns.Nth(1);
        public ILocator ddlAccountType => AllDropdowns.Nth(2);
        public ILocator ddlAccount => AllDropdowns.Nth(3);
        public ILocator txtBeneficiaryName => _page.Locator("//input[@data-testid='mwl-textbox']").Nth(4);
        public ILocator txtSortCode => _page.Locator("//input[@data-testid='mwl-textbox']").Nth(5);
        public ILocator txtAccountNumber => _page.Locator("//input[@data-testid='mwl-textbox']").Nth(6);
        public ILocator txtNetValue => _page.Locator("//input[@data-testid='mwl-textbox']").Nth(9);
        public ILocator ddlCurrency => AllDropdowns.Nth(5);
        public ILocator ddlEntity => AllDropdowns.Nth(6);
        public ILocator txtNominalAccount => _page.Locator("//input[@data-testid='mwl-textbox']").Nth(11);
        public ILocator txtDescription => _page.Locator("//textarea[@data-testid='mwl-textbox']").Nth(0);
        public ILocator btnSubmitRequest => _page.Locator("text=SUBMIT REQUEST");

       public ILocator toastMessage => _page.Locator("div.MuiSnackbarContent-message");

    }
}
