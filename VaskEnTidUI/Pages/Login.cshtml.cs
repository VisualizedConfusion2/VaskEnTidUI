using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using VaskEnTidLib.Models;

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
        [Required(ErrorMessage = "Email er påkrævet")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Adgangskode er påkrævet")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Indtast venligst både email og adgangskode.";
                return Page();
            }

            var user = GetUserByEmail(Email);

            if (user == null || user.Password != Password)
            {
                ErrorMessage = "Ugyldig email eller adgangskode.";
                return Page();
            }

            // Login successful – save session
            HttpContext.Session.SetString("UserID", user.UserId.ToString());
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToPage("/Index");
        }

        private User? GetUserByEmail(string email)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LaundryManagementDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Application Name=\"SQL Server Management Studio\";Command Timeout=30";


            using var conn = new SqlConnection(connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
            SELECT u.UserId, u.ApartmentNumber, u.Name, u.Phone, u.Email, u.Password,
                   udm.DepartmentID, udm.UserTypeID
            FROM Users u
            INNER JOIN UserDepartmentMappings udm ON u.UserId = udm.UserID
            WHERE u.Email = @Email
        ", conn);

            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    UserId = (int)reader["UserId"],
                    ApartmentNumber = reader["ApartmentNumber"].ToString() ?? string.Empty,
                    Name = reader["Name"].ToString() ?? string.Empty,
                    Phone = reader["Phone"].ToString() ?? string.Empty,
                    Email = reader["Email"].ToString() ?? string.Empty,
                    Password = reader["Password"].ToString() ?? string.Empty,
                    DepartmentID = (int)reader["DepartmentID"],
                    UserTypeID = (int)reader["UserTypeID"]
                };
            }

            return null;
        }
    }
}
