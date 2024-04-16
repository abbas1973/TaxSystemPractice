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

namespace Application.Features.Web.Roles
{
    #region Request
    public class RoleUpdateCommand
    : BaseEntityDTO, IRequest<BaseResult>, IMapFrom<Role>
    {
        #region Constructors
        public RoleUpdateCommand()
        {
            Claims = new List<string>();
        }
        #endregion


        #region Properties
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "دسترسی ها")]
        public List<string> Claims { get; set; }
        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<RoleUpdateCommand, Role>()
            .ForMember(dest => dest.Claims,
                opt => opt.MapFrom(src => src.Claims.Select(claim => new RoleClaim() { Claim = claim })));
        #endregion
    }
    #endregion



    #region Handler
    public class RoleUpdateQueryHandler : IRequestHandler<RoleUpdateCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public RoleUpdateQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
        {
            #region یافتن اطلاعات کاربر
            var Role = await _uow.Roles.FirstOrDefaultAsync(
                x => x.Id == request.Id,
                includes: x => x.Include(z => z.Claims));

            if (Role == null)
                throw new NotFoundException("کاربر مورد نظر یافت نشد!");
            #endregion

            #region حذف نقش های قبلی کاربر
            _uow.RoleClaims.RemoveRange(Role.Claims);
            #endregion

            #region ویرایش اطلاعات کاربر و نقش ها
            Role.Claims.Clear();
            Role = _mapper.Map(request, Role);
            _uow.Roles.Update(Role);
            #endregion

            await _uow.CommitAsync();
            return new BaseResult(true);
        }
    }

    #endregion
}
