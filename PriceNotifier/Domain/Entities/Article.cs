using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        [Required(ErrorMessage = "Please Enter Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please Enter Summary")]
        public string Summary { get; set; }
        [Required(ErrorMessage = "Please Enter Body")]
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPublished { get; set; }
        [Required(ErrorMessage = "Please Enter Publication Date")]
        public DateTime DateAdded { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
