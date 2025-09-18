// Pages/LoginPageLocators.cs
using Microsoft.Playwright;

namespace FionaAutomation.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page)
        {
            _page = page;
        }

        public ILocator msSignInButton => _page.Locator("text=Sign in with Microsoft");
        public ILocator Usernamefield => _page.Locator("#i0116");
        public ILocator NextButton1 => _page.Locator("#idSIButton9");
        public ILocator Passwordfield => _page.Locator("#i0118");
        public ILocator SignInButton => _page.Locator("#idSIButton9");
        public ILocator BackButton => _page.Locator("#idBtn_Back");
    }
}
