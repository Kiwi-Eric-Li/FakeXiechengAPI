using System.ComponentModel.DataAnnotations;

namespace FakeXiechengAPI.Dtos
{
    public class TouristRouteForUpdateDto: TouristRouteForManipulationDto
    {
        [Required(ErrorMessage = "更新必备")]
        [MaxLength(1500)]
        public string Description { get; set; }
    }
}
