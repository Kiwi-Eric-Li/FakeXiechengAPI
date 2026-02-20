using AutoMapper;
using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Models;
using FakeXiechengAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace FakeXiechengAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        // IActionResult 处理API GET 请求的动作
        // /api/touristroutes?keyword=xxx
        [HttpGet]
        public IActionResult GetTouristRoutes([FromQuery] string keyword, string ratingOperator, int ratingValue)   // FromQuery vs. FromBody
        {
            Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
            string operatorType = "";
            int ratingValue = -1;
            Match match = regex.Match(rating);
            if (match.Success)
            {
                operatorType = match.Groups[1].Value;
                ratingValue = Int32.Parse(match.Groups[2].Value);
            }


            var routes = _touristRouteRepository.GetTouristRoutes(keyword, operatorType, ratingValue);
            if(routes == null || routes.Count() <= 0)
            {
                return NotFound("没有旅游路线。");
            }
            var routesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(routes);
            return Ok(routesDto);
        }

        // /api/touristroutes/{touristRouteId}
        [HttpGet("{touristRouteId}")]
        public IActionResult GetTouristRouteById(Guid touristRouteId)
        {
            var route = _touristRouteRepository.GetTouristRoute(touristRouteId);
            Console.WriteLine("<<<<<<<<<<<<<<<<<<");
            Console.WriteLine(route);
            Console.WriteLine("<<<<<<<<<<<<<<<<<<");
            if (route == null)
            {
                return NotFound($"旅游路线{touristRouteId}，没找到。");
            }
            var routeDto = _mapper.Map<TouristRouteDto>(route);
            return Ok(routeDto);
        }

        [HttpPost]
        public IActionResult CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute("GetTouristRouteById", new { touristRouteId = touristRouteToReturn.Id}, touristRouteToReturn);
        }

        [HttpPut("{touristRouteId}")]
        public IActionResult UpdateTouristRoute([FromRoute] Guid touristRouteId, [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)
        {
            // 判断旅游路线是否存在
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线找不到!");
            }
            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            // 1. 映射DTO
            // 2. 更新DTO
            // 3. 映射model
            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);
            _touristRouteRepository.Save();
            return NoContent(); // 返回204
        }

        [HttpDelete("{touristRouteId}")]
        public IActionResult DeleteTouristRoute([FromRoute] Guid touristRouteId)
        {
            // 判断旅游路线是否存在
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线不存在！");
            }
            var touristRoute = _touristRouteRepository.GetTouristRoute(touristRouteId);
            _touristRouteRepository.DeleteTouristRoute(touristRoute);
            _touristRouteRepository.Save();
            return NoContent();
        }

        [HttpDelete("batch")]
        public IActionResult DeleteByIds([FromBody] List<Guid> ids)
        {

        }

    }
}
