using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using studentManagement.Data;
using studentManagement.Models;
using System.Threading.Tasks;

namespace Enrollments
{
    public class CreateModel : PageModel
    {
        private readonly EnrollmentDbContext _context;
        public CreateModel(EnrollmentDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public EnrollmentRecord EnrollmentRecord { get; set; } = new EnrollmentRecord();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            _context.Enrollments.Add(EnrollmentRecord);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}