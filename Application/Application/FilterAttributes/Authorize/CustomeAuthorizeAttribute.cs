using Application.Filters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Filters
{
    public class CustomeAuthorizeAttribute: ActionFilterAttribute
    {
        #region Constructors and Fields
        /// <summary>
        /// نوع احراز هویت درخواستی
        /// </summary>
        public AuthorizeType AuthorizeType { get; set; }


        /// <summary>
        /// اگر نوع احراز هویت نیاز به دسترسی اختصاصی بود،
        /// چه دسترسی هایی لازم است تا کاربر احراز هویت شود.
        /// <para>
        /// حداقل یکی از دسترسی ها کافیست
        /// </para>
        /// </summary>
        protected List<string> AllowedClaims { get; set; }


        /// <summary>
        /// بررسی دسترسی کاربر
        /// </summary>
        public CustomeAuthorizeAttribute(AuthorizeType authorizeType = AuthorizeType.NeedPermission, params string[] allowedClaims)
        {
            AuthorizeType = authorizeType;
            AllowedClaims = allowedClaims.ToList() ?? new List<string>();
        }
        #endregion

    }
}
