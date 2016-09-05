using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLL
{
    public class PageResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalItems { get; set; }

    }
}