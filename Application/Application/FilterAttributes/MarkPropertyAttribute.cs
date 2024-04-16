
namespace Application.Filters
{
	/// <summary>
	/// اتریبیوت برای مارک کردن پروپرتی ها برای اینکهاز بقیه قابل جدا سازی بشن
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	sealed public class MarkAttribute : Attribute
	{
		public MarkAttribute()
		{
		}

	}
}
