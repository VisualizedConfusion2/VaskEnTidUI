using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VaskEnTidLib.Models;
using VaskEnTidLib.Services;

namespace VaskEnTidUI.Pages
{
    public class BookingModel : PageModel
    {
        private readonly ILogger<BookingModel> _logger;
        private readonly MachineService _machineService;

        public BookingModel(ILogger<BookingModel> logger, MachineService machineService)
        {
            _logger = logger;
            _machineService = machineService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Maskintype er påkrævet")]
        public string MachineType { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Maskine er påkrævet")]
        public int MachineID { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Dato er påkrævet")]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [BindProperty]
        [Required(ErrorMessage = "Start tid er påkrævet")]
        public TimeOnly StartTime { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Slut tid er påkrævet")]
        public TimeOnly EndTime { get; set; }

        public List<Machine> Machines { get; set; } = new();
        public List<String> MachineTypes { get; set; } = new();

        public IActionResult OnGet()
        {
            // Check if user is logged in
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                // Not logged in – redirect to login page
                return RedirectToPage("/Login");
            }

            // User is logged in – continue loading data
            LoadMachines();
            LoadMachineTypes();

            return Page();
        }

        private void LoadMachineTypes()
        {
            Machines = _machineService.GetMachinesByUserId(1);
            MachineTypes = Machines.Select(m => m.MachineType).Distinct().ToList();
        }

        private void LoadMachines()
        {
            if (!string.IsNullOrEmpty(MachineType))
            {
                Machines = Machines.Where(m => m.MachineType == MachineType).ToList();
            }
        }
    }
}
