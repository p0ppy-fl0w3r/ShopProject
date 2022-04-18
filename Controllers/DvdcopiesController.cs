using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.DTOs;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class DvdcopiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DvdcopiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dvdcopies
        public async Task<IActionResult> Index()
        {
            ViewData["DvdList"] = new SelectList(_context.Dvdtitles, "DvdId", "DvDname");
            var applicationDbContext = _context.Dvdcopies.Include(d => d.Dvd);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: Dvdcopies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvdcopy = await _context.Dvdcopies
                .Include(d => d.Dvd)
                .FirstOrDefaultAsync(m => m.CopyId == id);
            if (dvdcopy == null)
            {
                return NotFound();
            }

            return View(dvdcopy);
        }

        // POST: Dvdcopies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dvdcopy = await _context.Dvdcopies.FindAsync(id);
            _context.Dvdcopies.Remove(dvdcopy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> OldStock()
        {

            var oldDvd = _context.Dvdcopies.Include(d => d.Dvd).Where(c => !c.Loans.Where(l => l.ReturnedDate == null).Any());

            var oldDvdNoLoan = oldDvd.Where(o => o.DatePurchased.AddDays(365) < DateTime.Now);

            return View(await oldDvdNoLoan.ToListAsync());
        }

        public IActionResult InLoan()
        {
            var currentLoan = _context.Loans
                .Include(l => l.Copy)
                .Include(l => l.Member)
                .Include(l => l.Copy.Dvd)
                .Where(l => l.ReturnedDate == null)
                .OrderBy(l => l.DateOut)
                .GroupBy(l => l.DateOut);

            var gropuedLoanObj = currentLoan.Select(l => new InLoanDto
            {
                OutDate = (l.Key ?? DateTime.Now).ToString("d"),
                Lonas = l.ToList()

            });

            return View(gropuedLoanObj.ToList());
        }

        private bool DvdcopyExists(int id)
        {
            return _context.Dvdcopies.Any(e => e.CopyId == id);
        }
    }
}
