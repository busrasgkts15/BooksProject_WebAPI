﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApı.Models;
using WebApı.Repositories;

namespace WebApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly RepositoryContext _context;

        public BooksController(RepositoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _context.Books.ToList();
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
                var book = _context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

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

                _context.Books.Add(book);
                _context.SaveChanges();
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
                var entity = _context.Books.Where(
                    b => b.Id.Equals(id)).SingleOrDefault();

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
                _context.SaveChanges();
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
                var removebook = _context.Books.Where(
                    b => b.Id.Equals(id)).SingleOrDefault();

                if (removebook is null)
                {
                    return NotFound();
                }

                _context.Books.Remove(removebook);
                _context.SaveChanges();
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

            var entity = _context.Books.Where(
                b => b.Id.Equals(id)).SingleOrDefault();

            if (entity is null)
            {
                return NotFound();
            }

            bookPatch.ApplyTo(entity);
            _context.SaveChanges();
            return NoContent();


        }

    }
}
