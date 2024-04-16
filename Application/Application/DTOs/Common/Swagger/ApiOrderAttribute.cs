
namespace DTOs.Swagger
{
    /// <summary>
    /// برای تعیین ترتیب نمایش گروه ها در swagger
    /// </summary>
    public class ApiOrderAttribute : Attribute
    {
        public ApiOrderAttribute(int order)
        {
            Order = order;
        }
        public int Order { get; set; }
    }
}
