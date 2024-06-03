using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avito.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public static Response Ok() => new Response() { IsSuccess = true };
        public static Response Error(string message) => new Response() { IsSuccess = false, ErrorMessage = message };
    }
}
