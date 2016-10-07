namespace Messages
{
    public class UpdatedPrice
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
    }
}