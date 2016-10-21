using System;

namespace PriceNotifier.DTO
{
    public class ArticleDto
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPublished { get; set; }
        public DateTime DateAdded { get; set; }
        public int ProductId { get; set; }
    }
}