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

namespace Application.Features.Web.Roles
{
    #region Request
    public class RoleCreateCommand
    : IRequest<BaseResult<long>>, IMapFrom<Role>
    {
        #region Constructors
        public RoleCreateCommand()
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
            profile.CreateMap<RoleCreateCommand, Role>()
            .ForMember(dest => dest.Claims,
                opt => opt.MapFrom(src => src.Claims.Select(claim => new RoleClaim() { Claim = claim })));
        #endregion
    }
    #endregion



    #region Handler
    public class RoleCreateQueryHandler : IRequestHandler<RoleCreateCommand, BaseResult<long>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public RoleCreateQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult<long>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            var Role = _mapper.Map<Role>(request);
            await _uow.Roles.AddAsync(Role);
            await _uow.CommitAsync();
            return new BaseResult<long>(Role.Id);
        }
    }

    #endregion
}
