using AutoMapper;
using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Models;
using FakeXiechengAPI.Services;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetTouristRoutes([FromQuery] string keyword, string ratingOperator, int ratingValue)   // FromQuery vs. FromBody
        {
            Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
            string operatorType = "";
            ratingValue = -1;
            Match match = regex.Match(ratingOperator);
            if (match.Success)
            {
                operatorType = match.Groups[1].Value;
                ratingValue = Int32.Parse(match.Groups[2].Value);
            }


            var routes = await _touristRouteRepository.GetTouristRoutesAsync(keyword, operatorType, ratingValue);
            if(routes == null || routes.Count() <= 0)
            {
                return NotFound("没有旅游路线。");
            }
            var routesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(routes);
            return Ok(routesDto);
        }

        // /api/touristroutes/{touristRouteId}
        [HttpGet("{touristRouteId}")]
        public async Task<IActionResult> GetTouristRouteById(Guid touristRouteId)
        {
            var route = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
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
        [Authorize] // 相当于给这个API上了一把锁，只有登录的用户，才能访问
        public IActionResult CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute("GetTouristRouteById", new { touristRouteId = touristRouteToReturn.Id}, touristRouteToReturn);
        }

        [HttpPut("{touristRouteId}")]
        [Authorize]
        public async Task<IActionResult> UpdateTouristRoute([FromRoute] Guid touristRouteId, [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)
        {
            // 判断旅游路线是否存在
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("旅游路线找不到!");
            }
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            // 1. 映射DTO
            // 2. 更新DTO
            // 3. 映射model
            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();
            return NoContent(); // 返回204
        }

        [HttpDelete("{touristRouteId}")]
        [Authorize]
        public async Task<IActionResult> DeleteTouristRoute([FromRoute] Guid touristRouteId)
        {
            // 判断旅游路线是否存在
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在！");
            }
            var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            _touristRouteRepository.DeleteTouristRoute(touristRoute);
            _touristRouteRepository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("batch")]
        [Authorize]
        public IActionResult DeleteByIds([FromBody] List<Guid> ids)
        {
            return Ok();
        }

    }
}
