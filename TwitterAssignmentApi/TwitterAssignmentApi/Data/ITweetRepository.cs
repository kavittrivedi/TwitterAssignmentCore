using TwitterAssignmentApi.Dtos;
using TwitterAssignmentApi.Models;

namespace TwitterAssignmentApi.Data
{
    public interface ITweetRepository
    {
        Task<ServiceResponse<GetTweetDto>> AddTweet(string username, AddTweetDto addTweetDto);

        Task<ServiceResponse<List<GetTweetDto>>> GetTweets(string username);

        Task<ServiceResponse<GetTweetDto>> ReplyTweet(string username, string reactingUserName, int id, AddTweetDto addTweetDto);

        Task<ServiceResponse<List<GetTweetDto>>> DeleteTweet(string username, int id);

        Task<ServiceResponse<GetTweetDto>> UpdateTweet(string username, int id, UpdateTweetDto updateTweetDto);
    }
}
