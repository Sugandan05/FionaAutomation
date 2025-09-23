using FionaAutomation.Pages;
using Microsoft.Playwright;

namespace FionaAutomation.Actions
{
    public class CreateRequestPageActions
    {
        private readonly CreateRequestPage _locators;

        public CreateRequestPageActions(CreateRequestPage locators)
        {
            _locators = locators;
        }

        public async Task ClickCreateRequest() => await _locators.btnCreateRequest.ClickAsync();
        public async Task EnterDate(string date) => await _locators.txtDate.FillAsync(date);
        public async Task SelectPaymentType(string paymentType)
        {
            await _locators.ddlPaymentType.FillAsync(paymentType);
            await Task.Delay(500);
            await _locators.ddlPaymentType.PressAsync("ArrowDown");
            
            await _locators.ddlPaymentType.PressAsync("Enter");
            await _locators.ddlPaymentType.PressAsync("Enter");
        }

        public async Task SelectPaymentRegion(string paymentRegion)
        {
            await _locators.ddlPaymentRegion.FillAsync(paymentRegion);
            await Task.Delay(500);
            await _locators.ddlPaymentRegion.PressAsync("ArrowDown");
            await _locators.ddlPaymentRegion.PressAsync("Enter");
        }

        public async Task SelectAccountType(string accountType)
        {
            await _locators.ddlAccountType.FillAsync(accountType);
            await Task.Delay(500);
            await _locators.ddlAccountType.PressAsync("ArrowDown");
            await _locators.ddlAccountType.PressAsync("Enter");
        }
        public async Task SelectAccount(string account)
        {
            await _locators.ddlAccount.FillAsync(account);
            await Task.Delay(1000);
            await _locators.ddlAccount.PressAsync("ArrowDown");
            await _locators.ddlAccount.PressAsync("Enter");
        }

        public async Task
        EnterBeneficiaryName(string beneficiaryName) => await _locators.txtBeneficiaryName.FillAsync(beneficiaryName);
        public async Task EnterSortCode(string sortCode) => await _locators.txtSortCode.FillAsync(sortCode);
        public async Task EnterAccountNumber(string accountNumber) => await _locators.txtAccountNumber.FillAsync(accountNumber);
        public async Task EnterNetValue(string netValue) => await _locators.txtNetValue.FillAsync(netValue);
        public async Task SelectCurrency(string currency)
        {
            await _locators.ddlCurrency.FillAsync(currency);
            await Task.Delay(1000);
            await _locators.ddlCurrency.PressAsync("ArrowDown");
            await _locators.ddlCurrency.PressAsync("Enter");
        }

        public async Task SelectEntity(string entity)
        {
            await _locators.ddlEntity.FillAsync(entity);
            await Task.Delay(500);
            await _locators.ddlEntity.PressAsync("ArrowDown");
            await _locators.ddlEntity.PressAsync("Enter");
        }

        public async Task EnterNominalAccount(string nominalAccount) => await _locators.txtNominalAccount.FillAsync(nominalAccount);
        public async Task EnterDescription(string description) => await _locators.txtDescription.FillAsync(description);
        public async Task ClickSubmitRequest() => await _locators.btnSubmitRequest.ClickAsync();
        public async Task ClickEditRequest() => await _locators.btnEditRequest.ClickAsync();
        public ILocator ToastMessage => _locators.toastMessage;
        

    }
}
