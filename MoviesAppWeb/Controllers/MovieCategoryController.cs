using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using MoviesAppWeb.Data;
using Microsoft.AspNetCore.Authorization;

namespace MoviesAppWeb.Controllers
{
    [Authorize()]
    public class MovieCategoryController : Controller
    {

        private readonly ILogger<MovieCategoryController> _logger;

        private readonly ApplicationDbContext _context;

        public MovieCategoryController(ILogger<MovieCategoryController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");

            _context = context;
        }

        // GET: MovieCategory
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserName == userName);


            return _context.MovieCategories != null ? 
                          View(await _context.MovieCategories.Where(a=> a.DeletedAt == null && a.CreatedById == user.Id).OrderByDescending(c => c.CreatedAt).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.MovieCategories'  is null.");
        }

     

        // GET: MovieCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MovieCategory/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,CreatedAt,CreatedById,ModifiedAt,ModifiedById,DeletedAt,DeletedById")] MovieCategory movieCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movieCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movieCategory);
        }

        // GET: MovieCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MovieCategories == null)
            {
                return NotFound();
            }

            var movieCategory = await _context.MovieCategories.FindAsync(id);
            if (movieCategory == null)
            {
                return NotFound();
            }
            return View(movieCategory);
        }

        // POST: MovieCategory/Edit/5      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,CreatedAt,CreatedById,ModifiedAt,ModifiedById,DeletedAt,DeletedById")] MovieCategory movieCategory)
        {
            if (id != movieCategory.Id)
            {

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex.Message);

                    if (!MovieCategoryExists(movieCategory.Id))
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
            return View(movieCategory);
        }

        // GET: MovieCategory/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userName = User.Identity.Name;
                var user = _context.Users.FirstOrDefault(a => a.UserName == userName);
                var movieCategory = await _context.MovieCategories.FindAsync(id);
                movieCategory.DeletedById = user.Id;
                movieCategory.DeletedAt = DateTime.Now;
                _context.MovieCategories.Update(movieCategory);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Successfully deleted" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new { success = false, message = "Something went wrong!" });

            }

        }


        private bool MovieCategoryExists(int id)
        {
          return (_context.MovieCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
