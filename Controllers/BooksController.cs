using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using Repositories.EFCore;
using Repositories.Contracts;

namespace WebApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly IRepositoryManager _manager;

        public BooksController(IRepositoryManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _manager.Book.GetAllBooks(false);
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
                var book = _manager.Book.GetOneBookById(id, false);

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

                _manager.Book.CreateOneBook(book);
                _manager.Save();

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
                // check book ?
                var entity = _manager.Book.GetOneBookById(
                    id, true);

                if (entity is null)
                {
                    return NotFound();
                }

                if (id != book.Id)
                {
                    return BadRequest("İnvalid Argument");

                }


                entity.Title = book.Title;
                entity.Price = book.Price;
                _manager.Save();
                return Ok(book);

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
                var removebook = _manager.Book.GetOneBookById(id, false);

                if (removebook is null)
                {
                    return NotFound();
                }

                _manager.Book.DeleteOneBook(removebook);
                _manager.Save();
                return Ok(removebook);
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
                var entity = _manager.Book.GetOneBookById(id, true);

                if (entity is null)
                {
                    return NotFound();
                }

                bookPatch.ApplyTo(entity);
                _manager.Book.Update(entity);
                
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

    }

}
