namespace BLL
{
    public class UserFromDbWithCount
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string SocialNetworkUserId { get; set; }
        public string SocialNetworkName { get; set; }
        public int CountTrackedItems { get; set; }
    }
}