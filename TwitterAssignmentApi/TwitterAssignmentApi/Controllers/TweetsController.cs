using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitterAssignmentApi.Data;
using TwitterAssignmentApi.Dtos;

namespace TwitterAssignmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TweetsController : ControllerBase
    {
        private readonly ITweetRepository _tweetRepository;

        public TweetsController(ITweetRepository tweetRepository)
        {
            _tweetRepository = tweetRepository;
        }

        [HttpGet("{username}")]
        [Authorize]
        public async Task<IActionResult> GetTweets(string username)
        {
            var response = await _tweetRepository.GetTweets(username);

            if (!response.Success)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost("{username}/add/")]
        [Authorize]
        public async Task<IActionResult> AddTweet(string username, AddTweetDto addTweetDto)
        {
            var response = await _tweetRepository.AddTweet(username, addTweetDto);

            if (!response.Success)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpDelete("{username}/delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTweet(string username, int id)
        {
            var response = await _tweetRepository.DeleteTweet(username, id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPut("{username}/update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTweet(string username, int id, UpdateTweetDto updateTweetDto)
        {
            var response = await _tweetRepository.UpdateTweet(username, id, updateTweetDto);

            if (!response.Success)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost("{username}/reply/{id}")]
        [Authorize]
        public async Task<IActionResult> Reply(string username, int id, AddTweetDto addTweetDto)
        {
            var reactingUserName = User.Identity.Name;
            var response = await _tweetRepository.ReplyTweet(username, reactingUserName, id, addTweetDto);

            if (!response.Success)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }
    }
}
