namespace TwitterAssignmentApi.Dtos
{
    public class AddTweetDto
    {
        public int? ParentTweetId { get; set; }
        public string Message { get; set; }
        public DateTime TweetDateTime { get; set; }
    }
}
