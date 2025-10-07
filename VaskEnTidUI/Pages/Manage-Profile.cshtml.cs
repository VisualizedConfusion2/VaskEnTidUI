using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace VaskEnTidUI.Pages
{
    public class Manage_ProfileModel : PageModel
    {
        private readonly ILogger<Manage_ProfileModel> _logger;

        public Manage_ProfileModel(ILogger<Manage_ProfileModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        [Required(ErrorMessage = "Telefonnummer eller Email er påkrævet")]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Adgangskode er påkrævet")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } // <-- This will hold the "wrong password" message

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
            if (Username == "admin" && Password == "1234")
            {
                _logger.LogInformation("Bruger loggede ind: {Username}", Username);
                return RedirectToPage("/Index");
            }
            ErrorMessage = "Der er indtastet et forkert brugernavn eller adgangskode";


            ModelState.AddModelError(string.Empty, "Ugyldigt loginforsøg.");
            return Page();
        }
    }
}
