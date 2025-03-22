using System;
using System.Collections.Generic;
using System.Linq;
using Assignment_3_CRUD___Model.Models;

namespace Assignment_3_CRUD___Model.Repositories
{
    public class ReaderRepository : IReaderRepository
    {
        // Static test data (Simulating a database)
        private static List<Reader> readers = new List<Reader>
        {
            new Reader
            {
                Id = 1,
                FullName = "John Doe",
                Email = "john.doe@example.com",
                MembershipDate = new DateTime(2020, 1, 15),
                PhoneNumber = "123-456-7890",
                Address = "123 Main St, Anytown, USA"
            },
            new Reader
            {
                Id = 2,
                FullName = "Jane Smith",
                Email = "jane.smith@example.com",
                MembershipDate = new DateTime(2021, 5, 22),
                PhoneNumber = "987-654-3210",
                Address = "456 Elm St, Othertown, USA"
            },
            new Reader
            {
                Id = 3,
                FullName = "Alice Johnson",
                Email = "alice.johnson@example.com",
                MembershipDate = new DateTime(2019, 9, 30),
                PhoneNumber = "403-123-4567",
                Address = "789 Oak St, Sometown, USA"
            }
        };

        // Retrieve all readers
        public IEnumerable<Reader> GetAllReaders()
        {
            return readers;
        }

        // Retrieve a single reader by ID
        public Reader GetReaderById(int id)
        {
            return readers.FirstOrDefault(r => r.Id == id);
        }

        // Add a new reader
        public void AddReader(Reader reader)
        {
            int nextId = (readers.Any() ? readers.Max(r => r.Id) : 0) + 1;
            reader.Id = nextId;
            readers.Add(reader);
        }

        // Update an existing reader
        public bool UpdateReader(Reader reader)
        {
            var existingReader = readers.FirstOrDefault(r => r.Id == reader.Id);
            if (existingReader != null)
            {
                existingReader.FullName = reader.FullName;
                existingReader.Email = reader.Email;
                existingReader.MembershipDate = reader.MembershipDate;
                existingReader.PhoneNumber = reader.PhoneNumber;
                existingReader.Address = reader.Address;
                return true;
            }
            return false;
        }

        // Delete a reader by ID
        public void DeleteReader(int id)
        {
            var reader = GetReaderById(id);
            if (reader != null)
            {
                readers.Remove(reader);
            }
        }
    }
}
