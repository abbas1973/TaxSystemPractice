namespace Application.DTOs
{
    /// <summary>
    /// مدلی که برای ارسال به نوتیفیکیشن لاگ استفاده می شود
    /// </summary>
    public class LogParameterDTO
    {
        #region Constructors
        public LogParameterDTO() { }
        public LogParameterDTO(string title, string key, object value)
        {
            Title = title;
            Key = key;
            Value = value;
        }

        #endregion


        #region Properties
        /// <summary>
        /// عنوان فارسی
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// کلید که در seq قابل فیلتر باشد
        /// <para>
        /// بدون علامت @ در ابتدا
        /// </para>
        /// </summary>
        public string Key { get; set; }


        /// <summary>
        /// مقدار مورد نظر. میتواند آبجکت باشد
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// خروجی مناسب برای لاگ
        /// </summary>
        public string LogText => Title + ": {@" + Key + "}";
        #endregion


    }
}
