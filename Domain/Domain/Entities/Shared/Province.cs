namespace Domain.Entities
{
    /// <summary>
    /// استان ها
    /// </summary>
    public class Province : BaseEntity, IIsEnabled
    {
        #region Properties
        public string Name { get; set; }
        public bool IsEnabled { get; set; } 
        #endregion


        #region Navigation Properties
        /// <summary>
        /// شهرهای استان
        /// </summary>
        public ICollection<City> Cities { get; set; }
        #endregion

    }
}
