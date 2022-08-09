using AutoMapper;
using TwitterAssignmentApi.Dtos;
using TwitterAssignmentApi.Models;

namespace TwitterAssignmentApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GetUserDto, User>()
                .ReverseMap();

            CreateMap<AddTweetDto, Tweet>()
                .ReverseMap();

            CreateMap<UpdateTweetDto, Tweet>()
                .ReverseMap();

            CreateMap<GetTweetDto, Tweet>()
                .ReverseMap();
        }
    }
}
