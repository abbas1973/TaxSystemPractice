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

namespace Application.Features.Web.Users
{
    #region Request
    public class UserCreateCommand
    : IRequest<BaseResult<long>>, IUsernameDTO, IPasswordDTO, IMobileDTO, IMapFrom<User>
    {
        #region Constructors
        public UserCreateCommand()
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

        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }

        [Display(Name = "شهر")]
        public long CityId { get; set; }

        [Display(Name = "نقش ها")]
        public List<long> RoleIds { get; set; }
        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<UserCreateCommand, User>()
            .ForMember(dest => dest.Password,
                opt => opt.MapFrom(src => src.Password.GetHash(null)))
            .ForMember(dest => dest.MobileConfirmed,
                opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Roles,
                opt => opt.MapFrom(src => src.RoleIds.Select(id => new UserRole() { RoleId = id })));
        //.ConvertUsing(typeof(UserCreateCommand_To_User));
        #endregion
    }
    #endregion



    #region Handler
    public class UserCreateQueryHandler : IRequestHandler<UserCreateCommand, BaseResult<long>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UserCreateQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult<long>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);
            await _uow.Users.AddAsync(user);
            await _uow.CommitAsync();
            return new BaseResult<long>(user.Id);
        }
    }

    #endregion
}
