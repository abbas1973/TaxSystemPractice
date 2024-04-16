using MediatR;
using DTOs.DataTable;
using System.Linq.Expressions;
using Domain.Entities;
using LinqKit;
using System.Linq.Dynamic.Core;
using Application.Repositories;
using Application.Contracts;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Utilities;
using Application.DTOs;
using AutoMapper;

namespace Application.Features.Web.Invoices
{
    #region Request
    public class InvoiceGetEditDTOQuery
    : BaseEntityDTO, IRequest<InvoiceEditDTO>
    {
        #region Constructors
        public InvoiceGetEditDTOQuery()
        {
        }

        public InvoiceGetEditDTOQuery(long id)
        {
            Id = id;
        }
        #endregion


        #region توابع
        public Expression<Func<Invoice, bool>> GetFilter()
        {
            var filter = PredicateBuilder.New<Invoice>(true);

            filter.And(x => x.Id == Id);

            return filter;
        }
        #endregion

    }
    #endregion



    #region Handler
    public class InvoiceGetEditDTOQueryHandler : IRequestHandler<InvoiceGetEditDTOQuery, InvoiceEditDTO>
    {
        private readonly IDataTableManager _dataTableManager;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public InvoiceGetEditDTOQueryHandler(IUnitOfWork uow, IDataTableManager dataTableManager, IMapper mapper)
        {
            _uow = uow;
            _dataTableManager = dataTableManager;
            _mapper = mapper;
        }


        public async Task<InvoiceEditDTO> Handle(InvoiceGetEditDTOQuery request, CancellationToken cancellationToken)
        {
            var filter = request.GetFilter();
           
            var model = await _uow.Invoices.ProjectToOneAsync<InvoiceEditDTO>(
                _mapper.ConfigurationProvider,
                filter);

            return model;
        }
    }

    #endregion
}
