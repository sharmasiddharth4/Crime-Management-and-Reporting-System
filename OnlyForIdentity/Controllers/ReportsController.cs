using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlyForIdentity.Data;
using OnlyForIdentity.Models;

namespace OnlyForIdentity.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Index(string searchQuery)
        {
            if (User.IsInRole("Admin"))
            {
                IQueryable<Report> reportsQuery = _context.Reports;

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    reportsQuery = reportsQuery.Where(r => r.Description.Contains(searchQuery));
                }

                var reports = await reportsQuery.ToListAsync();
                return View(reports);
            }
            else if (User.IsInRole("User"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userReports = await _context.Reports
                    .Where(r => r.UserId == userId)
                    .ToListAsync();

                return View(userReports);
            }

            return Unauthorized(); // If the user's role is neither Admin nor User
        }



        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reports == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }


        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,Description,Location,DateTime,Case,UserId")] Report report)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                report.UserId = userId;

                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Validation Error for {key}: {error.ErrorMessage}");
                    }
                }
                return View(report);
            }
            return View(report);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reports == null)
            {
                return NotFound();
            }

            //var report = await _context.Reports.FindAsync(id);
            var report = await _context.Reports.Include(r => r.Case).FirstOrDefaultAsync(m => m.ReportId == id);
            
            if (report == null)
                if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportId,Description,Location,DateTime,Case,AccusedName")] Report report)
        {
            if (id != report.ReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingReport = await _context.Reports.Include(r => r.Case).FirstOrDefaultAsync(r => r.ReportId == id);

                    if (existingReport == null)
                    {
                        return NotFound();
                    }

                    existingReport.Description = report.Description;
                    existingReport.Location = report.Location;
                    existingReport.DateTime = report.DateTime;
                    existingReport.Case.Status = report.Case.Status;

                    if (User.IsInRole("Admin"))
                    {
                        if (existingReport.Case.Status == Case.CaseStatus.Solved)
                        {
                            existingReport.AccusedName = report.AccusedName;
                        }
                    }

                    _context.Update(existingReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.ReportId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reports == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reports == null)
            {
                return Problem("Entity set 'AppDbContext.Reports'  is null.");
            }
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                _context.Reports.Remove(report);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
          return (_context.Reports?.Any(e => e.ReportId == id)).GetValueOrDefault();
        }
    }
}
