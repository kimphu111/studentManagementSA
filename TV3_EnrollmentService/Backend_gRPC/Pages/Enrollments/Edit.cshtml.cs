using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using studentManagement.Data;
using studentManagement.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Enrollments
{
    public class EditModel : PageModel
    {
        private readonly EnrollmentDbContext _context;
        public EditModel(EnrollmentDbContext context)
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
            if (!ModelState.IsValid) return Page();
            _context.Attach(EnrollmentRecord).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Enrollments.Any(e => e.Id == EnrollmentRecord.Id))
                    return NotFound();
                else throw;
            }
            return RedirectToPage("Index");
        }
    }
}