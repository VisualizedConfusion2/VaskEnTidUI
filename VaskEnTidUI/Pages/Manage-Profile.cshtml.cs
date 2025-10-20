using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VaskEnTidLib.Repositories;

namespace VaskEnTidUI.Pages
{
    public class Manage_ProfileModel : PageModel
    {
        private readonly UserRepo _userRepo;

        public Manage_ProfileModel(UserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [BindProperty] public int UserId { get; set; }
        [BindProperty] public string? ApartmentNumber { get; set; }
        [BindProperty] public string? Name { get; set; }
        [BindProperty] public string? Phone { get; set; }
        [BindProperty] public string? Email { get; set; }
        [BindProperty] public string? Password { get; set; }

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            // Ingen datahentning – siden bruges kun til opdatering
        }

        public IActionResult OnPost()
        {
            if (UserId <= 0)
            {
                ErrorMessage = "Indtast et gyldigt bruger-ID.";
                return Page();
            }

            bool updated = _userRepo.UpdateUserById(
                UserId,
                ApartmentNumber,
                Name,
                Phone,
                Email,
                string.IsNullOrWhiteSpace(Password) ? null : Password
            );

            if (updated)
                SuccessMessage = "Bruger blev opdateret succesfuldt.";
            else
                ErrorMessage = "Kunne ikke opdatere bruger. Tjek at bruger-ID’et eksisterer.";

            return Page();
        }
    }
}
