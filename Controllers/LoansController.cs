using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.DTOs;
using MyShop.Models;

namespace MyShop.Controllers
{
    [Authorize]
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Available(string? error)
        {

            if (error != null)
            {
                ViewData["message"] = error;
            }
            else
            {
                ViewData["message"] = "";
            }

            ViewData["TypeId"] = new SelectList(_context.LoanTypes, "LoanTypeId", "LoanTypeName");

            var avalilableCopies = _context.Dvdcopies.Include(c => c.Loans).Include(c => c.Dvd)
                .Where(c => c.Loans.Count < 1 || c.Loans.All(e => e.ReturnedDate != null));

            var dvdCopies = avalilableCopies.OrderBy(d => d.Dvd.DvDname).ToList().GroupBy(d => d.DvdId).Select(g => g.FirstOrDefault());

            var copiesWithMember = dvdCopies.Select(d => new CopyDto
            {
                Dvdcopy = d,
            });

            return View(copiesWithMember); ;
        }


        public async Task<IActionResult> LoanToMember(CopyDto copy)
        {


            var dvdRating = _context.Dvdcopies.Include(c => c.Dvd).Include(c => c.Dvd.Category).Where(c => c.CopyId == copy.CopyId).FirstOrDefault();

            var member = _context.Members.Where(m => m.MemberId == copy.MemberId).FirstOrDefault();

            var loanType = _context.LoanTypes.Where(t => t.LoanTypeId == copy.TypeId).FirstOrDefault();
            int duration = 0;

            if (loanType != null)
            {
                duration = loanType.DurationDays ?? 0;
            }
            else
            {
                return RedirectToAction("Available", "Loans", new { error = "Loan type not valid!" });
            }

            if (member == null)
            {
                return RedirectToAction("Available", "Loans", new { error = "Member with given id not found!" });
            }

            if (!member.IsEighteen() && dvdRating.Dvd.Category.AgeRestricted)
            {
                return RedirectToAction("Available", "Loans", new { error = "Member is too young for this movie!" });
            }

            var dateOut = DateTime.Now;
            var dateDue = dateOut.AddDays(duration);

            var newLoan = new Loan
            {
                LoanId = 0,
                Type = loanType,
                CopyId = copy.CopyId,
                MemberId = copy.MemberId,
                DateOut = dateOut,
                DateDue = dateDue,
            };

            _context.Loans.Add(newLoan);
            await _context.SaveChangesAsync();

            var memberName = $"{member.FirstName} {member.LastName}";
            var memberId = $"{member.MemberId}";
            var copyId = $"{copy.CopyId}";
            var originalAmount = $"{dvdRating.Dvd.Rate}";
            var dvdTitle = $"{dvdRating.Dvd.DvDname}";
            var dueDate = dateDue.ToString("d");

            var summaryStr = $"<h3>Loaned to member.</h3><table><tr><td>Member:" +
                $"</td><td>{memberName}</td></tr><tr><td>Member Id:</td><td>{memberId}" +
                $"</td></tr><tr><td>Copy Id:</td><td>{copyId}</td></tr><tr><td>DvD Title" +
                $":</td><td>{dvdTitle}</td></tr><tr><td>Original Amount:</td><td>{originalAmount}" +
                $"</td></tr><tr><td>Due Date:</td><td>{dueDate}</td></tr></table>";


            string encodedStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(summaryStr));


            return RedirectToAction(nameof(Index), new { summary = encodedStr });

        }

        // GET: Loans
        public async Task<IActionResult> Index(string summary)
        {
            if (summary != null)
            {
                string inputStr = Encoding.UTF8.GetString(Convert.FromBase64String(summary));

                ViewData["summary"] = inputStr;
            }
            else
            {
                ViewData["summary"] = "";
            }

            var applicationDbContext = _context.Loans.Include(l => l.Copy).Include(l => l.Copy.Dvd).Include(l => l.Member).Include(l => l.Type);
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
                .Include(l => l.Copy.Dvd)
                .Include(l => l.Member)
                .Include(l => l.Type)
                .FirstOrDefaultAsync(m => m.LoanId == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }



        // Called when the member returns the DvD
        public async Task<IActionResult> Edit(int id)
        {

            var loan = _context.Loans
                .Include(l => l.Member)
                .Include(l => l.Copy)
                .Include(l => l.Copy.Dvd)
                .Where(l => l.LoanId == id)
                .FirstOrDefault();

            if (loan == null)
            {
                return NotFound();
            }

            loan.ReturnedDate = DateTime.Now;

            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();

            var memberName = $"{loan.Member.FirstName} {loan.Member.LastName}";
            var originalAmount = loan.Copy.Dvd.Rate;

            decimal penaltyAmount = 0;
            if (loan.ReturnedDate.Value > loan.DateDue.Value)
            {
                penaltyAmount = loan.ReturnedDate.Value.Subtract(loan.DateDue.Value).Days * loan.Copy.Dvd.PenaltyRate ?? 0;
            }

            var total = originalAmount + penaltyAmount;
            var dvdTitle = loan.Copy.Dvd.DvDname;

            var returnDate = loan.ReturnedDate.Value.ToString("d");
            var copyId = loan.CopyId;

            var htmlString = $"<h3>Received with thanks!</h3><table><tr><td>Member:</td><td>{memberName}</td>" +
                $"</tr><tr><td>Copy Id:</td><td>{copyId}</td></tr><tr><td>DvD Title:</td><td>{dvdTitle}</td>" +
                $"</tr><tr><td>Return Date:</td><td>{returnDate}</td></tr><tr><td>Original Amount:</td><td>{originalAmount}</td>" +
                $"</tr><tr><td>Penalty Amount:</td><td>{penaltyAmount}</td></tr><tr><td><strong>Total:</strong></td><td>{total}</td></tr></table>";



            string encodedStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(htmlString));

            return RedirectToAction(nameof(Index), new { summary = encodedStr });


        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.LoanId == id);
        }
    }
}
