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
    public class RoleToggleEnableCommand
        : IRequest<BaseResult>, IBaseEntityDTO
    {
        #region Constructors
        public RoleToggleEnableCommand(long id)
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
    public class RoleToggleEnableQueryHandler : IRequestHandler<RoleToggleEnableCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        public RoleToggleEnableQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
        }


        public async Task<BaseResult> Handle(RoleToggleEnableCommand request, CancellationToken cancellationToken)
        {
            var res = await _uow.Roles.ExecuteUpdateAsync(x => x.Id == request.Id, x => x.SetProperty(z => z.IsEnabled, z => !z.IsEnabled));
            if (res == 0)
                throw new NotFoundException("رکورد مورد نظر یافت نشد!");
            return new BaseResult(true);
        }
    }

    #endregion
}
