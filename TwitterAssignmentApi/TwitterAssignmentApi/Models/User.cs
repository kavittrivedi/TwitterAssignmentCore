namespace TwitterAssignmentApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string LoginId { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public virtual List<Tweet> Tweets { get; set; }
        public virtual List<TweetReaction> TweetReactions { get; set; }
    }
}
