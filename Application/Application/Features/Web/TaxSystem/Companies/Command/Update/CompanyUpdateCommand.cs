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

namespace Application.Features.Web.Companies
{
    #region Request
    public class CompanyUpdateCommand
    : BaseEntityDTO, IRequest<BaseResult>, IMapFrom<Company>
    {
        #region Constructors
        public CompanyUpdateCommand()
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
            profile.CreateMap<CompanyUpdateCommand, Company>();
        #endregion
    }
    #endregion



    #region Handler
    public class CompanyUpdateQueryHandler : IRequestHandler<CompanyUpdateCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CompanyUpdateQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<BaseResult> Handle(CompanyUpdateCommand request, CancellationToken cancellationToken)
        {
            #region یافتن اطلاعات شرکت
            var Company = await _uow.Companies.FirstOrDefaultAsync(
                x => x.Id == request.Id);

            if (Company == null)
                throw new NotFoundException("شرکت مورد نظر یافت نشد!");
            #endregion


            #region ویرایش اطلاعات شرکت
            Company = _mapper.Map(request, Company);
            _uow.Companies.Update(Company);
            #endregion

            await _uow.CommitAsync();
            return new BaseResult(true);
        }
    }

    #endregion
}
