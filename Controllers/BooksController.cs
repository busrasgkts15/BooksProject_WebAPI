using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using Repositories.EFCore;
using Repositories.Contracts;
using Services.Contracts;

namespace WebApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly IServicesManager _servicesManager;

        public BooksController(IServicesManager servicesManager)
        {
            _servicesManager = servicesManager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _servicesManager.BookService.GetAllBooks(false);
                return Ok(books);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook(int id)
        {

            try
            {
                var book = _servicesManager.BookService.GetOneBookById(id, false);

                if (book is null)
                {
                    return NotFound(); // 404
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult CreatedOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                {
                    return BadRequest(); // 400

                }

                _servicesManager.BookService.CreateOneBook(book);


                return StatusCode(201, book);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest();

                _servicesManager.BookService
                     .UpdateOneBook(id, book, true);

                return NoContent();// 204s

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }


        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook(int id)
        {
            try
            {

                _servicesManager.BookService.DeleteOneBook(id, false);
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {

            try
            {
                var entity = _servicesManager.BookService.GetOneBookById(id, true);

                if (entity is null)
                {
                    return NotFound();
                }

                bookPatch.ApplyTo(entity);
                _servicesManager.BookService.UpdateOneBook(id, entity, true);

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

    }

}
