using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AuthDto
    {
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
