using Application.DTOs;
using Application.Exceptions;
using Application.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Web.Companies
{
    #region Request
    public class CompanyGetUpdateDTOQuery
    : IRequest<CompanyUpdateDTO>, IBaseEntityDTO
    {
        #region Constructors
        public CompanyGetUpdateDTOQuery(long id)
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
    public class CompanyGetUpdateDTOQueryHandler : IRequestHandler<CompanyGetUpdateDTOQuery, CompanyUpdateDTO>
    {
        private readonly IUnitOfWork _uow;
        public CompanyGetUpdateDTOQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<CompanyUpdateDTO> Handle(CompanyGetUpdateDTOQuery request, CancellationToken cancellationToken)
        {
            var Company = await _uow.Companies.GetOneDTOAsync(
                CompanyUpdateDTO.Selector,
                x => x.Id == request.Id);
            if (Company == null)
                throw new NotFoundException("اطلاعات استان مورد نظر یافت نشد!");

            return Company;
        }
    }

    #endregion
}
