using Assignment_3_CRUD.Data;
using Assignment_3_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace Assignment_3_CRUD.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class BookController : Controller
    {
        private readonly LMA_DBcontext _context;

        public BookController(LMA_DBcontext context)
        {
            _context = context;
        }

        // Display all books
        [HttpGet]
        public async Task<IActionResult> BookList()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        // Display book details
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books.FindAsync(id);
            return book != null ? View(book) : NotFound();
        }

        // Show create form
        [HttpGet("Create")]
        public IActionResult AddBook()
        {
            return View();
        }

        // Add a new book
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Book newBook)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(newBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BookList));
            }

            return View(newBook);
        }

        // Show edit form
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            return book != null ? View(book) : NotFound();
        }

        // Update book details
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BookList));
            }

            return View(book);
        }

        // Show delete confirmation
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            return book != null ? View(book) : NotFound();
        }

        // Confirm and delete book
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BookList));
        }

        // Search Books by Title, Author, or Genre
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string query)
        {
            var books = await _context.Books
                .Where(b => b.Title.Contains(query) || b.Author.Contains(query) || b.Genre.Contains(query))
                .ToListAsync();

            return View("SearchResults", books);
        }


    }
}
