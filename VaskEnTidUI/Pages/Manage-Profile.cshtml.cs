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

        [BindProperty] public string? ApartmentNumber { get; set; }
        [BindProperty] public string? Name { get; set; }
        [BindProperty] public string? Phone { get; set; }
        [BindProperty] public string? Email { get; set; }
        [BindProperty] public string? Password { get; set; }

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            // Ingen opdatering på GET, siden viser kun formular
        }

        public IActionResult OnPost()
        {
            string? userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrWhiteSpace(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                ErrorMessage = "Kunne ikke finde bruger-ID i session. Log ind igen.";
                return Page();
            }

            bool updated = _userRepo.UpdateUserById(
                userId,
                ApartmentNumber,
                Name,
                Phone,
                Email,
                string.IsNullOrWhiteSpace(Password) ? null : Password
            );

            if (updated)
                SuccessMessage = "Bruger blev opdateret succesfuldt.";
            else
                ErrorMessage = "Kunne ikke opdatere bruger. Tjek dine oplysninger.";

            return Page();
        }
    }
}
