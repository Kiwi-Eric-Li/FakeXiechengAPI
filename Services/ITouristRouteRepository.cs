using FakeXiechengAPI.Models;

namespace FakeXiechengAPI.Services
{
    public interface ITouristRouteRepository
    {
        IEnumerable<TouristRoute> GetTouristRoutes();   // 获取所有旅游路线
        TouristRoute GetTouristRoute(Guid touristRouteId);  // 根据旅游路线id，获取单个旅游路线
    }
}
