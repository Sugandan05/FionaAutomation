using FionaAutomation.Pages;
using Microsoft.Playwright;

namespace FionaAutomation.Actions
{
    public class LoginPageActions
    {
        private readonly LoginPage _locators;

        public LoginPageActions(LoginPage locators)
        {
            _locators = locators;
        }

        public async Task ClickLogin() => await _locators.msSignInButton.ClickAsync();
        public async Task EnterUsername(string username) => await _locators.Usernamefield.FillAsync(username);
        public async Task ClickNextButton() => await _locators.NextButton1.ClickAsync();
        public async Task EnterPassword(string password) => await _locators.Passwordfield.FillAsync(password);
        public async Task ClickSigninButton() => await _locators.SignInButton.ClickAsync();
        public async Task ClickBackButton() => await _locators.BackButton.ClickAsync();

        public async Task<bool> IsMicrosoftLoginPageAsync()
        {
            await _locators.msSignInButton.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 10000 });
            return await _locators.msSignInButton.IsVisibleAsync();
        }
    }
}
