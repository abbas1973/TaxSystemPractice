using Application.DTOs;
using Application.Repositories;
using AutoMapper;
using Base.Application.Mapping;
using Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Web.Companies
{
    #region Request
    public class CompanyCreateCommand
    : IRequest<BaseResult<long>>, IMapFrom<Company>
    {
        #region Constructors
        public CompanyCreateCommand()
        {

        }
        #endregion


        #region Properties
        [Display(Name = "نام")]
        public string Name { get; set; }

        /// <summary>
        /// کلید اتصال به سامانه مودیان
        /// </summary>
        [Display(Name = "کلید اختصاصی سامانه مودیان")]
        public string PrivateKey { get; set; }


        [Display(Name = "شناسه ملی")]
        public string NationalCode { get; set; }

        /// <summary>
        /// شناسه اقتصادی شرکت
        /// </summary>
        [Display(Name = "شناسه اقتصادی")]
        public string EconomicCode { get; set; }

        /// <summary>
        /// کلاینت آیدی از سامانه مودیان
        /// </summary>
        [Display(Name = "کلاینت آیدی سامانه مودیان")]
        public string ClientId { get; set; }
        #endregion



        #region Mapping
        public void Mapping(Profile profile) =>
            profile.CreateMap<CompanyCreateCommand, Company>();
        #endregion
    }
    #endregion



    #region Handler
    public class CompanyCreateQueryHandler : IRequestHandler<CompanyCreateCommand, BaseResult<long>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CompanyCreateQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult<long>> Handle(CompanyCreateCommand request, CancellationToken cancellationToken)
        {
            var Company = _mapper.Map<Company>(request);
            await _uow.Companies.AddAsync(Company);
            await _uow.CommitAsync();
            return new BaseResult<long>(Company.Id);
        }
    }

    #endregion
}
