using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceNotifier.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string SocialNetworkUserId { get; set; }
        public string SocialNetworkName { get; set; }
    }
}