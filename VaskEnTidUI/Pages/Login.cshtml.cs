using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace VaskEnTidUI.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        [Required(ErrorMessage = "Brugernavn er p�kr�vet")]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Adgangskode er p�kr�vet")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Dummy login logik � du kan skifte det ud med rigtig brugerh�ndtering
            if (Username == "admin" && Password == "1234")
            {
                _logger.LogInformation("Bruger loggede ind: {Username}", Username);
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Ugyldigt loginfors�g.");
            return Page();
        }
    }
}
