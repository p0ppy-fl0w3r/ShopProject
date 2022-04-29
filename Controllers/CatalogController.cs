using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.DTOs;
using MyShop.Models;
using Newtonsoft.Json;


namespace MyShop.Controllers
{

    [Authorize]
    public class CatalogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CatalogController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Catalog
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchValue, string inStock)
        {
            ViewData["isSearch"] = false;
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                ViewData["isSearch"] = true;

                if (inStock != null)
                {
                    var inStockData = _context.Dvdtitles
                    .Include(d => d.Category)
                    .Include(d => d.Produce)
                    .Include(d => d.Actors)
                    .Include(d => d.Dvdcopies)
                    .Include(d => d.DvDimages)
                    .Include(d => d.Studio).Where(d => d.Actors.Where(a => a.ActorLastName == searchValue).Any()).ToList();


                    var inLoan = _context.Loans.Where(l => l.ReturnedDate == null).Where(l => inStockData.Contains(l.Copy.Dvd));

                    var lStock = inLoan.Select(i => i.Copy.Dvd).Distinct().ToList<Dvdtitle>();

                    return View(lStock);
                }

                var mData = _context.Dvdtitles
                    .Include(d => d.Category)
                    .Include(d => d.Produce)
                    .Include(d => d.Actors)
                    .Include(d => d.Dvdcopies)
                    .Include(d => d.DvDimages)
                    .Include(d => d.Studio).Where(d => d.Actors.Where(a => a.ActorLastName == searchValue).Any());


                return View(await mData.ToListAsync());
            }

            var applicationDbContext = _context.Dvdtitles.Include(d => d.Category).Include(d => d.Produce).Include(d => d.Studio).Include(d => d.Dvdcopies).Include(d => d.DvDimages);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Catalog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvdtitle = await _context.Dvdtitles
                .Include(d => d.Category)
                .Include(d => d.Produce)
                .Include(d => d.Studio)
                .Include(d => d.Actors)
                .FirstOrDefaultAsync(m => m.DvdId == id);
            if (dvdtitle == null)
            {
                return NotFound();
            }

            return View(dvdtitle);
        }

        // GET: Catalog/Create
        public IActionResult Create()
        {
            var dvdDto = new DvdTitleDto { DateReleased = DateTime.Now };

            ViewData["CategoryList"] = new SelectList(_context.Dvdcategories, "CategoryId", "CategoryDescription");
            ViewData["ProduceList"] = new SelectList(_context.Producers, "ProducerId", "ProducerName");
            ViewData["StudioList"] = new SelectList(_context.Studios, "StudioId", "StudioName");
            ViewData["ActorList"] = new SelectList(_context.Actors, "ActorId", "ActorName");

            return View(dvdDto);
        }

        // POST: Catalog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DvdTitleDto dvdDto)
        {

            var actorsList64 = Request.Cookies["actorIds"];

            string listValue = Encoding.UTF8.GetString(Convert.FromBase64String(actorsList64));
            List<int> actorIds = JsonConvert.DeserializeObject<List<int>>(listValue);

            if (dvdDto.Category.CategoryId != 0 ) 
            {
                ModelState.Remove("Category.CategoryDescription");
            }

            if (dvdDto.Producer.ProducerId != 0)
            {
                ModelState.Remove("Producer.ProducerName");
            }

            if (dvdDto.Studio.StudioId != 0)
            {
                ModelState.Remove("Studio.StudioName");
            }

            

            if (actorIds.Count() > 0)
            {
                ModelState.Remove("HasActors");
            }

            if (ModelState.IsValid)
            {

                using var transaction = _context.Database.BeginTransaction();

                try
                {

                    var category = dvdDto.Category;
                    var studio = dvdDto.Studio;
                    var producer = dvdDto.Producer;

                    var categoryId = dvdDto.Category.CategoryId;
                    var studioId = dvdDto.Studio.StudioId;
                    var producerId = dvdDto.Producer.ProducerId;

                    // If the user added new items then, add them to the database first and get the IDs.
                    if (categoryId == 0)
                    {
                        _context.Dvdcategories.Add(category);

                        _context.SaveChanges();
                        categoryId = category.CategoryId;
                    }

                    if (studioId == 0)
                    {
                        _context.Studios.Add(studio);

                        _context.SaveChanges();
                        studioId = studio.StudioId;
                    }

                    if (producerId == 0)
                    {
                        _context.Producers.Add(producer);

                        _context.SaveChanges();
                        producerId = producer.ProducerId;
                    }

                    var mDvdTitle = new Dvdtitle
                    {
                        DvdId = 0,
                        StudioId = studioId,
                        CategoryId = categoryId,
                        ProduceId = producerId,
                        DateReleased = dvdDto.DateReleased,
                        Rate = dvdDto.Rate,
                        PenaltyRate = dvdDto.PenaltyRate,
                        DvDname = dvdDto.Title,
                        Actors = _context.Actors.Where(x => actorIds.Contains(x.ActorId)).ToArray()
                    };

                    _context.Dvdtitles.Add(mDvdTitle);

                    _context.SaveChanges();

                    // Save the images to the folder in server.
                    var filePaths = await SaveImages(dvdDto.DvDImages, mDvdTitle);

                    foreach (string path in filePaths)
                    {
                        _context.DvDimages.Add(new DvDimage
                        {
                            DvDimageId = 0,
                            DvDnumber = mDvdTitle.DvdId,
                            DvdImage1 = path
                        });
                    }

                    await _context.SaveChangesAsync();


                    transaction.Commit();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred on our side. Please contact the IT department.");
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Dvdcategories, "CategoryId", "CategoryDescription", dvdDto.Category.CategoryId);
            ViewData["ProduceId"] = new SelectList(_context.Producers, "ProducerId", "ProducerName", dvdDto.Producer.ProducerId);
            ViewData["StudioId"] = new SelectList(_context.Studios, "StudioId", "StudioName", dvdDto.Studio.StudioId);
            return View(dvdDto);
        }

        private async Task<List<string>> SaveImages(IFormFileCollection files, Dvdtitle mDvdTitle)
        {

            string uploadFolder = Path.Combine("uploads", $"{mDvdTitle.DvDname}_{mDvdTitle.DvdId}");
            string contentPath = Path.Combine(_webHostEnvironment.WebRootPath, uploadFolder);

            if (!Directory.Exists(contentPath))
            {
                Directory.CreateDirectory(contentPath);
            }

            List<string> filePaths = new List<string>();

            foreach (var file in files)
            {
                if (file != null)
                {
                    var InputFileName = Path.GetFileName(file.FileName);

                    // File paths that will be saved to the database.
                    filePaths.Add(Path.Combine(uploadFolder, InputFileName));

                    var ServerSavePath = Path.Combine(contentPath, InputFileName);
                    //Save file to uploads folder  
                    using (Stream fileStream = new FileStream(ServerSavePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }

            return filePaths;
        }

        public IActionResult AddActorItem()
        {
            ViewData["ActorList"] = new SelectList(_context.Actors, "ActorId", "ActorName");

            return PartialView("_ActorsSelction", new Actor());
        }

        // GET: Catalog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvdtitle = await _context.Dvdtitles.FindAsync(id);
            if (dvdtitle == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Dvdcategories, "CategoryId", "CategoryDescription", dvdtitle.CategoryId);
            ViewData["ProduceId"] = new SelectList(_context.Producers, "ProducerId", "ProducerName", dvdtitle.ProduceId);
            ViewData["StudioId"] = new SelectList(_context.Studios, "StudioId", "StudioName", dvdtitle.StudioId);
            return View(dvdtitle);
        }

        // POST: Catalog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DvdId,DvDname,ProduceId,CategoryId,StudioId,DateReleased,Rate,PenaltyRate")] Dvdtitle dvdtitle)
        {
            if (id != dvdtitle.DvdId)
            {
                return NotFound();
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dvdtitle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DvdtitleExists(dvdtitle.DvdId))
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
            ViewData["CategoryId"] = new SelectList(_context.Dvdcategories, "CategoryId", "CategoryDescription", dvdtitle.CategoryId);
            ViewData["ProduceId"] = new SelectList(_context.Producers, "ProducerId", "ProducerName", dvdtitle.ProduceId);
            ViewData["StudioId"] = new SelectList(_context.Studios, "StudioId", "StudioName", dvdtitle.StudioId);
            return View(dvdtitle);
        }

        // GET: Catalog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvdtitle = await _context.Dvdtitles
                .Include(d => d.Category)
                .Include(d => d.Produce)
                .Include(d => d.Studio)
                .FirstOrDefaultAsync(m => m.DvdId == id);
            if (dvdtitle == null)
            {
                return NotFound();
            }

            return View(dvdtitle);
        }

        // POST: Catalog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dvdtitle = await _context.Dvdtitles.FindAsync(id);
            _context.Dvdtitles.Remove(dvdtitle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CompleteDetails(int? pageNumber)
        {
            var allDvdList = _context.Dvdtitles
                .Include(d => d.Category)
                .Include(d => d.Produce)
                .Include(d => d.Studio)
                .Include(d => d.Dvdcopies)
                .Include(d => d.Actors)
                .Include(d => d.DvDimages).OrderBy(d => d.DateReleased);

            int pageSize = 1;

            return View(await PaginatedList<Dvdtitle>.CreateAsync(allDvdList.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        public async Task<IActionResult> LeastPopular()
        {

            // Selecting DvD copies with no loans or where last loan was more than 31 days.
            var leastPopularDvs = _context.Dvdcopies
                .Include(c => c.Loans)
                .Include(c => c.Dvd)
                .Where(
                c => c.Loans.OrderBy(l => l.DateOut)
                .LastOrDefault().DateOut.Value.AddDays(31) < DateTime.Now || c.Loans.Count < 1);

            var unPopularDvd = _context.Dvdtitles.Include(c => c.Dvdcopies).Where(d => d.Dvdcopies.Count == leastPopularDvs.Where(c => c.DvdId == d.DvdId).Count());


            return View(await unPopularDvd.ToListAsync());
        }

        private bool DvdtitleExists(int id)
        {
            return _context.Dvdtitles.Any(e => e.DvdId == id);
        }
    }
}
