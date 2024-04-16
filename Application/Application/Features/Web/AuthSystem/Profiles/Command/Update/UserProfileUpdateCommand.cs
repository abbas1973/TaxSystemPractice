using Application.DTOs;
using Application.Exceptions;
using Application.Repositories;
using Application.SessionServices;
using AutoMapper;
using Base.Application.Mapping;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;

namespace Application.Features.Web.Profiles
{
    #region Request
    public class UserProfileUpdateCommand
    : IRequest<BaseResult>, IUsernameDTO, IMobileDTO, IMapFrom<User>
    {
        #region Constructors
        public UserProfileUpdateCommand()
        {

        }
        #endregion


        #region Properties
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "موبایل")]
        public string Mobile { get; set; }

        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Display(Name = "شناسه شهر")]
        public long CityId { get; set; }

        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<UserProfileUpdateCommand, User>();
        #endregion
    }
    #endregion



    #region Handler
    public class UserProfileUpdateQueryHandler : IRequestHandler<UserProfileUpdateCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly ISession _session;
        private readonly IMapper _mapper;
        public UserProfileUpdateQueryHandler(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor accessor)
        {
            _uow = uow;
            _mapper = mapper;
            _session = accessor.HttpContext.Session;
        }


        public async Task<BaseResult> Handle(UserProfileUpdateCommand request, CancellationToken cancellationToken)
        {
            var sessionUser = _session.GetUser();

            #region یافتن اطلاعات کاربر
            var user = await _uow.Users.GetByIdAsync(sessionUser.Id);

            if (user == null)
                throw new NotFoundException("کاربر مورد نظر یافت نشد!");
            #endregion


            #region ویرایش اطلاعات کاربر
            user = _mapper.Map(request, user);
            _uow.Users.Update(user);
            #endregion

            await _uow.CommitAsync();
            return new BaseResult(true);
        }
    }

    #endregion
}
