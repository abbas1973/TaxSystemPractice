using System.ComponentModel;

namespace Domain.Enums
{
    public enum BuyerType
    {
        /// <summary>
        /// حقیقی
        /// </summary>
        [Description("حقیقی")]
        Real = 1,

        /// <summary>
        /// حقوقی
        /// </summary>
        [Description("حقوقی")]
        Legal = 2
    }
}
