using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Domain.Entities;
using Base.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Application.SessionServices;

namespace Infrastructure.Persistence
{
    public class BaseContext : DbContext
    {
        #region Private fields
        protected ISession _session { get; }
        protected Assembly _domainAssembly { get; }
        protected Assembly _infrastructureAssembly { get; }
        #endregion


        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options">آپشن های دیتابیس</param>
        /// <param name="accessor">اکسسور</param>
        /// <param name="domainAssembly">اسمبلی دامین</param>
        /// <param name="infrastructureAssembly">اسمبلی اینفراستراکچر</param>
        public BaseContext(DbContextOptions options, IHttpContextAccessor accessor, Assembly domainAssembly, Assembly infrastructureAssembly)
            : base(options)
        {
            _session = accessor?.HttpContext?.Session;
            _domainAssembly = domainAssembly;
            _infrastructureAssembly = infrastructureAssembly;
        }
        #endregion



        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Fluent API - لود کردن از کلاس های جانبی
            modelBuilder.ApplyConfigurationsFromAssembly(_infrastructureAssembly);
            #endregion


            #region DeleteBehavior - رفتار در هنگام حذف دیتا
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            #endregion


            #region رجیستر کردن همه DBSet ها
            modelBuilder.RegisterAllEntities<BaseEntity>(_domainAssembly);
            #endregion


            #region اعمال حذف نرم افزاری در کوئری ها
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                if (typeof(IIsDeleted).IsAssignableFrom(entityType.ClrType))
                    modelBuilder.SetSoftDeleteFilter(entityType.ClrType);
            #endregion
        } 
        #endregion



        #region SaveChanges Override
        #region SaveChangesAsync
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetCreatorAndModifier();
            SetIsEnabled();
            SetIsDeleted();

            var res = base.SaveChangesAsync(cancellationToken);
            return res;
        }
        #endregion


        #region SaveChanges
        public override int SaveChanges()
        {
            SetCreatorAndModifier();
            SetIsEnabled();
            SetIsDeleted();

            var res = base.SaveChanges();
            return res;
        }
        #endregion


        #region مقدار دهی اطلاعات ایجاد و ویرایش
        public void SetCreatorAndModifier()
        {
            var user = _session?.GetUser();
            foreach (var entry in ChangeTracker.Entries<IBaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreateDate = DateTime.Now;
                    entry.Entity.CreatedBy = user?.Id;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifyDate = DateTime.Now;
                    entry.Entity.LastModifiedBy = user?.Id;
                }
            }
        }
        #endregion


        #region فعال کردن موجودیت هنگام افزودن به دیتابیس
        /// <summary>
        /// در صورت وجود پروپرتی IsEnabled
        /// مقدار آن هنگام افزودن موجودیت جدید فعال میشود
        /// </summary>
        public void SetIsEnabled()
        {
            var entities = ChangeTracker.Entries<IIsEnabled>()
                        .Where(e => e.State == EntityState.Added);
            foreach (var entity in entities)
                entity.Entity.IsEnabled = true;
        }
        #endregion


        #region حذف نرم افزاری المان های مورد نیاز
        /// <summary>
        /// تبدیل حذف فیزیکی به حذف نرم افزاری
        /// </summary>
        public void SetIsDeleted()
        {
            var entities = ChangeTracker.Entries<IIsDeleted>()
                        .Where(e => e.State == EntityState.Deleted);
            foreach (var entity in entities)
            {
                entity.State = EntityState.Modified;
                entity.Entity.IsDeleted = true;
            }
        }
        #endregion

        #endregion



    }

}

