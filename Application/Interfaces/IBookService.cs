using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBookService
    {
        BookDto AddBook(CreateBookDto dto);
        BookDto UpdateBook(BookDto dto);
        void DeleteBook(int id);
        BookDto GetBook(int id);
        List<BookDto> GetBooks(string title = null, string author = null, string genre = null);
    }
}
