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
    public class UserChangePasswordCommand
    : BaseEntityDTO, IRequest<BaseResult>, IPasswordDTO
    {
        #region Constructors
        public UserChangePasswordCommand()
        {
        }
        #endregion


        #region Properties       
        //[Display(Name = "کلمه عبور فعلی")]
        //public string CurrentPassword { get; set; }

        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        public string RePassword { get; set; }
        #endregion

    }
    #endregion



    #region Handler
    public class UserChangePasswordQueryHandler : IRequestHandler<UserChangePasswordCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UserChangePasswordQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult> Handle(UserChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetByIdAsync(request.Id);
            user.Password = request.Password.GetHash();
            await _uow.CommitAsync();
            return new BaseResult(true);
        }
    }

    #endregion
}
