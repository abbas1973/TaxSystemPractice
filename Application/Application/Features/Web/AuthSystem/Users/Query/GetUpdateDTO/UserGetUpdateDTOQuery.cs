using MediatR;
using Application.Repositories;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;
using Application.Exceptions;

namespace Application.Features.Web.Users
{
    #region Request
    public class UserGetUpdateDTOQuery
    : IRequest<UserUpdateDTO>, IBaseEntityDTO
    {
        #region Constructors
        public UserGetUpdateDTOQuery(long id)
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
    public class UserGetUpdateDTOQueryHandler : IRequestHandler<UserGetUpdateDTOQuery, UserUpdateDTO>
    {
        private readonly IUnitOfWork _uow;
        public UserGetUpdateDTOQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<UserUpdateDTO> Handle(UserGetUpdateDTOQuery request, CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetOneDTOAsync(
                UserUpdateDTO.Selector,
                x => x.Id == request.Id);
            if (user == null)
                throw new NotFoundException("اطلاعات کاربر مورد نظر یافت نشد!");

            return user;
        }
    }

    #endregion
}
