using MediatR;
using DTOs.DataTable;
using System.Linq.Expressions;
using Domain.Entities;
using LinqKit;
using System.Linq.Dynamic.Core;
using Application.Repositories;
using Application.Contracts;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Base.Application.Mapping;
using System.Drawing.Imaging;
using Utilities;
using Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Web.Users
{
    #region Request
    public class UserUpdateCommand
    : BaseEntityDTO, IRequest<BaseResult>, IUsernameDTO, IMobileDTO, IMapFrom<User>
    {
        #region Constructors
        public UserUpdateCommand()
        {
            RoleIds = new List<long>();
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

        [Display(Name = "فعال")]
        public bool IsEnabled { get; set; }

        [Display(Name = "نقش ها")]
        public List<long> RoleIds { get; set; }
        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<UserUpdateCommand, User>()
            .ForMember(dest => dest.Roles,
                opt => opt.MapFrom(src => src.RoleIds.Select(id => new UserRole() { RoleId = id })));
        #endregion
    }
    #endregion



    #region Handler
    public class UserUpdateQueryHandler : IRequestHandler<UserUpdateCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UserUpdateQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            #region یافتن اطلاعات کاربر
            var user = await _uow.Users.FirstOrDefaultAsync(
                x => x.Id == request.Id,
                includes: x => x.Include(z => z.Roles));

            if (user == null)
                throw new NotFoundException("کاربر مورد نظر یافت نشد!");
            #endregion

            #region حذف نقش های قبلی کاربر
            _uow.UserRoles.RemoveRange(user.Roles);
            #endregion

            #region ویرایش اطلاعات کاربر و نقش ها
            user.Roles.Clear();
            user = _mapper.Map(request, user);
            _uow.Users.Update(user);
            #endregion

            await _uow.CommitAsync();
            return new BaseResult(true);
        }
    }

    #endregion
}
