﻿using Assignment_3_CRUD.Models;
using System.Collections.Generic;

namespace Assignment_3_CRUD.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBookById(int id);
        void AddBook(Book book);
        void Update(Book book); 
        void DeleteBook(int id);

        void SetToAvailable(int id);

        void SetToNotAvailable(int id);
    }
}