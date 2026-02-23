using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace FakeXiechengAPI.Models
{
    public class ShoppingCart
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }  // 用户Id
        public User User { get; set; }
        public ICollection<LineItem> ShoppingCartItems { get; set; }  // 购物列表
    }
}
