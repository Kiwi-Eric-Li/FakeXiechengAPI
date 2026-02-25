using FakeXiechengAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FakeXiechengAPI.Controllers
{
    [ApiController]
    [Route("api/shoppingcart")]
    public class ShoppingCartController : ControllerBase
    {
        // http 上下文关系对象
        private readonly IHttpContextAccessor _httpContextAccessor;
        // 数据仓库成员变量
        private readonly ITouristRouteRepository _touristRouteRepository;

        public ShoppingCartController(IHttpContextAccessor httpContextAccessor, ITouristRouteRepository touristRouteRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetShoppingCart()
        {
            // 获取当前用户
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // 使用 userId 获取购物车
            var shoppingCart = _touristRouteRepository.GetShoppingCartByUserId(userId);

            return null;

        }
    }
}
