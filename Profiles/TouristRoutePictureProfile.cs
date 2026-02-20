using AutoMapper;
using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Models;

namespace FakeXiechengAPI.Profiles
{
    public class TouristRoutePictureProfile: Profile
    {
        public TouristRoutePictureProfile()
        {
            // 将 TouristRoutePicture 映射给 TouristRoutePictureDto
            CreateMap<TouristRoutePicture, TouristRoutePictureDto>();
            // 将 TouristRoutePictureDto 映射给 TouristRoutePicture
            CreateMap<TouristRoutePictureDto, TouristRoutePicture>();
        }
    }
}
