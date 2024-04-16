using Application.DTOs;
using Application.Repositories;
using AutoMapper;
using Base.Application.Mapping;
using Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Web.Provinces
{
    #region Request
    public class ProvinceCreateCommand
    : IRequest<BaseResult<long>>, IMapFrom<Province>
    {
        #region Constructors
        public ProvinceCreateCommand()
        {

        }
        #endregion


        #region Properties
        [Display(Name = "نام")]
        public string Name { get; set; }
        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<ProvinceCreateCommand, Province>();
        #endregion
    }
    #endregion



    #region Handler
    public class ProvinceCreateQueryHandler : IRequestHandler<ProvinceCreateCommand, BaseResult<long>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProvinceCreateQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult<long>> Handle(ProvinceCreateCommand request, CancellationToken cancellationToken)
        {
            var province = _mapper.Map<Province>(request);
            await _uow.Provinces.AddAsync(province);
            await _uow.CommitAsync();
            return new BaseResult<long>(province.Id);
        }
    }

    #endregion
}
