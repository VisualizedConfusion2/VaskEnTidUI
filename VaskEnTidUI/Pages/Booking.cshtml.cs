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
        private readonly BookingService _bookingService;

        public BookingModel(ILogger<BookingModel> logger)
        {
            _logger = logger;
            _bookingService = new BookingService();
        }

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

        [BindProperty]
        [Required(ErrorMessage = "Maskintype er påkrævet")]
        public int MachineTypeID { get; set; }

        public List<Machine> Machines { get; set; } = new();
        public List<MachineType> MachineTypes { get; set; } = new();
        public List<Booking> ExistingBookings { get; set; } = new(); // <-- new

        public void OnGet()
        {
            LoadMachines();
            LoadMachineTypes();
        }

        public IActionResult OnPost()
        {
            LoadMachines();
            LoadMachineTypes();

            // Custom validation
            if (StartTime < new TimeOnly(7, 0) || EndTime > new TimeOnly(21, 0))
            {
                ModelState.AddModelError(string.Empty, "Booking skal være mellem 07:00 og 21:00");
            }

            if (StartTime >= EndTime)
            {
                ModelState.AddModelError(string.Empty, "Start tid skal være før slut tid");
            }

            // Load existing bookings for the selected machine
            if (MachineID != 0)
            {
                ExistingBookings = _bookingService.GetBookingsByMachine(MachineID).ToList();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var booking = new Booking
            {
                UserId = 1, // Replace with actual logged-in user ID
                MachineId = MachineID,
                Date = Date,
                StartTime = StartTime,
                EndTime = EndTime
            };

            try
            {
                _bookingService.CreateBooking(booking);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            return RedirectToPage("Success");
        }

        private void LoadMachineTypes()
        {
            MachineTypes = new List<MachineType>
            {
                new MachineType { MachineTypeId = 1, Type = "Vaskemaskine" },
                new MachineType { MachineTypeId = 2, Type = "Tørretumbler" }
            };
        }

        private void LoadMachines()
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
