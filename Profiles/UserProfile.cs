using AutoMapper;
using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Models;

namespace FakeXiechengAPI.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            // 将 RegisterDto 映射为 User
            CreateMap<RegisterDto, User>();
        }
    }
}
