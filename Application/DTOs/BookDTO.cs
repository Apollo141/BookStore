using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public record BookDto(

        int Id,
        string Title,
        string Author,
        string Genre,
        decimal Price,
        bool IsAvailable
    );
}
