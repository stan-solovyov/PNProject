using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public int  Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string  ProductId { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public bool Checked { get; set; }  

        public int UserId { get; set; }
    }
}
