using System;

namespace Domain.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        #region Constructors
        public BaseEntity()
        {
            CreateDate = DateTime.Now;
        }
        #endregion


        #region Properties
        public long Id { get; set; }


        #region مشخصات ایجاد
        public long? CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        #endregion


        #region مشخصات ویرایش
        public long? LastModifiedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        #endregion 
        #endregion

    }
}
