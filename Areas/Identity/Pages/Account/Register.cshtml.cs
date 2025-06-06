using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using AI_Wardrobe.Models;
using AI_Wardrobe.Repositories;
using AI_Wardrobe.Data.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using static AI_Wardrobe.Data.Services.ReCAPTCHA;


namespace AI_Wardrobe.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly UserRepo _userRepo;
        private readonly UserRoleRepo _userRoleRepo;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IConfiguration configuration,
            UserRepo userRepo,
            UserRoleRepo userRoleRepo,
            IEmailService emailService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _configuration = configuration;
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
            _emailService = emailService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["SiteKey"] = _configuration.GetSection("recaptchaApiKey").Value;
            ViewData["InitialSetup"] = !_userRepo.HasUsers();
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            string captchaResponse = Request.Form["g-Recaptcha-Response"];

            string recaptchaSecret = _configuration.GetSection("recaptchaSecretKey").Value;


            ReCaptchaValidationResult resultCaptcha =
            ReCaptchaValidator.IsValid(recaptchaSecret, captchaResponse);
            // Invalidate the form if the captcha is invalid.
            if (!resultCaptcha.Success)
            {
                ViewData["SiteKey"] = _configuration["Recaptcha:SiteKey"];
                ModelState.AddModelError(string.Empty,
                "The ReCaptcha is invalid.");
            }


            if (ModelState.IsValid)
            {
                var user = CreateUser();


                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    //Added send grid verification
                    var response = await _emailService.SendSingleEmail(new ComposeEmailModel
                    {
                        Subject = "Confirm your email",
                        Email = Input.Email,
                        Body = $"Please confirm your account by <a " +
                           $"href='{HtmlEncoder.Default.Encode(callbackUrl)}'> " +
                           $"clicking here</a>."
                    });

                    ////add the data to the user table
                    //RegisteredUser registerUser = new RegisteredUser()
                    //{
                    //    Email = Input.Email,
                    //};
                    //_userRepo.AddUser(registerUser);
                    ////need to add the default user role eventually.
                    //await _userRoleRepo.AddAsCustomer(registerUser.Email);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        //add the data to the user table
                        RegisteredUser registerUser = new RegisteredUser()
                        {
                            Email = Input.Email,
                        };

                        //first registered user during setup is by default admin, any succedding registration is by default a customer
                        var initialSetup = !_userRepo.HasUsers();

                        _userRepo.AddUser(registerUser);

                        if (initialSetup)
                        {
                            await _userRoleRepo.AddAsAdmin(registerUser.Email);
                        } else
                        {
                            await _userRoleRepo.AddAsCustomer(registerUser.Email);
                        }

                        return RedirectToPage("RegisterConfirmation",
                            new
                            {
                                email = Input.Email,
                                returnUrl = returnUrl,
                                DisplayConfirmAccountLink = false
                            });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}