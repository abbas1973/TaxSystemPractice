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
    public class UserToggleEnableCommand
        : IRequest<BaseResult>, IBaseEntityDTO
    {
        #region Constructors
        public UserToggleEnableCommand(long id)
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
    public class UserToggleEnableQueryHandler : IRequestHandler<UserToggleEnableCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        public UserToggleEnableQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
        }


        public async Task<BaseResult> Handle(UserToggleEnableCommand request, CancellationToken cancellationToken)
        {
            var res = await _uow.Users.ExecuteUpdateAsync(x => x.Id == request.Id, x => x.SetProperty(z => z.IsEnabled, z => !z.IsEnabled));
            if (res == 0)
                throw new NotFoundException("رکورد مورد نظر یافت نشد!");
            return new BaseResult(true);
        }
    }

    #endregion
}
