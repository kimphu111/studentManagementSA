using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using studentManagement.Data;
using studentManagement.Models;
using System.Threading.Tasks;

namespace Enrollments
{
    public class DeleteModel : PageModel
    {
        private readonly EnrollmentDbContext _context;
        public DeleteModel(EnrollmentDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public EnrollmentRecord EnrollmentRecord { get; set; } = new EnrollmentRecord();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var record = await _context.Enrollments.FindAsync(id);
            if (record == null) return NotFound();
            EnrollmentRecord = record;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var record = await _context.Enrollments.FindAsync(EnrollmentRecord.Id);
            if (record == null) return NotFound();
            _context.Enrollments.Remove(record);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}