using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LibraryMVC.Domain.Entities;
using LibraryMVC.Domain.Repositories;

namespace LibraryMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly SignInManager<Account> _signInManager;
        private readonly UserManager<Account> _userManager;
        public AccountController(IAccountRepository accountRepository, UserManager<Account> userManager, SignInManager<Account> signInManager)
        {
            _accountRepository = accountRepository;
            _userManager = userManager;
            _signInManager = signInManager;
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
                //var encryptedPassword = _accountRepository.EncryptPassword(account.Password);
                var user = new Account { UserName = account.UserName, Email = account.Email , PasswordHash = account.PasswordHash};
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callBackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme); <- doesn't work;

                    //var callBackUrl = Url.Page("/Account/ConfirmEmail", pageHandler: null, values: new { user.ID, code }, protocol: Request.Scheme);
                    //await _emailSender.SendEmailAsync(user.Email, "Please confirm your registration", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>clicking here</a>.");
                    await _signInManager.SignInAsync(user, isPersistent: false);
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

        //[HttpPost]
        //public IActionResult Login(string email, string password)
        //{

        //    if (_accountRepository.LoginSuccess(email, password) == true)
        //    {
        //        //FormsAuthentication.SetAuthCookie(email, false);
        //        return RedirectToAction("SuccessfulLogin");
        //    }

        //    return RedirectToAction("UnsuccessfulLogin");
        //}
        public IActionResult SuccessfulRegister()
        {
            return View();
        }
    }

    
}