using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [Authorize(Policy = "UserOrAdmin")]
        [HttpGet]
        public ActionResult<List<BookDto>> Get([FromQuery] string title, [FromQuery] string author, [FromQuery] string genre)
        {
            var books = _bookService.GetBooks(title, author, genre);
            return Ok(books);
        }
        [Authorize(Policy = "UserOrAdmin")]
        [HttpGet("{id}")]
        public ActionResult<BookDto> Get(int id)
        {
            var book = _bookService.GetBook(id);
            if (book == null) return NotFound();
            return Ok(book);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("create")]
        public ActionResult<BookDto> Create([FromBody] CreateBookDto dto)
        {
            var book = _bookService.AddBook(dto);
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public ActionResult<BookDto> Update(int id, [FromBody] BookDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var updated = _bookService.UpdateBook(dto);
            return Ok(updated);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _bookService.DeleteBook(id);
            return NoContent();
        }
    
}
}
