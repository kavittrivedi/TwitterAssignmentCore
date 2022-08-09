using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TwitterAssignmentApi.Dtos;
using TwitterAssignmentApi.Models;

namespace TwitterAssignmentApi.Data
{
    public class TweetRepository : ITweetRepository
    {
        private readonly DataContext _context;
        private IMapper _mapper;
        public TweetRepository(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<ServiceResponse<GetTweetDto>> AddTweet(string username, AddTweetDto addTweetDto)
        {
            if (addTweetDto.ParentTweetId == 0)
            {
                addTweetDto.ParentTweetId = null;
            }

            var response = new ServiceResponse<GetTweetDto>();
            var tweet = _mapper.Map<Tweet>(addTweetDto);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == username.ToLower() || x.LoginId.ToLower() == username.ToLower());

            if (user == null)
            {
                response.Success = false;
                response.Message = "Something went wrong, Please contact support";
                return response;
            }

            tweet.UserId = user.Id;
            _context.Tweets.Add(tweet);
            await _context.SaveChangesAsync();

            tweet.User = user;

            return new ServiceResponse<GetTweetDto>
            {
                Data = _mapper.Map<GetTweetDto>(tweet)
            };
        }

        public async Task<ServiceResponse<List<GetTweetDto>>> GetTweets(string username)
        {
            var response = new ServiceResponse<List<GetTweetDto>>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == username.ToLower() || x.LoginId.ToLower() == username.ToLower());

            if (user == null)
            {
                response.Success = false;
                response.Message = "Something went wrong, Please contact support";
                return response;
            }

            var tweets = _mapper.Map<List<GetTweetDto>>(await _context.Tweets.Include("User").Where(x => x.UserId == user.Id).ToListAsync());

            response.Data = tweets;

            return response;
        }

        public async Task<ServiceResponse<GetTweetDto>> ReplyTweet(string username, string reactingUserName, int id, AddTweetDto addTweetDto)
        {
            var response = new ServiceResponse<GetTweetDto>();

            var tweet = _mapper.Map<Tweet>(addTweetDto);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == reactingUserName.ToLower() || x.LoginId == reactingUserName.ToLower());

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            if (tweet.ParentTweetId == null || tweet.ParentTweetId == 0 || tweet.ParentTweetId != id)
            {
                response.Success = false;
                response.Message = "Invalid parent tweet";
                return response;
            }

            var parentTweet = await _context.Tweets.Include("User").FirstOrDefaultAsync(x => x.Id == tweet.ParentTweetId && (x.User.Email == username.ToLower() || x.User.LoginId == username.ToLower()));

            if (parentTweet == null)
            {
                response.Success = false;
                response.Message = "Parent tweet not found";
                return response;
            }

            tweet.UserId = user.Id;
            _context.Tweets.Add(tweet);
            await _context.SaveChangesAsync();

            tweet.User = user;
            response.Data = _mapper.Map<GetTweetDto>(tweet);

            return response;
        }

        public async Task<ServiceResponse<List<GetTweetDto>>> DeleteTweet(string username, int id)
        {
            var response = new ServiceResponse<List<GetTweetDto>>();

            var result = await _context.Tweets.Include("User")
                .FirstOrDefaultAsync(x => x.Id == id && (x.User.Email.ToLower() == username.ToLower() || x.User.LoginId.ToLower() == username.ToLower()));

            if (result == null)
            {
                response.Success = false;
                response.Message = "Tweet not found";
                return response;
            }

            _context.Tweets.Remove(result);
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<List<GetTweetDto>>(await _context.Tweets.Where(x => x.UserId == result.UserId).ToListAsync());

            return response;
        }

        public async Task<ServiceResponse<GetTweetDto>> UpdateTweet(string username, int id, UpdateTweetDto updateTweetDto)
        {
            if (updateTweetDto.ParentTweetId == 0)
            {
                updateTweetDto.ParentTweetId = null;
            }

            var response = new ServiceResponse<GetTweetDto>();

            if (id != updateTweetDto.Id)
            {
                response.Success = false;
                response.Message = "Incorrect tweet id";
                return response;
            }

            var tweet = _mapper.Map<Tweet>(updateTweetDto);
            var result = await _context.Tweets.Include("User").AsNoTracking().FirstOrDefaultAsync(x => x.Id == tweet.Id && (x.User.Email.ToLower() == username.ToLower() || x.User.LoginId.ToLower() == username.ToLower()));

            if (result == null)
            {
                response.Success = false;
                response.Message = "Tweet not found";
                return response;
            }

            tweet.UserId = result.UserId;
            tweet.User = result.User;

            _context.Entry(tweet).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            response.Data = _mapper.Map<GetTweetDto>(tweet);

            return response;
        }
    }
}
