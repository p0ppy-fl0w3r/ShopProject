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
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Available(string? error)
        {

            if (error != null)
            {
                ViewData["message"] = error;
            }
            else {
                ViewData["message"] = "";
            }

            var avalilableCopies = _context.Dvdcopies.Include(c => c.Loans).Include(c => c.Dvd).Where(c => c.Loans.Count < 1 || c.Loans.All(e => e.ReturnedDate != null));
            var dvdCopies = await avalilableCopies.ToListAsync();

            var copiesWithMember = dvdCopies.Select(d => new CopyDto
            {
                Dvdcopy = d,
            });

            return View(copiesWithMember.ToList<CopyDto>()); ;
        }


        public async Task<IActionResult> LoanToMember(CopyDto copy)
        {

            // TODO check for age rating

            var dvdRating = _context.Dvdcopies.Include(c => c.Dvd).Where(c => c.CopyId == copy.CopyId).FirstOrDefault();

            var member = _context.Members.Where(m => m.MemberId == copy.MemberId).FirstOrDefault();

            if (member == null)
            {
                return RedirectToAction("Available", "Loans", new { error = "Big error!" });
            }

            // TODO check the member's age and dvd's rating.



            Console.WriteLine($"The ids are {copy.CopyId} {copy.MemberId}");

            return View();


        }

        private IActionResult JavaScript(object p)
        {
            throw new NotImplementedException();
        }

        // GET: Loans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Loans.Include(l => l.Copy).Include(l => l.Member).Include(l => l.Type);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Loans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Copy)
                .Include(l => l.Member)
                .Include(l => l.Type)
                .FirstOrDefaultAsync(m => m.LoanId == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loans/Create
        public IActionResult Create()
        {
            ViewData["CopyId"] = new SelectList(_context.Dvdcopies, "CopyId", "CopyId");
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "Address");
            ViewData["TypeId"] = new SelectList(_context.LoanTypes, "LoanTypeId", "LoanTypeName");
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanId,CopyId,MemberId,TypeId,DateOut,DateDue,ReturnedDate")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CopyId"] = new SelectList(_context.Dvdcopies, "CopyId", "CopyId", loan.CopyId);
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "Address", loan.MemberId);
            ViewData["TypeId"] = new SelectList(_context.LoanTypes, "LoanTypeId", "LoanTypeName", loan.TypeId);
            return View(loan);
        }

        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewData["CopyId"] = new SelectList(_context.Dvdcopies, "CopyId", "CopyId", loan.CopyId);
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "Address", loan.MemberId);
            ViewData["TypeId"] = new SelectList(_context.LoanTypes, "LoanTypeId", "LoanTypeName", loan.TypeId);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LoanId,CopyId,MemberId,TypeId,DateOut,DateDue,ReturnedDate")] Loan loan)
        {
            if (id != loan.LoanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.LoanId))
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
            ViewData["CopyId"] = new SelectList(_context.Dvdcopies, "CopyId", "CopyId", loan.CopyId);
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "Address", loan.MemberId);
            ViewData["TypeId"] = new SelectList(_context.LoanTypes, "LoanTypeId", "LoanTypeName", loan.TypeId);
            return View(loan);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Copy)
                .Include(l => l.Member)
                .Include(l => l.Type)
                .FirstOrDefaultAsync(m => m.LoanId == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.LoanId == id);
        }
    }
}
