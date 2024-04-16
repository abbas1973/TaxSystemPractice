using MediatR;
using Domain.Entities;
using Application.Repositories;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Application.Exceptions;

namespace Application.Features.Web.Users
{
    #region Request
    public class UserDeleteCommand
    : IRequest<BaseResult>, IBaseEntityDTO
    {
        #region Constructors
        public UserDeleteCommand(long id)
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
    public class UserDeleteQueryHandler : IRequestHandler<UserDeleteCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        public UserDeleteQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
        }


        public async Task<BaseResult> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            var deletedRoles = await _uow.UserRoles.ExecuteDeleteAsync(x => x.UserId == request.Id);
            var deletedUsers = await _uow.Users.ExecuteDeleteAsync(x => x.Id == request.Id);
            if (deletedUsers == 0)
                throw new NotFoundException("کاربر مورد نظر یافت نشد!");
            return new BaseResult(true);
        }
    }

    #endregion
}
