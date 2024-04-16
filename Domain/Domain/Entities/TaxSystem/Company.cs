namespace Domain.Entities
{
    public class Company : BaseEntity
    {
        #region Properties
        /// <summary>
        /// نام شرکت
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// کلید اتصال به سامانه مودیان
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// کلاینت آیدی از سامانه مودیان
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// شناسه ملی شرکت
        /// </summary>
        public string NationalCode { get; set; }


        /// <summary>
        /// شناسه اقتصادی شرکت
        /// </summary>
        public string EconomicCode { get; set; }
        #endregion



        #region Navigation Properties
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<User> Users { get; set; }
        #endregion

    }
}
