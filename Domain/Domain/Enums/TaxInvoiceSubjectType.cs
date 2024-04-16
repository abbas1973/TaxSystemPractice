using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    /// <summary>
    /// موضوع صورتحساب
    /// </summary>
    public enum TaxInvoiceSubjectType
    {
        /// <summary>
		/// صورتحساب اصلی فروش
		/// </summary>
        [Description("صورتحساب اصلی فروش")]
        Main = 1,


        /// <summary>
		/// صورتحساب الکترونیکی اصلاحی
		/// </summary>
        [Description("صورتحساب الکترونیکی اصلاحی")]
        Edited = 2,


        /// <summary>
        /// صورتحساب الکترونیکی ابطالی
        /// </summary>
        [Description("صورتحساب الکترونیکی ابطالی")]
        Cancelled = 3,


        /// <summary>
        /// صورتحساب الکترونیکی برگشت از فروش
        /// </summary>
        [Description("صورتحساب الکترونیکی برگشت از فروش")]
        Rejected = 4


    }
}
