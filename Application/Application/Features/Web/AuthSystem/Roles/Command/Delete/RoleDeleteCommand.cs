using MediatR;
using Domain.Entities;
using Application.Repositories;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Application.Exceptions;

namespace Application.Features.Web.Roles
{
    #region Request
    public class RoleDeleteCommand
    : IRequest<BaseResult>, IBaseEntityDTO
    {
        #region Constructors
        public RoleDeleteCommand(long id)
        {
            Id = id;
        }
        #endregion


        #region Properties
        [Display(Name = "شناسه")]
        public long Id { get; set; }
        #endregion
    }
    #endregion



    #region Handler
    public class RoleDeleteQueryHandler : IRequestHandler<RoleDeleteCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        public RoleDeleteQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
        }


        public async Task<BaseResult> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
        {
            var deletedUserRoles = await _uow.UserRoles.ExecuteDeleteAsync(x => x.RoleId == request.Id);
            var deletedClaims = await _uow.RoleClaims.ExecuteDeleteAsync(x => x.RoleId == request.Id);
            var deletedRoles = await _uow.Roles.ExecuteDeleteAsync(x => x.Id == request.Id);
            if (deletedRoles == 0)
                throw new NotFoundException("کاربر مورد نظر یافت نشد!");
            return new BaseResult(true);
        }
    }

    #endregion
}
