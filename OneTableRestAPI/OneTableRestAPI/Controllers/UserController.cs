using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EzCoreKit.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTableRestAPI.Models;

namespace OneTableRestAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        public AnonChatContext Database { get; private set; }

        public new UserInfo User {
            get {
                var userId = base.User.Claims.SingleOrDefault(x =>
                    x.Properties.Any(y => y.Value == JwtRegisteredClaimNames.Sub)
                )?.Value;

                return Database.UserInfo.SingleOrDefault(x => x.Id == userId);
            }
        }

        public UserController(AnonChatContext database) {//自DI取得DBContext
            this.Database = database;
        }

        [Authorize]
        [HttpGet]
        [HttpGet("{userId}")]
        public UserInfo Get([FromRoute]string userId)
        {
            if (string.IsNullOrEmpty(userId)) {
                return User;
            }
            return Database.UserInfo.SingleOrDefault(x => x.Id == userId);
        }
        

        // POST api/values
        [HttpPost("login")]
        public string Login([FromBody]UserInfo loginInfo)
        {

            var hashedPassword = (loginInfo.Password + "_magic_").ToHashString<MD5>();//Hash前要加鹽
            var userinfo = Database.UserInfo.SingleOrDefault(x => x.Id == loginInfo.Id && x.Password == hashedPassword);
            if (userinfo == null) {
                Unauthorized();
                return null;
            }

            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, userinfo.Id)
            };

            return EzCoreKit.AspNetCore.EzJwtBearerHelper.GenerateToken(DateTime.MaxValue, claims);// 發個永久token
        }

        // PUT api/values/5
        [HttpPost("register")]
        public async void Register([FromBody]UserInfo loginInfo)
        {
            if(string.IsNullOrEmpty(loginInfo.Id) || 
               string.IsNullOrEmpty(loginInfo.Gender)||
               string.IsNullOrEmpty(loginInfo.Name)
                ) {
                BadRequest();
                return;
            }

            if(Database.UserInfo.Any(x=>x.Id == loginInfo.Id)) {//重複註冊
                BadRequest();
                return;
            }

            loginInfo.Password = (loginInfo.Password + "_magic_").ToHashString<MD5>();

            Database.UserInfo.Add(loginInfo);

            await Database.SaveChangesAsync();
        }

        [Authorize]
        public async void Update([FromBody] UserInfo userinfo) {
            if (userinfo == null) {
                BadRequest();
                return;
            }

            if (!string.IsNullOrEmpty(userinfo.Name)) {
                User.Name = userinfo.Name;
            }
            if (!string.IsNullOrEmpty(userinfo.Password)) {
                User.Password = (userinfo.Password + "_magic_").ToHashString<MD5>();
            }
            if (!string.IsNullOrEmpty(userinfo.AboutMe)) {
                User.AboutMe = userinfo.AboutMe;
            }

            await Database.SaveChangesAsync();
        }
    }
}
