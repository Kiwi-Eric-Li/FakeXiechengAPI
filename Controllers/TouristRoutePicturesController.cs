using AutoMapper;
using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Models;
using FakeXiechengAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeXiechengAPI.Controllers
{
    [Route("api/touristRoutes/{touristRouteId}/pictures")]
    [ApiController]
    public class TouristRoutePicturesController: ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private IMapper _mapper;

        public TouristRoutePicturesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPictureListForTouristRoute([FromRoute] Guid touristRouteId)
        {
            // 判断旅游路线是否存在
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线不存在。");
            }
            var pictures = _touristRouteRepository.GetPicturesByTouristRouteId(touristRouteId);
            if(pictures == null || pictures.Count() < 0)
            {
                return NotFound("图片不存在！");
            }
            return Ok(_mapper.Map<IEumerable<TouristRoutePictureDto>>(pictures));
        }

        [HttpGet("{pictureId}")]
        public IActionResult GetPicture(Guid touristRouteId, int pictureId)
        {
            // 判断旅游路线是否存在
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线不存在。");
            }
            var picture = _touristRouteRepository.GetPicture(pictureId);
            if(picture == null)
            {
                return NotFound("图片不存在。");
            }
            return Ok(_mapper.Map<TouristRoutePictureDto>(picture));
        }

        [HttpPost]
        public IActionResult CreateTouristRoutePicture(
            [FromRoute] Guid touristRouteId,
            [FromBody] TouristRoutePictureForCreationDto touristRoutePictureForCreationDto
        )
        {
            // 先判断旅游路线是否存在
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线不存在！");
            }
            // 将 touristRoutePictureForCreationDto 映射给 TouristRoutePicture
            var pictureModel = _mapper.Map<TouristRoutePicture>(touristRoutePictureForCreationDto);
            _touristRouteRepository.AddTouristRoutePicture(touristRouteId, pictureModel);
            var pictureToReturn = _mapper.Map<TouristRoutePictureDto>(pictureModel);
            return CreatedAtRoute(
                "GetPicture",
                new {touristRouteId = pictureModel.TouristRouteId, pictureId = pictureModel.Id},
                pictureToReturn
            );
        }

        [HttpDelete]
        public IActionResult DeletePicture([FromRoute] Guid touristRouteId, [FromRoute] int pictureId)
        {
            // 先判断旅游路线是否存在
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线不存在！");
            }

            var picture = _touristRouteRepository.GetPicture(pictureId);
            _touristRouteRepository.DeleteTouristRoutePicture(picture);
            _touristRouteRepository.Save();
            return NoContent();
        }



    }
}
