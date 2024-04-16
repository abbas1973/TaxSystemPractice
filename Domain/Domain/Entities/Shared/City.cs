namespace Domain.Entities
{
    /// <summary>
    /// شهرها
    /// </summary>
    public class City : BaseEntity,IIsEnabled
    {
        #region Properties
        public string Name { get; set; }

        public long ProvinceId { get; set; }
        public Province Province { get; set; }

        public bool IsEnabled { get; set; } 
        #endregion


        #region Navigation Properties
        public ICollection<User> Users { get; set; }
        #endregion
    }
}
