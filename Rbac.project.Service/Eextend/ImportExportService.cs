using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rbac.project.Service.Eextend
{
    public static class ImportExportService
    {
        /// <summary>
        /// 导出excal封装
        /// </summary>
        /// <typeparam name ="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>

        public static byte[] ListToExcelPack<T>(this List<T> data)
        {
            // 2007版本
            string sheetName = "sheet";
            bool isColumnWritten = true;
            IWorkbook workbook = new XSSFWorkbook();
            try
            {
                var sheet = workbook.CreateSheet(sheetName);
                //创建首行的样式
                ICellStyle s = workbook.CreateCellStyle();
                s.FillForegroundColor = HSSFColor.BlueGrey.Index;
                s.FillPattern = FillPattern.SolidForeground;
                var count = 0;
                var list = new List<string>();
                //标题
                PropertyInfo[] properties = typeof(T).GetProperties();//反射获取属性名称和displayName
                if (isColumnWritten)
                {
                    var row = sheet.CreateRow(0);
                    //循环填入首行
                    for (int j = 0; j < properties.Count(); j++)
                    {
                        var item = properties[j];
                        var attrs = item.GetCustomAttributes(typeof(DisplayNameAttribute), true);//反射获取属性
                        if (attrs != null && attrs.Count() > 0)
                        {
                            var displayName = ((DisplayNameAttribute)attrs[0]).DisplayName;
                            row.CreateCell(list.Count()).SetCellValue(displayName);
                            row.GetCell(list.Count()).CellStyle = s;
                            list.Add(item.Name);
                        }
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }
                int totalRow = sheet.LastRowNum;
                // 总列数（1开始）
                int totalColumn = sheet.GetRow(0).LastCellNum;
                var a = sheet.GetRow(0);
                if (data.Count > 0)
                {
                    //循环向行中填入列数据
                    for (var i = 0; i < data.Count; ++i)
                    {
                        var itemData = data[i];
                        var row = sheet.CreateRow(count);
                        for (int iCell = 0; iCell < list.Count; iCell++)
                        {
                            var p = list[iCell];
                            var Properties = itemData.GetType().GetProperties().Where(c => c.Name == p).FirstOrDefault();
                            var value = Properties.GetValue(itemData)?.ToString();
                            row.CreateCell(iCell).SetCellValue(value);
                        }
                        ++count;
                    }
                }
                else
                {
                    var row = sheet.CreateRow(count);
                    for (int iCell = 0; iCell < list.Count; iCell++)
                        row.CreateCell(iCell).SetCellValue("");
                }

                //调整表格宽度防止没有显示数据
                for (int columnNum = 0; columnNum <= list.Count; columnNum++)
                {
                    int columnWidth = sheet.GetColumnWidth(columnNum) / 256;//获取当前列宽度
                    for (int rowNum = 0; rowNum <= sheet.LastRowNum; rowNum++)//在这一列上循环行
                    {
                        IRow currentRow = sheet.GetRow(rowNum);
                        ICell currentCell = currentRow.GetCell(columnNum);
                        if (currentCell != null && !string.IsNullOrEmpty(currentCell.ToString()))
                        {
                            int length = Encoding.UTF8.GetBytes(currentCell.ToString()).Length;//获取当前单元格的内容宽度
                            if (columnWidth < length + 1)
                            {
                                columnWidth = length + 1;
                            }//若当前单元格内容宽度大于列宽，则调整列宽为当前单元格宽度，后面的+1是我人为的将宽度增加一个字符
                        }
                    }
                    sheet.SetColumnWidth(columnNum, columnWidth * 256);
                }

                //创建内存流
                MemoryStream ms = new MemoryStream();
                //写入到excel
                //var ms = new MemoryStream();
                Console.WriteLine(ms.CanRead);
                Console.WriteLine(ms.CanWrite);//防止流写入失败

                workbook.Write(ms, true);//写入ms
                ms.Flush();//清空流
                ms.Seek(0, SeekOrigin.Begin);
                byte[] bytes = ms.ToArray();//转byte类型(非必要)
                return bytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>

        public static List<T> ExcelToListPack<T>(this Stream stream, string fileName) where T : new()
        {
            IWorkbook wook = null;
            string ext = fileName.Substring(fileName.LastIndexOf("."), fileName.Length - fileName.LastIndexOf("."));
            stream.Position = 0;
            if (ext.ToLower() == ".xlsx")
            {
                wook = new XSSFWorkbook(stream);
            }
            else
            {
                wook = new HSSFWorkbook(stream);
            }

            ISheet sheet = wook.GetSheetAt(0);
            IRow row = sheet.GetRow(0);
            int totalColumn = row.LastCellNum;
            int total = sheet.LastRowNum;
            //通过反射获取类的属性写入Excel表头
            Dictionary<string, int> dic = new Dictionary<string, int>();
            var properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                object[] attr = properties[i].GetCustomAttributes(typeof(DisplayNameAttribute), true);
                if (attr.Length == 0)
                {
                    continue;
                }
                string desc = ((DisplayNameAttribute)attr[0]).DisplayName;
                if (!string.IsNullOrEmpty(desc))
                {
                    dic.Add(desc, i);
                }
            }
            string value = string.Empty;
            string type = string.Empty;
            int index = 0;
            List<T> list = new List<T>();

            for (int i = 1; i <= total; i++)
            {
                IRow rows = sheet.GetRow(i);
                if (rows == null)
                {
                    continue;
                }
                T t = new T() { };

                var obj = new T();
                for (int j = 0; j < totalColumn; j++)
                {
                    if (dic.TryGetValue(row.GetCell(j).ToString(), out index) && row.GetCell(j) != null)
                    {
                        value = rows.GetCell(j).ToString();
                        type = (properties[index].PropertyType).FullName;
                        if (type == "System.String")
                        {
                            properties[index].SetValue(obj, value, null);
                        }
                        else if (type == "System.DateTime")
                        {
                            DateTime dt = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                            properties[index].SetValue(obj, dt, null);

                        }
                        else if (type == "System.Boolean")
                        {
                            bool pb = Convert.ToBoolean(value);
                            properties[index].SetValue(obj, pb, null);
                        }
                        else if (type == "System.Int16")
                        {
                            short pi16 = Convert.ToInt16(value);
                            properties[index].SetValue(obj, pi16, null);
                        }
                        else if (type == "System.Int32")
                        {
                            int pi32 = Convert.ToInt32(value);
                            properties[index].SetValue(obj, pi32, null);
                        }
                        else if (type == "System.Int64")
                        {
                            long pi64 = Convert.ToInt64(value);
                            properties[index].SetValue(obj, pi64, null);
                        }
                        else if (type == "System.Byte")
                        {
                            byte pb = Convert.ToByte(value);
                            properties[index].SetValue(obj, pb, null);
                        }
                        else
                        {
                            properties[index].SetValue(obj, null, null);
                        }
                        index++;
                    }
                }
                list.Add(obj);
            }
            return list;
        }
    }
}
