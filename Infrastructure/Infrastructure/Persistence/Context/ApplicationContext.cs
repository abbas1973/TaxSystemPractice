using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Data.Entity.Infrastructure;
using Application.Features.Web.Invoices;

namespace Infrastructure.Persistence
{
    public class ApplicationContext : BaseContext
    {
        #region Constructors
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IHttpContextAccessor accessor)
            : base(options, accessor, typeof(User).Assembly, typeof(ApplicationContext).Assembly) { }
        #endregion


        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        #endregion


    }

}
