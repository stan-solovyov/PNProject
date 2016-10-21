using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPublished { get; set; }
        [Required]
        public DateTime DateAdded { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
