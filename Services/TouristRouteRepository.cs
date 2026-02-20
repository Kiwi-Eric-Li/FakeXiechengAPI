using FakeXiechengAPI.Database;
using FakeXiechengAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeXiechengAPI.Services
{
    // 接口实现类
    public class TouristRouteRepository: ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            return _context.TouristRoutes.FirstOrDefault(n => n.Id == touristRouteId);
        }

        public IEnumerable<TouristRoute> GetTouristRoutes(string keyword, string ratingOperator, int ratingValue)
        {
            // Include() vs. join()  连接两张表
            IQueryable<TouristRoute> result = _context.TouristRoutes.Include(t => t.TouristRoutePictures);
            if(keyword != null || keyword != '')
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }
            if(ratingValue >= 0)
            {
                switch (ratingOperator)
                {
                    case "largerThan":
                        result = result.Where(t => t.Rating >= ratingValue);
                        break;
                    case "lessThan":
                        result = result.Where(t => t.Rating <= ratingValue);
                        break;
                    case "equalTo":
                    default:
                        result = result.Where(t => t.Rating == ratingValue);
                        break;
                }
            }
            return result.ToList();
        }

        public bool TouristRouteExists(Guid touristRouteId)
        {
            return _context.TouristRoutes.Any(t => t.Id == touristRouteId);
        }

        public IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouteId)
        {
            return _context.TouristRoutePictures.Where(p => p.TouristRouteId == touristRouteId).ToList();
        }

        public TouristRoutePicture GetPicture(int pictureId)
        {
            return _context.TouristRoutePictures.Where(t => t.Id == pictureId).FirstOrDefault();
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            // 验证数据是否合法
            if(touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
            // 写入数据库
            _context.SaveChanges();
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            // 验证参数
            if(touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if(touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }
            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.TouristRoutePictures.Add(touristRoutePicture);
            _context.SaveChanges();
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture picture)
        {
            _context.TouristRoutePictures.Remove(picture);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
