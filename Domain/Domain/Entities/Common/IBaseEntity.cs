using System;

namespace Domain.Entities
{
    public interface IBaseEntity
    {
        long Id { get; set; }


        #region مشخصات ایجاد
        long? CreatedBy { get; set; }
        DateTime CreateDate { get; set; }
        #endregion


        #region مشخصات ویرایش
        long? LastModifiedBy { get; set; }
         DateTime? ModifyDate { get; set; }
        #endregion
    }
}
