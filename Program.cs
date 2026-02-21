using FakeXiechengAPI.Database;
using FakeXiechengAPI.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FakeXiechengAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            /**
             * builder.Services 用于注册应用程序所需要的各种服务，也就是所谓的IOC反转控制容器
             * 例如，添加数据库的上下文、身份认证服务或MVC服务等。
             * 通过builder.Services来配置服务，可以确保在app.Run()之前，就把这些服务准备好了。
             *
             */

            // 服务器配置验证客户端传来的JWT是否合法
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    var secretByte = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Authentication:Audience"],

                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                    };
            });



            // 注册控制器的服务
            builder.Services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            }).AddXmlDataContractSerializerFormatters();

            // 把一个接口和它的实现类注册到容器中
            builder.Services.AddScoped<ITouristRouteRepository, TouristRouteRepository>();
            // 把 AppDbContext 注册到依赖注入容器
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration["ConnectionStrings:DefaultConnection"])

            );

            // 扫描 profile 文件
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // 你是谁？
            app.UseAuthentication();
            // 你可以干什么？你有什么权限？
            app.UseAuthorization();

            // 请求处理管道，它将控制器的路由映射到处理请求的管道中
            app.MapControllers();

            app.Run();
        }
    }
}
