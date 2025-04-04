using Assignment_3_CRUD.Data;
using Assignment_3_CRUD.Models;
//using Assignment_3_CRUD.Repositories; removed not needed
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment_3_CRUD.Controllers
{
    [Route("[controller]")]
    public class ReaderController : Controller
    {
        //private readonly IReaderRepository _readerRepository; removed not needed
        private readonly LMA_DBcontext _context;

        public ReaderController(LMA_DBcontext context)
        {
            _context = context;
        }

        //Display All Reader
        [HttpGet]
        public IActionResult ReaderListPage()
        {
            return View(_context.Readers.ToList());
        }
        //Get Reader Details
        [HttpGet("Details/{id}")]
        public IActionResult ReaderDetails(int id)
        {
            var reader = _context.Readers.Find(id);
            return reader != null ? View(reader) : NotFound();
        }
        //Show create form
        [HttpGet("AddReader")]
        public IActionResult AddReader()
        {
            return View();
        }
        //Add a new Reader
        [HttpPost("AddReader")]
        public IActionResult AddReader(Reader newReader)
        {

            if (ModelState.IsValid)
            {
                _context.Readers.Add(newReader);
                _context.SaveChanges();
                return RedirectToAction(nameof(ReaderListPage)); // Redirect to Reader List page
            }

            return View(newReader);
        }

        // Show edit form
        [HttpGet("Edit/{id}")]
        public IActionResult EditReader(int id)
        {
            var reader = _context.Readers.Find(id);
            return reader != null ? View(reader) : NotFound();
        }

        // Update reader  details
        [HttpPost("Edit/{id}")]
        public IActionResult EditReader(int id, Reader reader)
        {
            if (id != reader.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(reader).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(ReaderListPage)); // Redirect to Reader List page
            }

            return View(reader);
        }
        

        

        //Show delete confirmation
        [HttpGet("Delete/{id}")]
        public IActionResult DeleteReader(int id)
        {
            var reader = _context.Readers.Find(id);
            return reader != null ? View(reader) : NotFound();
        }

        //Confirm and delete reader
        [HttpPost("Delete/{id}")]
        public IActionResult ConfirmDelete(int id)
        {
            var reader = _context.Readers.Find(id);
            if (reader == null) return NotFound();

            _context.Readers.Remove(reader);
            _context.SaveChanges();
            return RedirectToAction(nameof(ReaderListPage));
        }
        //Helper Method: Find book or return null - removed not needed
        //private Reader FindOrFail(int id) => _readerRepository.GetReaderById(id);

        // Search Reader by Name 
        [HttpGet("Search")]
        public IActionResult Search(string query)
        {
            var readers = _context.Readers
                .Where(r => r.FullName.Contains(query))
                .ToList();

            return View("SearchResults", readers);
        }
        private IActionResult NotFound()
        {
            //add 404 page
            return View("");
        }


    }
}
