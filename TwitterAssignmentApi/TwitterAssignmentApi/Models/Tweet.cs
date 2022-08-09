using System.ComponentModel.DataAnnotations;

namespace TwitterAssignmentApi.Models
{
    public class Tweet
    {
        public int Id { get; set; }

        public int? ParentTweetId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(144)]
        public string Message { get; set; }

        [Required]
        public DateTime TweetDateTime { get; set; }
        public virtual User User { get; set; }
        public virtual Tweet ParentTweet { get; set; }
        public virtual List<TweetReaction> TweetReactions { get; set; }
    }
}
