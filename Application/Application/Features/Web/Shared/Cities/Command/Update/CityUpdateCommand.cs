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

namespace Application.Features.Web.Cities
{
    #region Request
    public class CityUpdateCommand
    : BaseEntityDTO, IRequest<BaseResult>, IMapFrom<City>
    {
        #region Constructors
        public CityUpdateCommand()
        {
        }
        #endregion


        #region Properties
        [Display(Name = "نام")]
        public string Name { get; set; }


        [Display(Name = "شناسه استان")]
        public long ProvinceId { get; set; }

        [Display(Name = "فعال است")]
        public bool IsEnabled { get; set; }
        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<CityUpdateCommand, City>();
        #endregion
    }
    #endregion



    #region Handler
    public class CityUpdateQueryHandler : IRequestHandler<CityUpdateCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CityUpdateQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult> Handle(CityUpdateCommand request, CancellationToken cancellationToken)
        {
            #region یافتن اطلاعات شهر
            var city = await _uow.Cities.FirstOrDefaultAsync(
                x => x.Id == request.Id);

            if (city == null)
                throw new NotFoundException("شهر مورد نظر یافت نشد!");
            #endregion


            #region ویرایش اطلاعات شهر
            city = _mapper.Map(request, city);
            _uow.Cities.Update(city);
            #endregion

            await _uow.CommitAsync();
            return new BaseResult(true);
        }
    }

    #endregion
}
