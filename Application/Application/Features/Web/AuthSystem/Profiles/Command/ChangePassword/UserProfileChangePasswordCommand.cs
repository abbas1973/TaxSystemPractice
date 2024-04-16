using Application.DTOs;
using Application.Repositories;
using Application.SessionServices;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Utilities;

namespace Application.Features.Web.Profiles
{
    #region Request
    public class UserProfileChangePasswordCommand
    : IRequest<BaseResult>, IPasswordDTO
    {
        #region Constructors
        public UserProfileChangePasswordCommand()
        {
        }
        #endregion


        #region Properties       
        [Display(Name = "کلمه عبور فعلی")]
        public string CurrentPassword { get; set; }

        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        public string RePassword { get; set; }

        [Display(Name = "کپچا")]
        public string Captcha { get; set; }
        #endregion

    }
    #endregion



    #region Handler
    public class UserProfileChangePasswordQueryHandler : IRequestHandler<UserProfileChangePasswordCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly ISession _session;
        public UserProfileChangePasswordQueryHandler(IUnitOfWork uow, IHttpContextAccessor accessor)
        {
            _uow = uow;
            _session = accessor.HttpContext.Session;
        }

        
        public async Task<BaseResult> Handle(UserProfileChangePasswordCommand request, CancellationToken cancellationToken)
        {
            #region بررسی کپچا در صورت وجود
            var captcha = _session.GetCaptcha();
            if (string.IsNullOrEmpty(captcha) || captcha != request.Captcha)
            {
                _session.RemoveCaptcha();
                return new BaseResult<bool>(false, "کد امنیتی صحیح نیست!");
            }
            _session.RemoveCaptcha();
            #endregion

            var sessionUser = _session.GetUser();
            var user = await _uow.Users.GetByIdAsync(sessionUser.Id);
            user.Password = request.Password.GetHash();
            await _uow.CommitAsync();
            return new BaseResult(true);
        }
    }

    #endregion
}
