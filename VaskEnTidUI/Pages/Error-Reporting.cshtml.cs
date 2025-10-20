using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using VaskEnTidLib.Services;
using VaskEnTidLib.Models;

namespace VaskEnTidUI.Pages
{
    public class Error_ReportingModel : PageModel
    {
        private readonly ILogger<Error_ReportingModel> _logger;
        private readonly ErrorReportService _errorReportService;

        public Error_ReportingModel(ILogger<Error_ReportingModel> logger, ErrorReportService errorReportService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _errorReportService = errorReportService;
        }

        public List<ErrorReport> ErrorReports { get; set; } = new();

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                // Not logged in – redirect to login page
                return RedirectToPage("/Login");
            }
            _loadErrorReports();
            return Page();
        }

        public IActionResult OnPost()
        {
            return Page();
        }

        private void _loadErrorReports()
        {
            ErrorReports = _errorReportService.GetErrorReportByUserId(int.Parse(HttpContext.Session.GetString("UserId")));
        }
    }
}
