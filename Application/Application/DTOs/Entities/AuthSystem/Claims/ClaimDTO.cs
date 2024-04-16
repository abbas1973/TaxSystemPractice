namespace Application.DTOs.Claims
{
    public class ClaimDTO
    {
        public string Name { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        /// <summary>
        /// کلایم برای دسترسی کلی کنترلر است یا خیر
        /// </summary>
        public bool IsController { get; set; }

        /// <summary>
        /// سطح فرورفتگی در درخت
        /// </summary>
        public int Level { get; set; }

        public string Claim
        {
            get
            {
                if (string.IsNullOrEmpty(Area))
                    return $"{Controller}.{(IsController ? "Full" : Action)}".Trim('.');
                else
                    return $"{Area}.{Controller}.{(IsController ? "Full" : Action)}".Trim('.');
            }
        }

        /// <summary>
        /// کلایم های زیر مجموعه که همان اکشن های کنترلر هستند
        /// </summary>
        public List<ClaimDTO> SubClaims { get; set; } = new List<ClaimDTO>();
    }
}
