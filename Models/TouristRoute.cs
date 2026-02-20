using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;     // 引入数据注解，用来给模型属性添加验证规则和元数据

namespace FakeXiechengAPI.Models
{
    public class TouristRoute
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal OriginalPrice { get; set; }
        [Range(0.0, 1.0)]
        public double? DiscountPresent { get; set; }    // 可空字段
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public string Features { get; set; }
        public string Fees { get; set; }
        public string Notes { get; set; }
        public ICollection<TouristRoutePicture> TouristRoutePictures { get; set; } = new List<TouristRoutePicture>();  // 一个旅游路线，会有多个旅游路线图片
        public double? Rating { get; set; }
        public TravelDays? TravelDays { get; set; } //可空 旅游天数
        public TripType? TripType { get; set; } // 旅游类型
        public DepartureCity? DepartureCity { get; set; }    // 出发城市

    }
}

/**
 * 1. 添加了新的属性：Rating, TravelDays, ...
 * 2. 修改种子数据， Database/touristRoutesMockData.json
 * 3. 改变数据库表的数据模型：
 *    cd <项目目录>
 *    dotnet ef migrations add UpdateTouristRouteSchema
 * 4. 更新数据库：
 *    dotnet ef database update
 * 
 * 
 * 
 * 在docker中，下载sql server镜像
 * docker run -e 'ACCEPT_EULA-Y' -e 'SA_PASSWORD=Password123!' -p 1433:1433 -d liaisonintl/mssql-server-linux
 */

