using Application.DTOs;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Reflection;

namespace Application.Contracts
{
	public interface IExcelReportGenerator<T> where T : class
	{

		/// <summary>
		/// لیست پروپرتی های یک مدل بودن پروپرتی های از نوع لیست و کالکشن و اینام
		/// </summary>
		/// <returns></returns>
		List<PropertyDetailDTO> GetProperties();


		/// <summary>
		/// آیا پروپرتی خاصی در مدل وجود دارد؟
		/// </summary>
		/// <param name="propertyname">نام پروپرتی</param>
		/// <returns></returns>
		bool HasProperty(string propertyname);



		/// <summary>
		/// گرفتن نام نمایشی پروپرتی
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		string GetDisplayName(PropertyInfo property);



		/// <summary>
		/// تبدیل لیست به دیتا تیبل
		/// </summary>
		/// <param name="items">لیستی از ایتم ها</param>
		/// <returns></returns>
		DataTable ToDataTable(IEnumerable<T> items);




		/// <summary>
		/// ساخت گزارش
		/// </summary>
		/// <param name="OutputPath">آدرس خروجی که فایل باید ذخیره شود</param>
		/// <param name="model">مدلی که گزارش از آن گرفته میشود</param>
		/// <returns></returns>
		bool GenerateReport(string OutputPath, string SheetTitle, IEnumerable<T> model);




		/// <summary>
		/// ساخت فایل اکسل برای گزارش بدون ذخیره روی هارد
		/// </summary>
		/// <param name="OutputPath">آدرس خروجی که فایل باید ذخیره شود</param>
		/// <param name="model">مدلی که گزارش از آن گرفته میشود</param>
		/// <returns></returns>
		IXLWorkbook GenerateReportWorkBook(string SheetTitle, IEnumerable<T> model);




		/// <summary>
		/// گرفتن خروجی دیتاتیبل از فایل اکسل و تبدیل به لیستی از آبجکتا 
		/// </summary>
		/// <param name="filePath">آدرس فایل اکسل</param>
		/// <returns></returns>
		DataTable ImportExceltoDatatable(string filePath);



		/// <summary>
		/// گرفتن خروجی دیتاتیبل از فایل اکسل و تبدیل به لیستی از آبجکتا 
		/// </summary>
		/// <param name="filePath">آدرس فایل اکسل</param>
		/// <returns></returns>
		BaseResult<DataTable> ImportExceltoDatatable(IFormFile file, bool ExcelHasHeader = false);



		/// <summary>
		/// تبدیل اکسل به لیستی از مدل
		/// </summary>
		BaseResult<List<T>> ImportExcelToDtoList(IFormFile file, bool ExcelHasHeader = false);
	}
}
