using System.ComponentModel.DataAnnotations;

namespace TwitterAssignmentApi.Models
{
    public class TweetReaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TweetId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ReactionType { get; set; }
        public virtual User User { get; set; }
        public virtual Tweet Tweet { get; set; }
    }
}
