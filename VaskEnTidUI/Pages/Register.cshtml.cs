using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VaskEnTidLib.Services;
using VaskEnTidLib.Models;

namespace VaskEnTidUI.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly UserService _userService;

        public RegisterModel(ILogger<LoginModel> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Telefonnummer er påkrævet")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Email er påkrævet")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Oprettelseskode er påkrævet")]
        public string RegisterCode { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Adgangskode er påkrævet")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Adgangskode skal udfyldes 2 gange")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Adgangskode matcher ikke")]
        public string ConfirmPassword { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User _user = _userService.RegisterUserByCreationCode(RegisterCode, PhoneNumber, Email, Password);
            if (_user == null)
            {
                ErrorMessage = "Forkert registreringskoden";
                return Page();
            }

            // Register successful – save session
            HttpContext.Session.SetString("UserId", _user.UserId.ToString());
            HttpContext.Session.SetString("UserName", _user.Name);
            HttpContext.Session.SetString("DepartmentId", _user.DepartmentID.ToString());
            HttpContext.Session.SetString("UserTypeId", _user.UserTypeID.ToString());

            return RedirectToPage("/Index");
        }
    }
}
