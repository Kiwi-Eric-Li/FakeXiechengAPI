namespace FakeXiechengAPI.Models
{
    public class TouristRoutePicture
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public Guid TouristRouteId { get; set; }    // 建立外键联系
        public TouristRoute TouristRoute { get; set; }
    }
}
