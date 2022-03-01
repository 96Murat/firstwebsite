using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Models
{
    public class RequestBase
    {
        public class List
        {
            public bool GetChildrenCount { get; set; }
            public bool IsOrderByAsc { get; set; } = true;
            public int RowCount { get; set; } = 4;
            public int Page { get; set; } = 1;
        }
    }
}
