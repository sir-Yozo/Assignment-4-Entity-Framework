using Assignment_3_CRUD.Data;
using Assignment_3_CRUD.Models;
//using Assignment_3_CRUD.Repositories; removed - Not needed
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace Assignment_3_CRUD.Controllers
{
    [Route("[controller]")]
    public class BookController : Controller
    {
        //private readonly IBookRepository _bookRepository; removed - Not needed
        private readonly LMA_DBcontext _context;

        public BookController(LMA_DBcontext context)
        {
            _context = context;
        }


        // Display all books
        [HttpGet]
        public IActionResult BookList()
        {
            return View(_context.Books.ToList());
        }
        // Display book details
        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var book = _context.Books.Find(id);
            return book != null ? View(book) : NotFound();
        }

        //Show create form
        [HttpGet("Create")]
        public IActionResult AddBook()
        {
            return View();
        }

        //Add a new book
        [HttpPost("Create")]
        public IActionResult Create(Book newBook)
        {

            if (ModelState.IsValid)
            {
                _context.Books.Add(newBook);
                _context.SaveChanges();
                return RedirectToAction(nameof(BookList)); // Redirect to book List page
            }

            return View(newBook);
            
                
        }

        // Show edit form
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var book = _context.Books.Find(id);
            return book != null ? View(book) : NotFound();
        }

        // Update book details
        [HttpPost("Edit/{id}")]
        public IActionResult Edit(int id, Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(book).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(BookList)); // Redirect to book List page
            }

            return View(book);
        }

        //Show delete confirmation
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var book = _context.Books.Find(id);
            return book != null ? View(book) : NotFound();
        }

        //Confirm and delete book
        [HttpPost("Delete/{id}")]
        public IActionResult ConfirmDelete(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return NotFound();

            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(BookList));
        }

        //Helper Method: Find book or return null (Removed- not needed)
        //private Book FindOrFail(int id) => _bookRepository.GetBookById(id);

        private IActionResult NotFound()
        {
            //add 404 page
            return View("");
        }
    }
}
