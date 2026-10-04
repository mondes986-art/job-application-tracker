using JobApplicationTracker.Data;
using JobApplicationTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Controllers
{
    public class JobApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobApplications
        // GET: JobApplications
        public async Task<IActionResult> Index(string searchString, string statusFilter)
        {
            var applications = _context.JobApplications.AsQueryable();

            // Search by company or position
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                applications = applications.Where(a =>
                    a.Company.Contains(searchString) ||
                    a.Position.Contains(searchString));
            }

            // Filter by status
            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                applications = applications.Where(a =>
                    a.Status == statusFilter);
            }

            var allApplications = await _context.JobApplications.ToListAsync();

            ViewBag.TotalApplications = allApplications.Count;
            ViewBag.Applied = allApplications.Count(a => a.Status == "Applied");
            ViewBag.Interviews = allApplications.Count(a => a.Status == "Interview");
            ViewBag.Assessments = allApplications.Count(a => a.Status == "Assessment");
            ViewBag.Offers = allApplications.Count(a => a.Status == "Offer");
            ViewBag.Rejected = allApplications.Count(a => a.Status == "Rejected");

            ViewBag.SearchString = searchString;
            ViewBag.StatusFilter = statusFilter;

            return View(await applications.ToListAsync());
        }

        // GET: JobApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications
                .FirstOrDefaultAsync(m => m.Id == id);

            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // GET: JobApplications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobApplications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobApplication jobApplication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobApplication);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(jobApplication);
        }

        // GET: JobApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications.FindAsync(id);

            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // POST: JobApplications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            JobApplication jobApplication)
        {
            if (id != jobApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobApplicationExists(jobApplication.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(jobApplication);
        }

        // GET: JobApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications
                .FirstOrDefaultAsync(m => m.Id == id);

            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // POST: JobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);

            if (jobApplication != null)
            {
                _context.JobApplications.Remove(jobApplication);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool JobApplicationExists(int id)
        {
            return _context.JobApplications.Any(e => e.Id == id);
        }
    }
}
