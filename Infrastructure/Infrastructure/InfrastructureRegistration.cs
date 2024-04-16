﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Application.Repositories;
using Application.Contracts;
using Infrastructure.Implementation;

namespace Infrastructure
{
    public static class InfrastructureRegistration
    {
        /// <summary>
        /// کانفیگ دیتابیس به uow در سرویس
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DB Context
            var connectionString = configuration.GetConnectionString("ApplicationContext");
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString,
                                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

            services.AddScoped<DbContext, ApplicationContext>();
            #endregion


            #region DI
            services.AddScoped<IDataTableManager, DataTableManager>();
            services.AddScoped(typeof(IGenericBaseUnitOfWork<>), typeof(GenericBaseUnitOfWork<>));
            services.AddScoped(typeof(IExcelReportGenerator<>), typeof(ExcelReportGenerator<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtManager, JwtManager>();

            #endregion

        }


    }
}
