using System.Collections.Generic;
using Assignment_3_CRUD.Models;

namespace Assignment_3_CRUD.Repositories
{
    public interface IReaderRepository
    {
        IEnumerable<Reader> GetAllReaders();
        Reader GetReaderById(int id);
        void AddReader(Reader reader);
        bool UpdateReader(Reader reader);
        void DeleteReader(int id);
    }
}
