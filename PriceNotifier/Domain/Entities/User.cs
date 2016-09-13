using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(25, ErrorMessage = "Name cannot be longer than 25 characters.")]
        [RegularExpression(@"^[а-яА-Яa-zA-Z]+$", ErrorMessage = "Username should contain only alphabetical characters")]
        public string Username { get; set; }
        public string SocialNetworkUserId { get; set; }
        public string SocialNetworkName { get; set; }
        public  string Token { get; set; }

        public List<Product> Products { get; set; }

    }
}
