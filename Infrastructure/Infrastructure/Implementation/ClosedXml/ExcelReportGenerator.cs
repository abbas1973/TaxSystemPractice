using Application.DTOs;
using Application.Exceptions;
using Application.Filters;
using ClosedXML.Excel;
using Application.Contracts;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Utilities;

namespace Infrastructure.Implementation
{
    /// <summary>
    /// گرفتن خروجی اکسل از مدل مربوطه
    /// </summary>
    public class ExcelReportGenerator<T> : IExcelReportGenerator<T> where T : class
    {
        private ILogger<ExcelReportGenerator<T>> _logger;
        public ExcelReportGenerator(ILogger<ExcelReportGenerator<T>> logger)
        {
            _logger = logger;
        }


        #region لیست پروپرتی های یک مدل بودن پروپرتی های از نوع لیست و کالکشن و اینام
        /// <summary>
        /// لیست پروپرتی های یک مدل بودن پروپرتی های از نوع لیست و کالکشن و اینام
        /// </summary>
        /// <returns></returns>
        public List<PropertyDetailDTO> GetProperties()
        {
            Type myType = typeof(T);
            var props = myType.GetProperties()
                              .Where(p => !typeof(IList<>).IsAssignableFrom(p.PropertyType)
                                       && !typeof(List<>).IsAssignableFrom(p.PropertyType)
                                       && !typeof(IEnumerable<>).IsAssignableFrom(p.PropertyType)
                                       && !typeof(ICollection<>).IsAssignableFrom(p.PropertyType)
                                       && !typeof(Expression).IsAssignableFrom(p.PropertyType));

            var Properties = props.Select(x => new PropertyDetailDTO
            {
                TitleEn = x.Name,
                TitleFa = GetDisplayName(x) ?? x.Name
            }).ToList();

            return Properties;
        }
        #endregion




        #region آیا پروپرتی خاصی در مدل وجود دارد؟
        /// <summary>
        /// آیا پروپرتی خاصی در مدل وجود دارد؟
        /// </summary>
        /// <param name="propertyname">نام پروپرتی</param>
        /// <returns></returns>
        public bool HasProperty(string propertyname)
        {
            Type myType = typeof(T);
            return myType.GetProperties()
                              .Any(p => p.Name.ToLower() == propertyname.ToLower());
        }
        #endregion




        #region گرفتن نام نمایشی پروپرتی
        /// <summary>
        /// گرفتن نام نمایشی پروپرتی
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public string GetDisplayName(PropertyInfo property)
        {
            string displayName = null;
            var attr = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            if (attr != null)
                displayName = attr.Name;
            return displayName;
        }
        #endregion




        #region تبدیل لیست به دیتا تیبل
        /// <summary>
        /// تبدیل لیست به دیتا تیبل
        /// به ازای پروپرتی هایی که اتریبیوت مارک ندارند
        /// </summary>
        /// <param name="items">لیستی از ایتم ها</param>
        /// <returns></returns>
        public DataTable ToDataTable(IEnumerable<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            #region ایجاد ستون دیتاتیبل به ازای هر پروپرتی
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                var attr = Attribute.GetCustomAttribute(prop, typeof(MarkAttribute)) as MarkAttribute;

                if (attr == null)
                {
                    var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                    string coulmnName = GetDisplayName(prop) ?? prop.Name;
                    dataTable.Columns.Add(coulmnName, type);
                }
            }
            #endregion

            #region ریختن دیتا درون دیتاتیبل
            foreach (T item in items)
            {
                DataRow dr = dataTable.NewRow();
                foreach (PropertyInfo prop in Props)
                {
                    var attr = Attribute.GetCustomAttribute(prop, typeof(MarkAttribute)) as MarkAttribute;
                    if (attr == null)
                    {
                        string coulmnName = GetDisplayName(prop) ?? prop.Name;
                        dr[coulmnName] = prop.GetValue(item, null) ?? DBNull.Value;
                    }
                }
                dataTable.Rows.Add(dr);
            }
            #endregion

            return dataTable;
        }
        #endregion




