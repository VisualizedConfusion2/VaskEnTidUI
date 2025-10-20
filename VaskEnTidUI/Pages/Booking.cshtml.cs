using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VaskEnTidLib.Models;
using VaskEnTidLib.Repositories;
using VaskEnTidLib.Services;

namespace VaskEnTidUI.Pages
{
    public class BookingModel : PageModel
    {
        private readonly ILogger<BookingModel> _logger;
        private readonly MachineService _machineService;
        private readonly BookingService _bookingService;

        public BookingModel(ILogger<BookingModel> logger, MachineService machineService, BookingService bookingService)
        {
            _logger = logger;
            _machineService = machineService;
            _bookingService = bookingService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Maskintype er p�kr�vet")]
        public string MachineType { get; set; }

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
        public string ErrorMessage { get; set; }
        public string SuccesMessage { get; set; }
        public List<Machine> Machines { get; set; } = new();
        public List<String> MachineTypes { get; set; } = new();
        public List<Booking> UserBookings { get; set; } = new();

        public IActionResult OnGet()
        {
            // Check if user is logged in
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                // Not logged in � redirect to login page
                return RedirectToPage("/Login");
            }
            // User is logged in � continue loading data

            LoadMachines();
            LoadMachineTypes();
            LoadUserBookings();

            return Page();
        }
        private void LoadUserBookings()
        {
            UserBookings = _bookingService.GetBookingsByUserId(int.Parse(HttpContext.Session.GetString("UserId")));
        }

        private void LoadMachineTypes()
        {
            Machines = _machineService.GetMachinesByUserId(int.Parse(HttpContext.Session.GetString("UserId")));
            MachineTypes = Machines.Select(m => m.MachineType).Distinct().ToList();
        }

        private void LoadMachines()
        {
            if (!string.IsNullOrEmpty(MachineType))
            {
                Machines = Machines.Where(m => m.MachineType == MachineType).ToList();
            }
        }

        public IActionResult OnPost()
        {
            if (Date < DateOnly.FromDateTime(DateTime.Now))
            {
                ErrorMessage = "V�lg venligst en dato, der ikke ligger f�r i dag!";
                LoadMachines();
                LoadMachineTypes();
                LoadUserBookings();
                return Page();
            }
            if (StartTime >= EndTime)
            {
                ErrorMessage = "Starttid skal v�re f�r sluttid";
                LoadMachines();
                LoadMachineTypes();
                LoadUserBookings();
                return Page();
            }
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                ErrorMessage = "Vi kan ikke se at du er logget ind";
                LoadMachines();
                LoadMachineTypes();
                LoadUserBookings();
                return Page();
            }
            try
            {
                var result = _bookingService.CreateBooking(int.Parse(HttpContext.Session.GetString("UserId")), MachineID, Date, StartTime, EndTime);
                if (result.Success)
                {
                    SuccesMessage = "Booking oprettet succesfuldt!";
                    LoadMachines();
                    LoadMachineTypes();
                    LoadUserBookings();
                    return Page();
                }
                else
                {
                    ErrorMessage = result.ErrorMessage ?? "Der opstod en fejl under booking.";
                    LoadMachines();
                    LoadMachineTypes();
                    LoadUserBookings();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fejl: {ex}";
                LoadMachines();
                LoadMachineTypes();
                LoadUserBookings();
                return Page();
            }
            return Page();
        }

    }
}
