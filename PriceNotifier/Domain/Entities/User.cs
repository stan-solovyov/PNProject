using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public string SocialNetworkName { get; set; }
        public  string Token { get; set; }

        public List<Product> Products { get; set; }

    }
}
