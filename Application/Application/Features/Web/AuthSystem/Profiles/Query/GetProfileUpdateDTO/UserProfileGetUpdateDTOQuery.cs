using MediatR;
using Application.Repositories;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;
using Application.Exceptions;
using Application.Features.Web.Users;

namespace Application.Features.Web.Profiles
{
    #region Request
    public class UserProfileGetUpdateDTOQuery
    : IRequest<UserProfileUpdateDTO>, IBaseEntityDTO
    {
        #region Constructors
        public UserProfileGetUpdateDTOQuery(long id)
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
    public class UserProfileGetUpdateDTOQueryHandler : IRequestHandler<UserProfileGetUpdateDTOQuery, UserProfileUpdateDTO>
    {
        private readonly IUnitOfWork _uow;
        public UserProfileGetUpdateDTOQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<UserProfileUpdateDTO> Handle(UserProfileGetUpdateDTOQuery request, CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetOneDTOAsync(
                UserProfileUpdateDTO.Selector,
                x => x.Id == request.Id);
            if (user == null)
                throw new NotFoundException("اطلاعات کاربر مورد نظر یافت نشد!");

            return user;
        }
    }

    #endregion
}
