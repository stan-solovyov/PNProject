namespace BLL.Models
{
    public class UserFromDbWithCount
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string SocialNetworkUserId { get; set; }
        public string SocialNetworkName { get; set; }
        public string Email { get; set; }
        public int CountTrackedItems { get; set; }
    }
}