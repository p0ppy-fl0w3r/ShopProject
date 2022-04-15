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
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MembersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Members.Include(m => m.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.MembershipCategories, "MemberCategoryId", "Description");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberDto member)
        {
            if (member.MemberCategory.MemberCategoryId != 0)
            {
                ModelState.Remove("MemberCategory.TotalLoans");
                ModelState.Remove("MemberCategory.Description");
                ModelState.Remove("MemberCategory.Description");
            }

            if (ModelState.IsValid)
            {

                using var transaction = _context.Database.BeginTransaction();

                var category = member.MemberCategory;

                if (category.MemberCategoryId == 0)
                {
                    _context.MembershipCategories.Add(category);
                }
                else { 
                    category = _context.MembershipCategories.Where(c => c.MemberCategoryId == category.MemberCategoryId).FirstOrDefault();
                }

                var filePath = await SaveImages(member.MemberImage, $"{member.FirstName}_{member.LastName}_{DateTime.Now.Millisecond}");

                var newMember = new Member {
                    MemberId = 0,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Address = member.Address,
                    MemberImage = filePath,
                    DateOfBirth = member.DateOfBirth,
                    Category = category,
                };

                _context.Members.Add(newMember);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.MembershipCategories, "MemberCategoryId", "Description", member.MemberCategory.MemberCategoryId);
            return View(member);
        }

        private async Task<string> SaveImages(IFormFile file, string memberInfo)
        {

            string uploadFolder = Path.Combine("uploads", "rupey_members", memberInfo);
            string contentPath = Path.Combine(_webHostEnvironment.ContentRootPath, uploadFolder);

            if (!Directory.Exists(contentPath))
            {
                Directory.CreateDirectory(contentPath);
            }

            string filePath = "";


            if (file != null)
            {
                var InputFileName = Path.GetFileName(file.FileName);

                // File paths that will be saved to the database.
                filePath = Path.Combine(uploadFolder, InputFileName);

                var ServerSavePath = Path.Combine(contentPath, InputFileName);
                //Save file to uploads folder  
                using (Stream fileStream = new FileStream(ServerSavePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return filePath;
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.MembershipCategories, "MemberCategoryId", "Description", member.CategoryId);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,CategoryId,Address,FirstName,LastName,DateOfBirth,MemberImage")] Member member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId))
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
            ViewData["CategoryId"] = new SelectList(_context.MembershipCategories, "MemberCategoryId", "Description", member.CategoryId);
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }
    }
}
