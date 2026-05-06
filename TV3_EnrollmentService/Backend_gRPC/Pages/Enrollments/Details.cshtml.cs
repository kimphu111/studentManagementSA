using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using studentManagement.Data;
using studentManagement.Models;
using System.Threading.Tasks;

namespace Enrollments
{
    public class DetailsModel : PageModel
    {
        private readonly EnrollmentDbContext _context;
        public DetailsModel(EnrollmentDbContext context)
        {
            _context = context;
        }
        public EnrollmentRecord EnrollmentRecord { get; set; } = new EnrollmentRecord();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var record = await _context.Enrollments.FindAsync(id);
            if (record == null) return NotFound();
            EnrollmentRecord = record;
            return Page();
        }
    }
}