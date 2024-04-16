namespace Application.DTOs.Common.Swagger
{
    /// <summary>
    /// اتریبیوت برای نشان ندادن یکسری از پروپرتی ها در سوگر
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }

}
