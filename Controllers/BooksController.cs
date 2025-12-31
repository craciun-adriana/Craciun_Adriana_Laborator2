using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Craciun_Adriana_Laborator2.Data;
using Craciun_Adriana_Laborator2.Models;

namespace Craciun_Adriana_Laborator2.Controllers
{
    public class BooksController : Controller
    {
        private readonly Craciun_Adriana_Laborator2Context _context;

        public BooksController(Craciun_Adriana_Laborator2Context context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["CurrentFilter"] = searchString;

            var book = from b in _context.Book
                       join a in _context.Author on b.AuthorID equals a.Id into authorGroup
                       from a in authorGroup.DefaultIfEmpty()
                       join g in _context.Genre on b.GenreID equals g.Id into genreGroup
                       from g in genreGroup.DefaultIfEmpty()
                       select new BookViewModel
                       {
                           ID = b.ID,
                           Title = b.Title,
                           Author = a.FullName,
                           Price = b.Price,
                           Genre = g.Name
                       };

            if (!String.IsNullOrEmpty(searchString))
            { 
                book = book.Where(b => b.Title.Contains(searchString)); 
            }

            switch (sortOrder)
            {
                case "title_desc":
                    book = book.OrderByDescending(b => b.Title);
                    break;
                case "Price":
                    book = book.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    book = book.OrderByDescending(b => b.Price);
                    break;
                default:
                    book = book.OrderBy(b => b.Title);
                    break;
            }
            return View(await book.AsNoTracking().ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Genre)
                .Include(b => b.Author)
                .Include(b => b.Orders)
                .ThenInclude(o => o.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "Id", "Name");
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "Id", "FullName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,AuthorID,Price,GenreID")] Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "Id", "Name", book.GenreID);
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "Id", "FullName", book.AuthorID);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "Id", "Name", book.GenreID);
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "Id", "FullName", book.AuthorID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,AuthorID,Price,GenreID")] Book book)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookToUpdate = await _context.Book.FirstOrDefaultAsync(b => b.ID == id);
            
            if (await TryUpdateModelAsync<Book>(
                bookToUpdate,
                "",
                b => b.Title, b => b.AuthorID, b => b.Price, b => b.GenreID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }

            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "Id", "Name", book.GenreID);
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "Id", "FullName", book.AuthorID);
            return View(bookToUpdate);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book); 
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.ID == id);
        }
    }
}
