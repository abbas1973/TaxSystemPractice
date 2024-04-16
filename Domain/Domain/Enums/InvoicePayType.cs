using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum InvoicePayType
    {
        /// <summary>
		/// نقد
		/// </summary>
        [Description("نقد")]
        Cash = 1,


        /// <summary>
		/// نسیه
		/// </summary>
        [Description("نسیه")]
        Credit = 2,


        /// <summary>
        /// نقد/نسیه
        /// </summary>
        [Description("نقد/نسیه")]
        CashCredit = 3
    }
}
