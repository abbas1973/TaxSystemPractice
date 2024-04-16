using System.Reflection;

namespace Application.Contracts
{
    /// <summary>
    /// خروجی پس از ارسال صورتحساب به سازمان مالیاتی
    /// </summary>
    public class SendTaxResponseDTO
    {
        #region Constructors
        public SendTaxResponseDTO()
        {
            
        }
        public SendTaxResponseDTO(
            long id,
            long serialNumber,
            DateTime sendDate,
            string taxId,
            string uid,
            string refNumber, 
            string errorCode, 
            string errorDetail)
        {
            Id = id;
            SerialNumber = serialNumber;
            SendDate = sendDate;
            TaxId = taxId;
            Uid = uid;
            RefNumber = refNumber;
            ErrorCode = errorCode;
            ErrorDetail = errorDetail;
        }

        public SendTaxResponseDTO(
            long id,
            long serialNumber,
            DateTime sendDate,
            string taxId)
        {
            Id = id;
            SerialNumber = serialNumber;
            SendDate = sendDate;
            TaxId = taxId;
        }

        public SendTaxResponseDTO(
            string uid,
            string refNumber,
            string errorCode,
            string errorDetail)
        {
            Uid = uid;
            RefNumber = refNumber;
            ErrorCode = errorCode;
            ErrorDetail = errorDetail;
        }
        #endregion


        #region Properties
        public long Id { get; set; }
        public long SerialNumber { get; set; }
        public DateTime SendDate { get; set; }
        public string TaxId { get; set; }
        public string Uid { get; set; }
        public string RefNumber { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetail { get; set; }
        #endregion
    }


}
