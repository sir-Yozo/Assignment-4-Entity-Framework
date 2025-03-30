using Assignment_3_CRUD.Models;
using Assignment_3_CRUD.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_3_CRUD.Controllers
{
    [Route("[controller]")]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }


        // Display all books
        [HttpGet]
        public IActionResult BookList()
        {
            return View(_bookRepository.GetAllBooks());
        }
        // Display book details
        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var book = _bookRepository.GetBookById(id);
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
                _bookRepository.AddBook(newBook);
                return RedirectToAction(nameof(BookList)); //Redirect to book List page
            }

            return View(newBook);
            
                
        }

        // Show edit form
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var book = FindOrFail(id);
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
                _bookRepository.Update(book);
                return RedirectToAction(nameof(BookList)); //Redirect to book List page
            }

            return View(book);
        }

        //Show delete confirmation
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var book = FindOrFail(id);
            return book != null ? View(book) : NotFound();
        }

        //Confirm and delete book
        [HttpPost("Delete/{id}")]
        public IActionResult ConfirmDelete(int id)
        {
            var book = FindOrFail(id);
            if (book == null) return NotFound();

            _bookRepository.DeleteBook(id);
            return RedirectToAction(nameof(BookList));
        }

        //Helper Method: Find book or return null
        private Book FindOrFail(int id) => _bookRepository.GetBookById(id);

        private IActionResult NotFound()
        {
            //add 404 page
            return View("");
        }
    }
}
