using Microsoft.AspNetCore.Mvc.RazorPages;
using studentManagement.Data;
using studentManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enrollments
{
    public class IndexModel : PageModel
    {
        private readonly EnrollmentDbContext _context;
        public IndexModel(EnrollmentDbContext context)
        {
            _context = context;
        }
        public IList<EnrollmentRecord> EnrollmentRecords { get; set; } = new List<EnrollmentRecord>();
        public async Task OnGetAsync()
        {
            EnrollmentRecords = await _context.Enrollments.ToListAsync();
        }
    }
}