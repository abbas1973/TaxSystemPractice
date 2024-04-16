using Application.DTOs;
using Application.Exceptions;
using Application.Repositories;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Web.Companies
{
    #region Request
    public class CompanyDeleteCommand
    : IRequest<BaseResult>, IBaseEntityDTO
    {
        #region Constructors
        public CompanyDeleteCommand(long id)
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
    public class CompanyDeleteQueryHandler : IRequestHandler<CompanyDeleteCommand, BaseResult>
    {
        private readonly IUnitOfWork _uow;
        public CompanyDeleteQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
        }


        public async Task<BaseResult> Handle(CompanyDeleteCommand request, CancellationToken cancellationToken)
        {
            var deletedCompanies = await _uow.Companies.ExecuteDeleteAsync(x => x.Id == request.Id);
            if (deletedCompanies == 0)
                throw new NotFoundException("استان مورد نظر یافت نشد!");
            return new BaseResult(true);
        }
    }

    #endregion
}
