using Application.Interfaces;
using Application.DTOs;
using Infastructure.Entities;
using Domain.Entities;
namespace Application.Services
{
  public class BookService : IBookService
    {
        private readonly IApplicationDbContext _ctx;

        public BookService(IApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public BookDto AddBook(CreateBookDto dto)
        {
            var book = new Book {
                Title       = dto.Title,
                Author      = dto.Author,
                Genre       = dto.Genre,
                Price       = dto.Price,
                IsAvailable = true
            };
            _ctx.Books.Add(book);
            _ctx.SaveChangesAsync();

            return ToDto(book);
        }

        public void DeleteBook(int id)
        {
            var book = _ctx.Books.Find(id);
            if (book == null) throw new KeyNotFoundException("Book not found");
            _ctx.Books.Remove(book);
            _ctx.SaveChangesAsync();
        }

        public BookDto GetBook(int id)
        {
            var book = _ctx.Books.FirstOrDefault(b => b.Id == id);
            return book == null ? null : ToDto(book);
        }

        public List<BookDto> GetBooks(string title = null, string author = null, string genre = null)
        {
            var query = _ctx.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(b => b.Title.Contains(title));
            if (!string.IsNullOrWhiteSpace(author))
                query = query.Where(b => b.Author.Contains(author));
            if (!string.IsNullOrWhiteSpace(genre))
                query = query.Where(b => b.Genre == genre);

            return query
                .Select(b => ToDto(b))
                .ToList();
        }

        public BookDto UpdateBook(BookDto dto)
        {
            var book = _ctx.Books.Find(dto.Id);
            if (book == null) throw new KeyNotFoundException("Book not found");

            book.Title       = dto.Title;
            book.Author      = dto.Author;
            book.Genre       = dto.Genre;
            book.Price       = dto.Price;
            book.IsAvailable = dto.IsAvailable;

            _ctx.SaveChangesAsync();
            return ToDto(book);
        }

        private static BookDto ToDto(Book b)
            => new(b.Id, b.Title, b.Author, b.Genre, b.Price, b.IsAvailable);
    }
}
