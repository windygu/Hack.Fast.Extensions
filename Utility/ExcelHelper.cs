using System;
using System.Data;
using System.IO;
using System.Text;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using System.Collections.Generic;
using System.Reflection;

namespace Hack.Fast.Extensions.Utility
{
    public static class ExcelHelper
    {
        static HSSFWorkbook s_workbook;
        private static void InitializeWorkbook()
        {
            s_workbook = new HSSFWorkbook();
            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "福禄网络";
                s_workbook.DocumentSummaryInformation = dsi;
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "zhourui"; //填加xls文件作者信息
                si.ApplicationName = "QQ:236773862提供技术支持"; //填加xls文件创建程序信息
                si.LastAuthor = "福禄网络"; //填加xls文件最后保存者信息
                si.Comments = "说明信息"; //填加xls文件作者信息
                si.CreateDateTime = DateTime.Now;
                s_workbook.SummaryInformation = si;
            }
            #endregion


        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="ds">源DataTable</param>
        public static MemoryStream Export(DataSet ds)
        {
            InitializeWorkbook();
            HSSFCellStyle dateStyle = s_workbook.CreateCellStyle() as HSSFCellStyle;
            HSSFDataFormat format = s_workbook.CreateDataFormat() as HSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
            int index = 0;
            foreach (DataTable dtSource in ds.Tables)
            {
                HSSFSheet sheet = s_workbook.CreateSheet() as HSSFSheet;
                s_workbook.SetSheetName(index, dtSource.TableName);
                //取得列宽
                int[] arrColWidth = new int[dtSource.Columns.Count];
                foreach (DataColumn item in dtSource.Columns)
                {
                    arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                }
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    for (int j = 0; j < dtSource.Columns.Count; j++)
                    {
                        int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                        if (intTemp > arrColWidth[j])
                        {
                            arrColWidth[j] = intTemp;
                        }
                    }
                }
                int rowIndex = 0;
                foreach (DataRow row in dtSource.Rows)
                {
                    #region 新建表，填充表头，填充列头，样式
                    if (rowIndex == 65535 || rowIndex == 0)
                    {
                        if (rowIndex != 0)
                        {
                            sheet = s_workbook.CreateSheet() as HSSFSheet;
                        }
                        #region 表头及样式
                        {
                            HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                            headerRow.HeightInPoints = 25;

                            headerRow.CreateCell(0).SetCellValue(dtSource.TableName);
                            HSSFCellStyle headStyle = s_workbook.CreateCellStyle() as HSSFCellStyle;
                            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                            HSSFFont font = s_workbook.CreateFont() as HSSFFont;
                            font.FontHeightInPoints = 16;
                            font.Boldweight = 700;
                            headStyle.SetFont(font);
                            headerRow.GetCell(0).CellStyle = headStyle;
                            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                            //sheet.SetColumnWidth(headerRow.CreateCell(0).ColumnIndex,dtSource.TableName.Length);
                        }
                        #endregion

                        #region 列头及样式
                        {
                            HSSFRow headerRow = sheet.CreateRow(1) as HSSFRow;
                            HSSFCellStyle headStyle = s_workbook.CreateCellStyle() as HSSFCellStyle;
                            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                            HSSFFont font = s_workbook.CreateFont() as HSSFFont;
                            font.FontHeightInPoints = 10;
                            font.Boldweight = 700;
                            headStyle.IsLocked = true;
                            headStyle.SetFont(font);
                            foreach (DataColumn column in dtSource.Columns)
                            {
                                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                                headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                                //设置列宽
                                sheet.SetColumnWidth(column.Ordinal,
                                    ((arrColWidth[column.Ordinal] + 1) > 80 ? 80 : (arrColWidth[column.Ordinal] + 1)) * 256);
                            }
                            //sheet.CreateFreezePane(0, 2, 0, dtSource.Columns.Count - 1);
                        }
                        #endregion
                        rowIndex = 2;
                    }
                    #endregion

                    #region 填充内容
                    HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                    foreach (DataColumn column in dtSource.Columns)
                    {
                        HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;
                        string drValue = row[column].ToString();
                        switch (column.DataType.ToString())
                        {
                            case "System.String"://字符串类型
                                newCell.SetCellValue(drValue);
                                break;
                            case "System.DateTime"://日期类型
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                newCell.SetCellValue(dateV);

                                newCell.CellStyle = dateStyle;//格式化显示
                                break;
                            case "System.Boolean"://布尔型
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                newCell.SetCellValue(boolV);
                                break;
                            case "System.Int16"://整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                newCell.SetCellValue(intV);
                                break;
                            case "System.Decimal"://浮点型
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                newCell.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理
                                newCell.SetCellValue("");
                                break;
                            default:
                                newCell.SetCellValue("");
                                break;
                        }
                    }
                    #endregion
                    rowIndex++;
                }
                index++;
            }


            using (MemoryStream ms = new MemoryStream())
            {
                s_workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }

        public static DataTable ListToDataTable<T>(IList<T> list)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            Boolean columnHeader = true;
            foreach (T t in list)
            {
                Type ut = t.GetType();
                PropertyInfo[] plist = ut.GetProperties();
                dr = dt.NewRow();
                foreach (PropertyInfo p in plist)
                {
                    //填充列头
                    if (columnHeader)
                    {
                        if (!dt.Columns.Contains(p.Name))
                        {
                            dt.Columns.Add(p.Name);
                        }
                    }
                    Object value = ut.GetProperty(p.Name).GetValue(t, null);
                    dr[p.Name] = value == null ? "" : value.ToString();
                }
                dt.Rows.Add(dr);
                if (columnHeader)
                {
                    columnHeader = false;
                }
            }
            return dt;
        }
    }


    /// <summary>
    /// 为了解决下载中文名称 有空格，中文符号等问题
    /// 来源于网上开源代码
    /// </summary>
    public static class FileNameConverter
    {
        #region 必须调用这个方法解决文件名中有空格的问题
        /// <summary>
        /// 为字符串中的非英文字符编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToHexString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] chars = s.ToCharArray();
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < chars.Length; index++)
            {
                bool needToEncode = NeedToEncode(chars[index]);
                if (needToEncode)
                {
                    string encodedString = ToHexString(chars[index]);
                    builder.Append(encodedString);
                }
                else
                {
                    builder.Append(chars[index]);
                }
            }

            return builder.ToString();
        }
        /// <summary>
        /// 为非英文字符串编码
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static string ToHexString(char chr)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(chr.ToString());
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < encodedBytes.Length; index++)
            {
                builder.AppendFormat("%{0}", Convert.ToString(encodedBytes[index], 16));
            }
            return builder.ToString();
        }
        /// <summary>
        ///指定一个字符是否应该被编码
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static bool NeedToEncode(char chr)
        {
            string reservedChars = "$-_.+!*'(),@=&";

            if (chr > 127)
                return true;
            if (char.IsLetterOrDigit(chr) || reservedChars.IndexOf(chr) >= 0)
                return false;

            return true;
        }
        #endregion
    }
}
