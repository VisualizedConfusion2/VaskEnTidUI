using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VaskEnTidLib.Models;

namespace VaskEnTidUI.Pages
{
    public class BookingModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;

        public BookingModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        [Required(ErrorMessage = "Maskine er p�kr�vet")]
        public int MachineID { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Dato er p�kr�vet")]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [BindProperty]
        [Required(ErrorMessage = "Start tid er p�kr�vet")]
        public TimeOnly StartTime { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Slut tid er p�kr�vet")]
        public TimeOnly EndTime { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Maskintype er p�kr�vet")]
        public int MachineTypeID { get; set; }

        public List<Machine> Machines { get; set; } = new();
        public List<MachineType> MachineTypes { get; set; } = new();

        public void OnGet()
        {
            _loadMachines();
            _loadMachineTypes();
        }

        public IActionResult OnPost()
        {
            _loadMachines();
            _loadMachineTypes();
            // Custom validation
            if (StartTime < new TimeOnly(7, 0) || EndTime > new TimeOnly(21, 0))
            {
                ModelState.AddModelError(string.Empty, "Booking skal v�re mellem 07:00 og 21:00");
            }

            if (StartTime >= EndTime)
            {
                ModelState.AddModelError(string.Empty, "Start tid skal v�re f�r slut tid");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Process booking here

            return RedirectToPage("Success");
        }

        private void _loadMachineTypes()
        {
            MachineTypes = new List<MachineType>
            {
                new MachineType { MachineTypeId = 1, Type = "Vaskemaskine" },
                new MachineType { MachineTypeId = 2, Type = "T�rretumbler" }
            };
        }

        private void _loadMachines()
        {
            Machines = new List<Machine>
            {
                new Machine { MachineId = 1, Name = "Maskine A", MachineTypeId = 1 },
                new Machine { MachineId = 2, Name = "Maskine B", MachineTypeId = 2 },
                new Machine { MachineId = 3, Name = "Maskine C", MachineTypeId = 1 }
            };
        }
    }
}
