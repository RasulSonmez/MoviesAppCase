using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesAppWeb.Data;
using MoviesAppWeb.Models;
using System.Diagnostics;
using X.PagedList;

namespace MoviesAppWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            _logger.LogInformation("Hello, this is the index!");
            var lastMovies = await _context.Movies.Include(m => m.MovieCategory).Where(a => a.DeletedAt == null)
                .OrderByDescending(c => c.CreatedAt).ToPagedListAsync(page, 8);
            return View(lastMovies);
        }

        public async Task<IActionResult> MovieDetails(int? id)
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 404)
            {
                return View("NotFound");
            }
            if (statuscode == 401)
            {
                return View("AccessDenied");
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

          
        }
    }
}