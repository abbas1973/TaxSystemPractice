using System.Security.Claims;
using System.Text.Json;
using Utilities;

namespace Application.DTOs
{
    /// <summary>
    /// مدل اطلاعات کاربر برای افزودن به توکن
    /// </summary>
    public class UserTokenDTO
    {
        #region Constructors
        public UserTokenDTO() { }

        public UserTokenDTO(long id, string username, string name, bool mic)
        {
            Id = id;
            Username = username;
            Name = name;
            Mic = mic;
        }
        #endregion


        #region Properties
        public long Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// آیا موبایل کاربر تایید شده است؟
        /// <para>
        /// MobileIsConfirmed
        /// </para>
        /// </summary>
        public bool Mic { get; set; }
        #endregion


        #region Functions
        #region گرفتن کلایم ها برای افزودن به توکن
        public List<Claim> GetClaims()
        {
            var claims = new List<Claim>();
            foreach (var prop in this.GetType().GetProperties())
                claims.Add(new Claim(prop.Name.ToCamelCase(), prop.GetValue(this, null).ToString()));
            return claims;
        } 
        #endregion

        #endregion
    }
}
