using Assignment_3_CRUD___Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_3_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReaderController : Controller
    {

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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(readers);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var reader = readers.FirstOrDefault(r => r.Id == id);
            if (reader == null)
            {
                return NotFound();
            }
            return Ok(reader);
        }

        [HttpPost]
        public IActionResult Post(Reader newReader)
        {
            readers.Add(newReader);
            return Ok(newReader);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Reader updatedReader)
        {
            var reader = readers.FirstOrDefault(r => r.Id == id);
            if (reader == null)
            {
                return NotFound();
            }

            reader.FullName = updatedReader.FullName;
            reader.Email = updatedReader.Email;
            reader.MembershipDate = updatedReader.MembershipDate;
            reader.PhoneNumber = updatedReader.PhoneNumber;
            reader.Address = updatedReader.Address;

            return Ok(reader);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var reader = readers.FirstOrDefault(r => r.Id == id);
            if (reader == null)
            {
                return NotFound();
            }

            readers.Remove(reader);
            return Ok(reader);
        }
    }
}