        #region ساخت گزارش
        /// <summary>
        /// ساخت گزارش
        /// </summary>
        /// <param name="OutputPath">آدرس خروجی که فایل باید ذخیره شود</param>
        /// <param name="model">مدلی که گزارش از آن گرفته میشود</param>
        /// <returns></returns>
        public bool GenerateReport(string OutputPath, string SheetTitle, IEnumerable<T> model)
        {
            try
            {
                var dt = ToDataTable(model);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(dt, SheetTitle);
                    worksheet.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.RightToLeft = true; //راست چین کردن اکسل
                    worksheet.Columns().AdjustToContents();  // Adjust column width
                    worksheet.Rows().AdjustToContents();     // Adjust row heights
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + OutputPath);
                    workbook.SaveAs(_Path);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion




        #region ساخت فایل اکسل برای گزارش بدون ذخیره روی هارد
        /// <summary>
        /// ساخت فایل اکسل برای گزارش بدون ذخیره روی هارد
        /// </summary>
        /// <param name="OutputPath">آدرس خروجی که فایل باید ذخیره شود</param>
        /// <param name="model">مدلی که گزارش از آن گرفته میشود</param>
        /// <returns></returns>
        public IXLWorkbook GenerateReportWorkBook(string SheetTitle, IEnumerable<T> model)
        {
            try
            {
                var dt = ToDataTable(model);
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(dt, SheetTitle);
                worksheet.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.RightToLeft = true; //راست چین کردن اکسل
                worksheet.Columns().AdjustToContents();  // Adjust column width
                worksheet.Rows().AdjustToContents();     // Adjust row heights
                return workbook;
            }
            catch (Exception e)
            {
                throw new InternalServerException("ایجاد دیتاتیبل با خطا همراه بوده است!");
            }
        }
        #endregion





        #region گرفتن خروجی دیتاتیبل از فایل اکسل و تبدیل به لیستی از آبجکتا 
        /// <summary>
        /// گرفتن خروجی دیتاتیبل از فایل اکسل و تبدیل به لیستی از آبجکتا 
        /// </summary>
        /// <param name="filePath">آدرس فایل اکسل</param>
        /// <returns></returns>
        public DataTable ImportExceltoDatatable(string filePath)
        {
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                IXLWorksheet workSheet = workBook.Worksheet(1);
                DataTable dt = new DataTable();

                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                    string coulmnName = prop.Name;
                    dt.Columns.Add(coulmnName);
                }

                bool firstRow = true;
                var rows = workSheet.RowsUsed();
                int start = 0;
                int end = 0;
                foreach (IXLRow row in rows)
                {
                    if (firstRow)
                    {
                        firstRow = false;
                        start = row.FirstCellUsed().Address.ColumnNumber;
                        end = row.LastCellUsed().Address.ColumnNumber;
                    }
                    else
                    {
                        dt.Rows.Add();
                        int i = 0;

                        foreach (IXLCell cell in row.Cells(start, end))
                        {
                            string val = null;
                            if (cell.HasFormula)
                                val = cell.CachedValue.ToString();
                            else
                                val = cell.Value.ToString();

                            dt.Rows[dt.Rows.Count - 1][i] = string.IsNullOrEmpty(val) ? null : val;// cell.GetValue<type>();
                            i++;
                        }
                    }
                }

                return dt;
            }
        }
        #endregion





        #region گرفتن خروجی دیتاتیبل از فایل اکسل و تبدیل به لیستی از آبجکتا 
        /// <summary>
        /// گرفتن خروجی دیتاتیبل از فایل اکسل و تبدیل به لیستی از آبجکتا 
        /// </summary>
        /// <param name="filePath">آدرس فایل اکسل</param>
        /// <returns></returns>
        public BaseResult<DataTable> ImportExceltoDatatable(IFormFile file, bool ExcelHasHeader = false)
        {
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);

                // Open the Excel file using ClosedXML.
                using (XLWorkbook workBook = new XLWorkbook(stream))
                {
                    //Read the first Sheet from Excel file.
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Create a new DataTable.
                    DataTable dt = new DataTable();

                    //Get all the properties
                    PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (PropertyInfo prop in Props)
                    {
                        //Defining type of data column gives proper data table 
                        var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                        //Setting column names as Property names
                        string coulmnName = prop.Name;
                        dt.Columns.Add(coulmnName);
                    }

                    if (workSheet.ColumnsUsed().Count() > dt.Columns.Count)
                        return new BaseResult<DataTable>(false, "فایل اکسل باید حاوی تنها " + dt.Columns.Count + " ستون داده باشد!");


                    //Loop through the Worksheet rows.
                    bool firstRow = true;
                    var rows = workSheet.RowsUsed();
                    int start = 0;
                    int end = 0;
                    foreach (IXLRow row in rows)
                    {
                        //Use the first row to add columns to DataTable.
                        if (firstRow)
                        {
                            firstRow = false;
                            start = row.FirstCellUsed().Address.ColumnNumber;
                            end = row.LastCellUsed().Address.ColumnNumber;
                        }
                        if (!firstRow || !ExcelHasHeader)
                        {
                            //Add rows to DataTable.
                            dt.Rows.Add();
                            int i = 0;

                            foreach (IXLCell cell in row.Cells(start, end))
                            {
                                //var prop = Props.Where(x => x.Name == dt.Columns[i].ColumnName);
                                //var type = dt.Columns[i].DataType;

                                //var val = cell.Value?.ToString();
                                //var val = cell.GetFormattedString();

                                string val = null;
                                if (cell.HasFormula)
                                    val = cell.CachedValue.ToString();
                                else
                                    val = cell.Value.ToString();

                                dt.Rows[dt.Rows.Count - 1][i] = string.IsNullOrEmpty(val) ? null : val;// cell.GetValue<type>();
                                i++;
                            }
                        }
                    }

                    return new BaseResult<DataTable>(dt);
                }
            }
        }
        #endregion






        #region تبدیل اکسل به لیستی از مدل
        /// <summary>
        /// تبدیل اکسل به لیستی از مدل
        /// </summary>
        public BaseResult<List<T>> ImportExcelToDtoList(IFormFile file, bool ExcelHasHeader = false)
        {
            try
            {
                var model = new List<T>();
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);

                    using (XLWorkbook workBook = new XLWorkbook(stream))
                    {
                        IXLWorksheet workSheet = workBook.Worksheet(1);
                        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(prop => !Attribute.IsDefined(prop, typeof(MarkAttribute))).ToArray();
                        if (workSheet.ColumnsUsed().Count() > Props.Length)
                            return new BaseResult<List<T>>(false, "فایل اکسل باید حاوی تنها " + Props.Length + " ستون داده باشد!");


                        bool firstRow = true;
                        var rows = workSheet.RowsUsed();
                        int start = 0;
                        int end = 0;
                        foreach (IXLRow row in rows)
                        {
                            if (firstRow)
                            {
                                firstRow = false;
                                start = row.FirstCellUsed().Address.ColumnNumber;
                                end = row.LastCellUsed().Address.ColumnNumber;

                                if (ExcelHasHeader)
                                    continue;
                            }

                            // ایجاد شی‌ء جدید از مدل مورد نظر
                            var tmp = (T)Activator.CreateInstance(typeof(T));
                            int i = 0;
                            foreach (IXLCell cell in row.Cells(start, end))
                            {
                                // گرفتن مقدار از اکسل
                                string val = null;
                                if (cell.HasFormula)
                                    val = cell.CachedValue.ToString()?.Trim();
                                else
                                    val = cell.Value.ToString()?.Trim();

                                // تبدیل داده گرفته شده از کسل به نوع داده پروپرتی متناظر
                                var prop = Props[i];
                                if (string.IsNullOrWhiteSpace(val))
                                    prop.SetValue(tmp, null, null); // مقدار دیفالت
                                else
                                    //prop.SetValue(tmp, Convert.ChangeType(val, prop.PropertyType), null); // مقدار خوانده شده از اکسل
                                    prop.SetValue(tmp, ReflectionExtension.ChangeType(val, prop.PropertyType)); // مقدار خوانده شده از اکسل
                                i++;
                            }

                            model.Add(tmp);

                        }

                        return new BaseResult<List<T>>(model);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new BaseResult<List<T>>(false, ex.ToString());
            }

        }
        #endregion






    }


}
