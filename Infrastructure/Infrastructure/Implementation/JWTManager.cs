using Application.Contracts;
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Implementation
{
    public class JwtManager : IJwtManager
    {
        private IConfiguration configuration;
        private IHttpContextAccessor _accessor;
        public JwtManager(IConfiguration iConfig, IHttpContextAccessor accessor)
        {
            configuration = iConfig;
            _accessor = accessor;
        }



        #region گرفتن توکن از هدر ریکوئست
        /// <summary>
        /// گرفتن توکن از هدر ریکوئست
        /// </summary>
        /// <returns></returns>
        public string GetHeaderToken()
        {
            if (_accessor?.HttpContext?.Request == null)
                return null;
            if (!_accessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var headerAuth))
                return null;

            if (string.IsNullOrEmpty(headerAuth)) return null;

            var TokenArray = headerAuth.ToString().Split(' ');
            string token = null;
            if (TokenArray.Length > 1)
                token = TokenArray[1];
            else
                token = TokenArray[0];

            return token;
        } 
        #endregion




        #region گرفتن آیدی کاربر و عضو از توکن
        // <summary>
        /// آیدی کاربر
        /// </summary>
        /// <param name="jwt">توکن مورد نظر. در صورت نال بودن از هدر میگیرد.</param>
        /// <returns></returns>
        public long? GetUserId(string jwt = null)
        {
            if (string.IsNullOrEmpty(jwt))
                jwt = GetHeaderToken();
            if (string.IsNullOrEmpty(jwt))
                return null;

            var TokenArray = jwt.ToString().Split(' ');
            string token = null;
            if (TokenArray.Length > 1)
                token = TokenArray[1];
            else
                token = TokenArray[0];

            var _id = GetClaim(token, "userId");
            if (!long.TryParse(_id, out var id)) return null;
            return id;
        }
        #endregion



        #region گرفتن نام کاربر از توکن
        // <summary>
        /// گرفتن نام کاربر
        /// </summary>
        /// <param name="jwt">توکن مورد نظر. در صورت نال بودن از هدر میگیرد.</param>
        /// <returns></returns>
        public string GetUserName(string jwt = null)
        {
            if (string.IsNullOrEmpty(jwt))
                jwt = GetHeaderToken();
            if (string.IsNullOrEmpty(jwt))
                return null;

            var TokenArray = jwt.ToString().Split(' ');
            string token = null;
            if (TokenArray.Length > 1)
                token = TokenArray[1];
            else
                token = TokenArray[0];

            var _name = GetClaim(token, "name");
            return _name;
        }
        #endregion


        #region توابع توکن دسترسی
        #region ایجاد توکن دسترسی با استفاده از اطلاعات کاربر
        /// <summary>
        /// ایجاد توکن دسترسی با استفاده از اطلاعات کاربر
        /// </summary>
        /// <param name="userId"> آیدی کاربر</param>
        /// <param name="tokenKey"> کلید یکتای توکن که در توکن دسترسی و توکن بازنشانی یکسان است.</param>
        /// <returns></returns>
        public TokenResponseDTO GenerateAccessToken(long? userId, string name, string tokenKey, double? expMin = null)
        {
            if (expMin == null)
                expMin = configuration.GetValue<double?>("JWT:AccessExpMin");
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("JWT:AccessSecretKey").Value);
            var issuer = _accessor.HttpContext.Request.Host.Value;

            var claims = new List<Claim>();
            claims.Add(new Claim("tk", tokenKey));
            claims.Add(new Claim("name", name));
            if (userId != null)
                claims.Add(new Claim("userId", userId.ToString()));

            var _expDate = DateTime.Now.AddMinutes(expMin ?? 30);
            var JWToken = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                notBefore: DateTime.Now,
                expires: _expDate,
                //Using HS256 Algorithm to encrypt Token
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            return new TokenResponseDTO(token, _expDate.ToString("yyyy/MM/dd HH:mm:ss"));
        }
        #endregion



        #region ولیدیت کردن توکن دسترسی
        /// <summary>
        /// ولیدیت کردن توکن دسترسی
        /// </summary>
        /// <param name="token">توکن</param>
        /// <returns></returns>
        public bool ValidateAccessToken(string token)
        {
            var mySecret = Encoding.ASCII.GetBytes(configuration.GetSection("JWT:AccessSecretKey").Value); ;
            var mySecurityKey = new SymmetricSecurityKey(mySecret);

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        #endregion
        #endregion





        #region توابع رفرش توکن
        #region ایجاد رفرش توکن
        /// <summary>
        /// ایجاد توکن دسترسی با استفاده از اطلاعات کاربر
        /// </summary>
        /// <param name="userId"> آیدی کاربر</param>
        /// <param name="tokenKey"> کلید یکتای توکن که در توکن دسترسی و توکن بازنشانی یکسان است.</param>
        /// <returns></returns>
        public TokenResponseDTO GenerateRefreshToken(long? userId, string tokenKey, double? expMin = null)
        {
            if (expMin == null)
                expMin = configuration.GetValue<double?>("JWT:RefreshExpMin");
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("JWT:RefreshSecretKey").Value);

            var claims = new List<Claim>();
            claims.Add(new Claim("tk", tokenKey));
            if (userId != null)
                claims.Add(new Claim("userId", userId.ToString()));

            var _expDate = DateTime.Now.AddMinutes(expMin ?? 120);
            var JWToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.Now,
                expires: _expDate,
                //Using HS256 Algorithm to encrypt Token
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.Aes128CbcHmacSha256)
            );
            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            return new TokenResponseDTO(token, _expDate.ToString("yyyy/MM/dd HH:mm:ss"));
        }
        #endregion


        #region ولیدیت کردن توکن دسترسی
        /// <summary>
        /// ولیدیت کردن توکن دسترسی
        /// </summary>
        /// <param name="token">توکن</param>
        /// <returns></returns>
        public bool ValidateRefreshToken(string token)
        {
            var mySecret = Encoding.ASCII.GetBytes(configuration.GetSection("JWT:RefreshSecretKey").Value); ;
            var mySecurityKey = new SymmetricSecurityKey(mySecret);

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        #endregion
        #endregion



        #region خواندن کلایم از توکن
        /// <summary>
        ///  خواندن کلایم از توکن
        /// </summary>
        /// <param name="token">توکن</param>
        /// <param name="claimType">پارامتر مورد نیاز</param>
        /// <returns></returns>
        public string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var stringClaimValue = securityToken.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
            return stringClaimValue;
        }
        #endregion



    }
}
