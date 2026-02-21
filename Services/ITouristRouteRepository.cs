using FakeXiechengAPI.Models;
using System.Threading.Tasks;

namespace FakeXiechengAPI.Services
{
    public interface ITouristRouteRepository
    {
        Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int ratingValue);   // 获取所有旅游路线
        Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId);  // 根据旅游路线id，获取单个旅游路线
        Task<bool> TouristRouteExistsAsync(Guid touristRouteId);
        Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId);
        Task<TouristRoutePicture> GetPictureAsync(int pictureId);

        Task<bool> SaveAsync();
        void AddTouristRoute(TouristRoute touristRoute);
        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);
        void DeleteTouristRoute(TouristRoute touristRoute);
        void DeleteTouristRoutePicture(TouristRoutePicture picture);
    }
}
