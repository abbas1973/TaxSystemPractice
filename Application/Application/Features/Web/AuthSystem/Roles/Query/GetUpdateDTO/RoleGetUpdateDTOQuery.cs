using MediatR;
using Application.Repositories;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;
using Application.Exceptions;

namespace Application.Features.Web.Roles
{
    #region Request
    public class RoleGetUpdateDTOQuery
    : IRequest<RoleUpdateDTO>, IBaseEntityDTO
    {
        #region Constructors
        public RoleGetUpdateDTOQuery(long id)
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
    public class RoleGetUpdateDTOQueryHandler : IRequestHandler<RoleGetUpdateDTOQuery, RoleUpdateDTO>
    {
        private readonly IUnitOfWork _uow;
        public RoleGetUpdateDTOQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<RoleUpdateDTO> Handle(RoleGetUpdateDTOQuery request, CancellationToken cancellationToken)
        {
            var Role = await _uow.Roles.GetOneDTOAsync(
                RoleUpdateDTO.Selector,
                x => x.Id == request.Id);
            if (Role == null)
                throw new NotFoundException("اطلاعات نقش مورد نظر یافت نشد!");

            return Role;
        }
    }

    #endregion
}
