using Assignment_3_CRUD___Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_3_CRUD___Model.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        //sample data
        private static List<Book> books = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                PublishedDate = new DateTime(1960, 7, 11),
                Genre = "Fiction",
                Availability = true
            },
            new Book
            {
                Id = 2,
                Title = "1984",
                Author = "George Orwell",
                PublishedDate = new DateTime(1949, 6, 8),
                Genre = "Dystopian",
                Availability = true
            },
            new Book
            {
                Id = 3,
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                PublishedDate = new DateTime(1925, 4, 10),
                Genre = "Tragedy",
                Availability = false
            }
        };

        //get all books
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(books);


        }

        //get a book by ID
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);

        }

        //adding new book
        [HttpPost]
        public IActionResult Post(Book newBook)
        {
            books.Add(newBook);
            return Ok(books);

        }

        //updating a book by ID
        [HttpPut("{id}")]
        public IActionResult Put(int id, Book updatedBook)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.PublishedDate = updatedBook.PublishedDate;
            book.Genre = updatedBook.Genre;
            book.Availability = updatedBook.Availability;

            return Ok(book);
        }

        //delete a book by ID
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            books.Remove(book);
            return Ok(book);

        }

    }
}
