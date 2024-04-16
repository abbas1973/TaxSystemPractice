using MediatR;
using Application.DTOs.Claims;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.ComponentModel;
using Application.Filters;

namespace Application.Features.Web.Claims
{
    #region Request
    public class ClaimGetAllQuery
    : IRequest<ClaimDTO>
    {
        #region Constructors
        public ClaimGetAllQuery(Assembly assembly)
        {
            Assembly = assembly;
        }
        #endregion

        #region Properties
        public Assembly Assembly { get; set; }
        #endregion
    }
    #endregion



    #region Handler
    public class ClaimGetAllQueryHandler : IRequestHandler<ClaimGetAllQuery, ClaimDTO>
    {
        private readonly IActionDescriptorCollectionProvider _actionProvider;

        public ClaimGetAllQueryHandler(IActionDescriptorCollectionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }


        public async Task<ClaimDTO> Handle(ClaimGetAllQuery request, CancellationToken cancellationToken)
        {
            var claims = new List<ClaimDTO>();
            #region گرفتن لیست کنترلر ها
            var controllers = request.Assembly.GetTypes()
                        .Where(type => typeof(Controller).IsAssignableFrom(type) && type.Name != "MyBaseController");
            #endregion

            foreach (var item in controllers)
            {
                var _controller = item.Name.Replace(nameof(Controller), "");
                var _area = item.GetCustomAttribute<AreaAttribute>()?.RouteValue;
                var _name = item.GetCustomAttribute<DescriptionAttribute>()?.Description ?? _controller;
                var controllerAuthType = item.GetCustomAttributes<UserAuthorize>()?.FirstOrDefault()?.AuthorizeType;

                #region اگر کنترلر پرمیشن اختصاصی داشت همه اکشن های با پرمیشن اختصاصی یا بدون اتریبیوت گرفته شوند
                if (controllerAuthType == AuthorizeType.NeedPermission)
                {
                    var controllerClaim = new ClaimDTO()
                    {
                        Name = _name,
                        Level = 2,
                        IsController = true,
                        Area = _area,
                        Controller = _controller,
                        SubClaims = item.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                            .Where(x => x.GetCustomAttributes<UserAuthorize>()?.FirstOrDefault()?.AuthorizeType == AuthorizeType.NeedPermission
                                || x.GetCustomAttributes<UserAuthorize>()?.FirstOrDefault() == null)
                            .Select(x => new ClaimDTO()
                            {
                                Name = x.GetCustomAttribute<DescriptionAttribute>()?.Description ?? x.Name,
                                IsController = false,
                                Level = 3,
                                Controller = _controller,
                                Action = x.Name,
                                Area = _area
                            })
                            .GroupBy(x => x.Action)
                            .Select(g => g.First()).ToList()
                    };
                    claims.Add(controllerClaim);
                }
                #endregion

                #region اگر کنترلر پرمیشن اختصاصی نداشت، همه اکشن ها با پرمیشن اختصاصی لیست شوند
                else
                {
                    var actionClaims = item.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                            .Where(x => x.GetCustomAttributes<UserAuthorize>()?.FirstOrDefault()?.AuthorizeType == AuthorizeType.NeedPermission)
                            .Select(x => new ClaimDTO()
                            {
                                Name = x.GetCustomAttribute<DescriptionAttribute>()?.Description ?? x.Name,
                                IsController = false,
                                Level = 2,
                                Controller = _controller,
                                Action = x.Name,
                                Area = _area
                            }).ToList();
                    claims.AddRange(actionClaims);
                } 
                #endregion
            }


            #region ایجاد کلایم روت و افزودن بقیه کلایم ها به زیر مجموعه
            var root = new ClaimDTO()
            {
                Controller = "Full",
                IsController = false,
                Level = 1,
                Name = "دسترسی کل سامانه",
                SubClaims = claims
            };

            return root; 
            #endregion
        }
    }

    #endregion
}
