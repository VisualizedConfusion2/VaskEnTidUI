using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VaskEnTidUI.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;

        public RegisterModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
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

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Dummy login logik – du kan skifte det ud med rigtig brugerhåndtering
            if (PhoneNumber == "112")
            {
                _logger.LogInformation("Bruger oprettet");
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Ugyldigt registrerforsøg.");
            return Page();
        }
    }
}
