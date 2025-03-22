using Assignment_3_CRUD___Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace Assignment_3_CRUD___Model.Repositories
{
    public class BookRepository : IBookRepository
    {
        private static List<Book> books = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "Great Big Beautiful Life",
                Author = "Emily Henry",
                PublishedDate = new DateOnly(2025, 3, 1),
                Genre = "Fiction",
                Availability = true
            },
            new Book
            {
                Id = 2,
                Title = "Sunrise on the Reaping",
                Author = "Suzanne Collins",
                PublishedDate = new DateOnly(2025, 3, 1),
                Genre = "Dystopian",
                Availability = true
            },
            new Book
            {
                Id = 3,
                Title = "The Knight and the Moth",
                Author = "Rachel Gillig",
                PublishedDate = new DateOnly(2025, 3, 1),
                Genre = "Fantasy",
                Availability = true
            }
        };

        public IEnumerable<Book> GetAllBooks()
        {
            return books;
        }

        public Book GetBookById(int id)
        {
            return books.FirstOrDefault(b => b.Id == id);
        }

        public void AddBook(Book book)
        {
            book.Id = books.Count() + 1;
            books.Add(book);
        }

        public void Update(Book book)
        {
            var existingBook = books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.PublishedDate = book.PublishedDate;
                existingBook.Genre = book.Genre;
                existingBook.Availability = book.Availability;
            }
        }


        public void DeleteBook(int id)
        {
            var book = GetBookById(id);
            if (book != null)
            {
                books.Remove(book);
            }
        }
    }
}