﻿using System.ComponentModel.DataAnnotations;

namespace PriceNotifier.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(25, ErrorMessage = "Name cannot be longer than 25 characters.")]
        [RegularExpression(@"^[а-яА-Яa-zA-Z]+$", ErrorMessage = "Username should contain only alphabetical characters")]
        public string Username { get; set; }
        public string SocialNetworkUserId { get; set; }
        public string SocialNetworkName { get; set; }
    }
}