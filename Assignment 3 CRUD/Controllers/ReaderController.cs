using Assignment_3_CRUD___Model.Models;
using Assignment_3_CRUD___Model.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment_3_CRUD___Model.Controllers
{
    [Route("[controller]")]
    public class ReaderController : Controller
    {
        private readonly IReaderRepository _readerRepository;
        public ReaderController(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        //Display All Reader
        [HttpGet]
        public IActionResult ReaderListPage()
        {
            return View(_readerRepository.GetAllReaders());
        }
        //Get Reader Details
        [HttpGet("Details/{id}")]
        public IActionResult ReaderDetails(int id)
        {
            var reader = _readerRepository.GetReaderById(id);
            if (reader == null)
            {
                return NotFound();
            }
            return View(reader);
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
                _readerRepository.AddReader(newReader);
                return RedirectToAction(nameof(ReaderListPage)); //Redirect to Reader List page
            }

            return View(newReader);
        }

        // Show edit form
        [HttpGet("Edit/{id}")]
        public IActionResult EditReader(int id)
        {
            var reader = FindOrFail(id);
            return reader != null ? View(reader) : NotFound();
        }

        // Update book details
        [HttpPost("Edit/{id}")]
        public IActionResult EditReader(int id, Reader reader)
        {
            if (id != reader.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _readerRepository.UpdateReader(reader);
                return RedirectToAction(nameof(ReaderListPage)); //Redirect to reader List page
            }

            return View(reader);
        }
        //Helper Method: Find book or return null
        private Reader FindOrFail(int id) => _readerRepository.GetReaderById(id);

        private IActionResult NotFound()
        {
            //add 404 page
            return View("");
        }

        //Show delete confirmation
        [HttpGet("Delete/{id}")]
        public IActionResult DeleteReader(int id)
        {
            var reader = FindOrFail(id);
            return reader != null ? View(reader) : NotFound();
        }

        //Confirm and delete reader
        [HttpPost("Delete/{id}")]
        public IActionResult ConfirmDelete(int id)
        {
            var book = FindOrFail(id);
            if (book == null) return NotFound();

            _readerRepository.DeleteReader(id);
            return RedirectToAction(nameof(ReaderListPage));
        }


    }
}
