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

namespace Application.Features.Web.Cities
{
    #region Request
    public class CityCreateCommand
    : IRequest<BaseResult<long>>, IMapFrom<City>
    {
        #region Constructors
        public CityCreateCommand()
        {
            
        }
        #endregion


        #region Properties
        [Display(Name = "نام")]
        public string Name { get; set; }


        [Display(Name = "استان")]
        public long ProvinceId { get; set; }
        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<CityCreateCommand, City>();
        #endregion
    }
    #endregion



    #region Handler
    public class CityCreateQueryHandler : IRequestHandler<CityCreateCommand, BaseResult<long>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CityCreateQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult<long>> Handle(CityCreateCommand request, CancellationToken cancellationToken)
        {
            var city = _mapper.Map<City>(request);
            await _uow.Cities.AddAsync(city);
            await _uow.CommitAsync();
            return new BaseResult<long>(city.Id);
        }
    }

    #endregion
}
