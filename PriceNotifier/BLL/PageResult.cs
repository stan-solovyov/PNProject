using System.Collections.Generic;

namespace BLL
{
    public class PageResult<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int TotalItems { get; set; }
    }
}