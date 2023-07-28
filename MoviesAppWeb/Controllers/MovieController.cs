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
using X.PagedList;
using NLog;


namespace MoviesAppWeb.Controllers
{
    [Authorize()]
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;

        private readonly ApplicationDbContext _context;

        public MovieController(ILogger<MovieController> logger, ApplicationDbContext context)
        {
            _logger = logger;
           
            _context = context;

        }

        // GET: Movie
        public async Task<IActionResult> Index(int page = 1)
        {
            _logger.LogInformation("Hello, this is the movie index!");
          
            var userName = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserName == userName);


            var movies = await _context.Movies.Include(m => m.MovieCategory).Where(a => a.DeletedAt == null && a.CreatedById == user.Id).OrderByDescending(c => c.CreatedAt).ToPagedListAsync(page, 5);
            return View(movies);
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.MovieCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

    
        // GET: Movie/Create
        public IActionResult Create()
        {

            ViewData["MovieCategoryId"] = new SelectList(_context.MovieCategories.Where(a => a.DeletedAt == null), "Id", "Name");
            return View();
        }

        // POST: Movie/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Summary,Description,ReleaseDate,CoverImageFilePath,CoverImageFile, MovieCategoryId,Id,CreatedAt,CreatedById,ModifiedAt,ModifiedById,DeletedAt,DeletedById")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;
                var user = _context.Users.FirstOrDefault(a => a.UserName == userName);
                movie.CreatedAt = DateTime.Now;
                movie.CreatedById = user.Id;


                #region ImageUpload

                if (movie.CoverImageFile != null)
                {
                    var guid = Guid.NewGuid();
                    var fileName = guid + Path.GetExtension(movie.CoverImageFile.FileName);

                    string path = Path.Combine($"/upload/movieImages", fileName).Replace("\\", "/");
                    string savePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/upload/movieImages", fileName);
                    try
                    {
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await movie.CoverImageFile.CopyToAsync(stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);

                        ModelState.AddModelError("ImageFile", "Something went wrong!");
                        return View(movie);
                    }

                    movie.CoverImageFilePath = path;

                }

                #endregion



                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieCategoryId"] = new SelectList(_context.MovieCategories.Where(a => a.DeletedAt == null), "Id", "Name", movie.MovieCategoryId);
            return View(movie);
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(a => a.UserName == userName);
            if (movie.CreatedById != user.Id)
            {
                return Unauthorized();
            }


            ViewData["MovieCategoryId"] = new SelectList(_context.MovieCategories.Where(a => a.DeletedAt == null), "Id", "Name");
            return View(movie);
        }

        // POST: Movie/Edit/5      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Summary,Description,ReleaseDate,CoverImageFilePath,CoverImageFile, MovieCategoryId,Id,CreatedAt,CreatedById,ModifiedAt,ModifiedById,DeletedAt,DeletedById")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(a => a.UserName == userName);
            var movieFromDb =  _context.Movies.Find(id);
            if (movieFromDb == null)
            {
                return NotFound();
            }
            if (movieFromDb.CreatedById != user.Id)
            {
                return Unauthorized();
            }


            if (ModelState.IsValid)
            {
                try
                {

                    #region ImageUpload

                    if (movie.CoverImageFile != null)
                    {
                        var guid = Guid.NewGuid();
                        var fileName = guid + Path.GetExtension(movie.CoverImageFile.FileName);

                        string path = Path.Combine($"/upload/movieImages", fileName).Replace("\\", "/");
                        string savePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/upload/movieImages", fileName);
                        try
                        {
                            using (var stream = new FileStream(savePath, FileMode.Create))
                            {
                                await movie.CoverImageFile.CopyToAsync(stream);
                            }
                        }
                        catch
                        {
                            ModelState.AddModelError("ImageFile", "Something went wrong!");
                            return View(movie);
                        }

                        movie.CoverImageFilePath = path;

                    }

                    #endregion


                 
                    movie.ModifiedAt = DateTime.Now;
                    movie.ModifiedById = user.Id;

                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!MovieExists(movie.Id))
                    {
                        _logger.LogError(ex.Message);
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieCategoryId"] = new SelectList(_context.MovieCategories.Where(a => a.DeletedAt == null), "Id", "Name");
            return View(movie);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userName = User.Identity.Name;
                var user = _context.Users.FirstOrDefault(a => a.UserName == userName);
                var movie = await _context.Movies.FindAsync(id);
                movie.DeletedById = user.Id;
                movie.DeletedAt = DateTime.Now;
                _context.Movies.Update(movie);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Successfully deleted" });
            }
            catch (Exception ex )
            {
                _logger.LogError(ex.Message);
                return Json(new { success = false, message = "Something went wrong!" });

            }

        }

        private bool MovieExists(int id)
        {
            return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
