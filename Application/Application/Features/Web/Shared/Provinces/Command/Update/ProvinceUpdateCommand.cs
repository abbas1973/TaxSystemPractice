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

namespace Application.Features.Web.Provinces
{
    #region Request
    public class ProvinceUpdateCommand
    : BaseEntityDTO, IRequest<BaseResult>, IMapFrom<Province>
    {
        #region Constructors
        public ProvinceUpdateCommand()
        {
        }
        #endregion


        #region Properties
        [Display(Name = "نام")]
        public string Name { get; set; }


        [Display(Name = "شناسه استان")]
        public long ProvinceId { get; set; }
        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<ProvinceUpdateCommand, Province>();
        #endregion
    }
    #endregion



    #region Handler
    public class ProvinceUpdateQueryHandler : IRequestHandler<ProvinceUpdateCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProvinceUpdateQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult> Handle(ProvinceUpdateCommand request, CancellationToken cancellationToken)
        {
            #region یافتن اطلاعات استان
            var province = await _uow.Provinces.FirstOrDefaultAsync(
                x => x.Id == request.Id);

            if (province == null)
                throw new NotFoundException("استان مورد نظر یافت نشد!");
            #endregion


            #region ویرایش اطلاعات استان
            province = _mapper.Map(request, province);
            _uow.Provinces.Update(province);
            #endregion

            await _uow.CommitAsync();
            return new BaseResult(true);
        }
    }

    #endregion
}
