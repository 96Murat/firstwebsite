using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public int Count { get; set; }
        public int MaxCount { get; set; }
        public object Result { get; set; }
    }
}
