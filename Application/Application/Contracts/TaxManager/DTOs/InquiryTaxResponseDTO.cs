using System.Reflection;

namespace Application.Contracts
{
    /// <summary>
    /// خروجی پس از استعلام صورتحساب از سازمان مالیاتی
    /// </summary>
    public class InquiryTaxResponseDTO
    {
        #region Constructors
        public InquiryTaxResponseDTO()
        {
            
        }
        public InquiryTaxResponseDTO(string referenceNumber, string uid, string status, string packetType, string fiscalId, string data)
        {
            ReferenceNumber = referenceNumber;
            Uid = uid;
            Status = status;
            PacketType = packetType;
            FiscalId = fiscalId;
            Data = data;
        }
        #endregion


        #region Properties
        public string ReferenceNumber { get; set; }
        public string Uid { get; set; }
        public string Status { get; set; }
        public string StatusMessage => TaxInquiryStatusMessage.GetStatusMessage(Status);
        public string PacketType { get; set; }
        public string FiscalId { get; set; }
        public string Data { get; set; }
        #endregion
    }



    /// <summary>
    /// انواع استاتوس های برگشتی
    /// </summary>
    public class TaxInquiryStatusMessage
    {
        /// <summary>
        /// صورتحساب هنوز اعتبارسنجی نشده و در صف بررسی میباشد.
        /// </summary>
        public string IN_PROGRESS => "صورتحساب هنوز اعتبارسنجی نشده و در صف بررسی میباشد.";

        /// <summary>
        /// صورتحساب ارسالی دارای خطا بوده و رد شده است.
        /// </summary>
        public string FAILED => "صورتحساب ارسالی دارای خطا بوده و رد شده است.";

        /// <summary>
        /// صورتحساب فاقد خطا بود و با موفقیت در کارپوشه ثبت شد.
        /// </summary>
        public string SUCCESS => "صورتحساب فاقد خطا بود و با موفقیت در کارپوشه ثبت شد.";

        /// <summary>
        /// پردازش صورتحساب بیش از اندازه طول کشیده و در کارپوشه ثبت نشد.
        /// </summary>
        public string TIMEOUT => "پردازش صورتحساب بیش از اندازه طول کشیده و در کارپوشه ثبت نشد.";

        /// <summary>
        /// شماره پیگیری داده شده یافت نشد.
        /// </summary>
        public string NOT_FOUND => "شماره پیگیری داده شده یافت نشد.";


        #region گرفتن پیغام مناسب استاتوس
        public static string GetStatusMessage(string status)
        {
            var prop = typeof(TaxInquiryStatusMessage).GetProperties()
                .Where(x => x.Name == status)
                .FirstOrDefault();

            if (prop == null)
                return null;

            return prop.GetValue(new TaxInquiryStatusMessage()).ToString();
        } 
        #endregion
    }
}
