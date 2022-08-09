namespace TwitterAssignmentApi.Dtos
{
    public class UpdateTweetDto
    {
        public int Id { get; set; }
        public int? ParentTweetId { get; set; }
        public string Message { get; set; }
        public DateTime TweetDateTime { get; set; }
    }
}
