using FakeXiechengAPI.Models;

namespace FakeXiechengAPI.Services
{
    public interface ITouristRouteRepository
    {
        IEnumerable<TouristRoute> GetTouristRoutes(string keyword, string ratingOperator, int ratingValue);   // 获取所有旅游路线
        TouristRoute GetTouristRoute(Guid touristRouteId);  // 根据旅游路线id，获取单个旅游路线
        bool TouristRouteExists(Guid touristRouteId);
        IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouteId);
        TouristRoutePicture GetPicture(int pictureId);

    }
}
