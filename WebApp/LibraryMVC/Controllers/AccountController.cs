using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LibraryMVC.Domain.Entities;
using LibraryMVC.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace LibraryMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly SignInManager<Account> _signInManager;
        private readonly UserManager<Account> _userManager;
        private readonly IPasswordHasher<Account> _passwordHasher;
        private readonly ILogger<Account> _logger;


        public AccountController(IAccountRepository accountRepository, UserManager<Account> userManager, SignInManager<Account> signInManager, IPasswordHasher<Account> passwordHasher, ILogger<Account> logger)
        {
            _accountRepository = accountRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
            _logger = logger;
            //_emailSender = emailSender;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Account account, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = new Account { UserName = account.UserName, Email = account.Email , PasswordHash = account.PasswordHash};
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callBackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme); <- doesn't work;

                    //var callBackUrl = Url.Page("/Account/ConfirmEmail", pageHandler: null, values: new { user.ID, code }, protocol: Request.Scheme);
                    //await _emailSender.SendEmailAsync(user.Email, "Please confirm your registration", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>clicking here</a>.");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(1, "User created a new account with a password.");
                    //return RedirectToLocal(returnUrl); <- Redirect to Local doesn't work, find out why;
                    return RedirectToAction("SuccessfulRegister");
                }
            }
            return View();   
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel user, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                Account signedUser = await _userManager.FindByNameAsync(user.UserName);
                var result = await _signInManager.PasswordSignInAsync(signedUser, user.PasswordHash, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(2, "User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl});
                }
                if (result.IsLockedOut)
                {
                    _logger.LogInformation(3, "User account is locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }
            return RedirectToAction("UnsuccessfulLogin");
        }
        public IActionResult SuccessfulRegister()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }

    }
    public class LoginViewModel
    {
        public string PasswordHash { get; set; }
        public string UserName { get; set; }
    }

    
}