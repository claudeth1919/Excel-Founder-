using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;     
using BOM.Model;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using BOM.View;
using System.Windows.Forms;

namespace BOM.Tool
{
    public static class ExcelUtil
    {
        private static string CurrentExcelOpenPath = String.Empty;
        private const int MIN_COLUMNS_BOM_AMOUNT = 10;
        private const int MIN_COLUMNS_WH_AMOUNT = 5;
        private const int HEADER_COLUMN_TOLERANCE = 4;
        private const int EMPTINESS_ROW_TOLERANCE = 2;
        private const int NORMAL_COLUMN_AMOUNT = 22;

        public static List<MaterialBOM> ConvertExcelToMaterialListFromBOM(string filePath, RichTextBox processTextBox)
        {
            List<MaterialBOM> materialList = new List<MaterialBOM>();
            List<string> errorList = new List<string>();
            ShowChangesRichTextBox(processTextBox, "\nAbriendo Libro de Excel...");
            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook;
            try
            {
                 workbook = app.Workbooks.Open(filePath, UpdateLinks: 2);
            }
            catch (Exception e)
            {
                Util.ShowMessage(AlarmType.ERROR, e.Message);
                return null;
            }
            
            
            foreach (Excel._Worksheet sheet in workbook.Sheets)
            {
                if (sheet.Visible == Excel.XlSheetVisibility.xlSheetHidden) continue;
                Excel.Range range = sheet.UsedRange;
                int rowCount = range.Rows.Count;
                int colCount = range.Columns.Count;
                List<Column> headercolumns = new List<Column>(colCount);
                string sheetName = sheet.Name.ToUpper();
                if (sheetName == Defs.EXCLUDED_SHEET_COSTOS|| sheetName == Defs.EXCLUDED_SHEET_LEYENDA)
                {
                    continue;
                }
                int rowToleranceIndex = 0;
                for (int rowIndex = 1; rowIndex <= rowCount; rowIndex++)
                {
                    MaterialBOM material = new MaterialBOM();

                    if (headercolumns.Count < MIN_COLUMNS_BOM_AMOUNT) //Get Columns names
                    {
                        colCount = colCount > NORMAL_COLUMN_AMOUNT ? NORMAL_COLUMN_AMOUNT  : colCount;
                        for (int colIndex = 1; colIndex <= colCount; colIndex++)
                        {
                            try
                            {
                                if (range.Cells[rowIndex, colIndex] != null && range.Cells[rowIndex, colIndex].Value2 != null)
                                {
                                    string columnName = (string)range.Cells[rowIndex, colIndex].Value2.ToString();
                                    Column column = new Column(columnName, colIndex);
                                    headercolumns.Add(column);
                                }
                            }
                            catch (Exception e)
                            {
                                break;
                            }
                        }
                        if (rowIndex == HEADER_COLUMN_TOLERANCE) break;
                    }
                    else
                    {
                        string errorItem = String.Empty;
                        string materialMessageItem = String.Empty;
                        Column colMaterialCode = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_MATERIAL_CODE));
                        Column colAmount = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_AMOUNT));
                        Column colStatus = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_STATUS));
                        Column colUnit = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_UNIT));
                        if (colMaterialCode == null)
                        {
                            errorItem = $"\n*ERROR: Formato no reconocido en la hoja {sheet.Name.ToString()}, la columna del número de material, debe de decir: \"TEXTO BREVE Clave de material del proveedor\"";
                            errorList.Add(errorItem);
                            ShowChangesRichTextBox(processTextBox, errorItem);
                            break;
                        }
                        if (colAmount == null)
                        {
                            errorItem = $"\n*ERROR: Formato no reconocido en la hoja {sheet.Name.ToString()}, la columna de la Cantidad del producto debe de decir: \"Cantidad\"";
                            errorList.Add(errorItem);
                            ShowChangesRichTextBox(processTextBox, errorItem);
                            break;
                        }
                        if (errorList.Count > 0)
                        {
                            break;
                        }
                        var dynamicCode = range.Cells[rowIndex, colMaterialCode.Index].Value;
                        var dynamicAmount = range.Cells[rowIndex, colAmount.Index].Value;
                        var dynamicStatus = range.Cells[rowIndex, colStatus.Index].Value;
                        var dynamicUnit = range.Cells[rowIndex, colUnit.Index].Value;

                        string status = Util.ConvertDynamicToString(dynamicStatus);

                        if (!Util.IsEmptyString(status)&&status.ToUpper()!= Defs.STATE_PENDING)
                        {
                            continue;
                        }

                        string materialCode = Util.ConvertDynamicToString(dynamicCode);
                        if (Util.IsEmptyString(materialCode))
                        {
                            rowToleranceIndex++;
                            if (EMPTINESS_ROW_TOLERANCE < rowToleranceIndex) break;
                        }
                        material.Code = Util.NormalizeString(materialCode);
                        material.Unit = Util.ConvertDynamicToString(dynamicUnit);
                        
                        material.Amount = Util.ConvertDynamicToDouble(dynamicAmount);
                        material.Sheet = sheet.Name;
                        material.Row = rowIndex;
                        //Add Valid material to list
                        if (!Util.IsEmptyString(material.Code) && material.Amount != 0)
                        {
                            if (Util.IsEmptyString(material.Unit))
                            {
                                material.Unit = "(Sin Unidad)";
                            }
                            materialList.Add(material);
                            materialMessageItem = $"\nPROCESANDO: Hoja: {sheet.Name} Num Parte: {material.Code} Cantidad: {material.Amount} Unidad {material.Unit}";
                            ShowChangesRichTextBox(processTextBox, materialMessageItem);
                        }
                        else if (!Util.IsEmptyString(material.Code))
                        {
                            errorItem = $"No hay información en la columna de {Defs.COL_AMOUNT} en la fila {rowIndex} de la hoja {sheet.Name.ToString()}";
                            errorList.Add(errorItem);
                        }

                    }
                }
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(sheet);
            }
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();


            //close and release
            workbook.Close(0);
            Marshal.ReleaseComObject(workbook);

            //quit and release
            app.Quit();
            Marshal.ReleaseComObject(app);

            if (errorList.Count>0)
            {
                materialList.Clear();
                Util.ShowMessage(AlarmType.ERROR, errorList);
            }

            return materialList;
        }


        //Begin

        public static List<MaterialStuck> ConvertExcelToMaterialListFromAvila(string filePath)
        {
            List<MaterialStuck> materialList = new List<MaterialStuck>();
            List<string> errorList = new List<string>();
            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook;
            try
            {
                workbook = app.Workbooks.Open(filePath, UpdateLinks: 2);
            }
            catch (Exception e)
            {
                Util.ShowMessage(AlarmType.ERROR, e.Message);
                return null;
            }


            foreach (Excel._Worksheet sheet in workbook.Sheets)
            {
                if (sheet.Visible == Excel.XlSheetVisibility.xlSheetHidden) continue;
                Excel.Range range = sheet.UsedRange;
                int rowCount = range.Rows.Count;
                int colCount = range.Columns.Count;
                List<Column> headercolumns = new List<Column>(colCount);
                string sheetName = sheet.Name.ToUpper();
                
                int rowToleranceIndex = 0;
                for (int rowIndex = 1; rowIndex <= rowCount; rowIndex++)
                {
                    MaterialStuck material = new MaterialStuck();

                    if (headercolumns.Count < 3) //Get Columns names
                    {
                        colCount = 4;
                        for (int colIndex = 1; colIndex <= colCount; colIndex++)
                        {
                            try
                            {
                                if (range.Cells[rowIndex, colIndex] != null && range.Cells[rowIndex, colIndex].Value2 != null)
                                {
                                    string columnName = (string)range.Cells[rowIndex, colIndex].Value2.ToString();
                                    Column column = new Column(columnName, colIndex);
                                    headercolumns.Add(column);
                                }
                            }
                            catch (Exception e)
                            {
                                break;
                            }
                        }
                        if (rowIndex == HEADER_COLUMN_TOLERANCE) break;
                    }
                    else
                    {
                        string errorItem = String.Empty;
                        string materialMessageItem = String.Empty;
                        Column colMaterialCode = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_MATERIAL_ORDERN_AVILA));
                        Column colAmount = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_PZAS_AVILA));
                        Column colMarca = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_MARCA_AVILA));
                        Column colDescription = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_DESCRIPCION_AVILA));
                        if (colMaterialCode == null)
                        {
                            errorItem = $"\n*ERROR: Formato no reconocido en la hoja {sheet.Name.ToString()}, la columna del número de material, debe de decir: \"TEXTO BREVE Clave de material del proveedor\"";
                            errorList.Add(errorItem);
                            break;
                        }
                        
                        var dynamicCode = range.Cells[rowIndex, colMaterialCode.Index].Value;
                        var dynamicAmount = range.Cells[rowIndex, colAmount.Index].Value;
                        var dynamicMarca = range.Cells[rowIndex, colMarca.Index].Value;
                        var dynamicDescription = range.Cells[rowIndex, colDescription.Index].Value;

                        

                        

                        string materialCode = Util.ConvertDynamicToString(dynamicCode)=="?" ? String.Empty : Util.ConvertDynamicToString(dynamicCode);
                        if (Util.IsEmptyString(materialCode))
                        {
                            rowToleranceIndex++;
                            if (EMPTINESS_ROW_TOLERANCE < rowToleranceIndex) break;
                        }
                        material.Code = Util.NormalizeString(materialCode);
                        material.OriginalCode = materialCode;
                        material.Description = Util.ConvertDynamicToString(dynamicDescription);

                        material.Total = Util.ConvertDynamicToDouble(dynamicAmount);
                        material.Provider = Util.ConvertDynamicToString(dynamicMarca) == "?" ? "" : Util.ConvertDynamicToString(dynamicMarca);
                        material.Ktl = sheet.Name;
                        //Add Valid material to list
                        if (!Util.IsEmptyString(material.Code))
                        {
                            if (Util.IsEmptyString(material.Unit))
                            {
                                material.Unit = "Pieza";
                            }
                            materialList.Add(material);
                            materialMessageItem = $"\nPROCESANDO: Hoja: {sheet.Name} Num Parte: {material.Code} Cantidad: {material.Total} Unidad {material.Unit}";
                        }
                        else if (!Util.IsEmptyString(material.Code))
                        {
                            errorItem = $"No hay información en la columna de {Defs.COL_AMOUNT} en la fila {rowIndex} de la hoja {sheet.Name.ToString()}";
                            errorList.Add(errorItem);
                        }

                    }
                }
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(sheet);
            }
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();


            //close and release
            workbook.Close(0);
            Marshal.ReleaseComObject(workbook);

            //quit and release
            app.Quit();
            Marshal.ReleaseComObject(app);

            if (errorList.Count > 0)
            {
                materialList.Clear();
                Util.ShowMessage(AlarmType.ERROR, errorList);
            }

            return materialList;
        }



        public static List<MaterialStuck> ConvertExcelToMaterialListFromMemo(string filePath)
        {
            List<MaterialStuck> materialList = new List<MaterialStuck>();
            List<string> errorList = new List<string>();
            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook;
            try
            {
                workbook = app.Workbooks.Open(filePath, UpdateLinks: 2);
            }
            catch (Exception e)
            {
                Util.ShowMessage(AlarmType.ERROR, e.Message);
                return null;
            }


            foreach (Excel._Worksheet sheet in workbook.Sheets)
            {
                if (sheet.Visible == Excel.XlSheetVisibility.xlSheetHidden) continue;
                Excel.Range range = sheet.UsedRange;
                int rowCount = range.Rows.Count;
                int colCount = range.Columns.Count;
                List<Column> headercolumns = new List<Column>(colCount);
                string sheetName = sheet.Name.ToUpper();

                int rowToleranceIndex = 0;
                for (int rowIndex = 1; rowIndex <= rowCount; rowIndex++)
                {
                    MaterialStuck material = new MaterialStuck();

                    if (headercolumns.Count < 4) //Get Columns names
                    {
                        colCount = 4;
                        for (int colIndex = 1; colIndex <= colCount; colIndex++)
                        {
                            try
                            {
                                if (range.Cells[rowIndex, colIndex] != null && range.Cells[rowIndex, colIndex].Value2 != null)
                                {
                                    string columnName = (string)range.Cells[rowIndex, colIndex].Value2.ToString();
                                    Column column = new Column(columnName, colIndex);
                                    headercolumns.Add(column);
                                }
                            }
                            catch (Exception e)
                            {
                                break;
                            }
                        }
                        if (rowIndex == HEADER_COLUMN_TOLERANCE) break;
                    }
                    else
                    {
                        string errorItem = String.Empty;
                        string materialMessageItem = String.Empty;
                        Column colMaterialCode = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_MATERIAL_ORDERN_MEMO));
                        Column colAmount = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_PZAS_MEMO));
                        Column colMarca = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_MARCA_MEMO));
                        Column colDescription = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_DESCRIPCION_MEMO));
                        if (colMaterialCode == null)
                        {
                            errorItem = $"\n*ERROR: Formato no reconocido en la hoja {sheet.Name.ToString()}, la columna del número de material, debe de decir: \"TEXTO BREVE Clave de material del proveedor\"";
                            errorList.Add(errorItem);
                            break;
                        }

                        var dynamicCode = range.Cells[rowIndex, colMaterialCode.Index].Value;
                        var dynamicAmount = range.Cells[rowIndex, colAmount.Index].Value;
                        var dynamicMarca = range.Cells[rowIndex, colMarca.Index].Value;
                        var dynamicDescription = range.Cells[rowIndex, colDescription.Index].Value;





                        string materialCode = Util.ConvertDynamicToString(dynamicCode) == "?" ? String.Empty : Util.ConvertDynamicToString(dynamicCode);
                        if (Util.IsEmptyString(materialCode))
                        {
                            rowToleranceIndex++;
                            if (EMPTINESS_ROW_TOLERANCE < rowToleranceIndex) break;
                        }
                        material.Code = Util.NormalizeString(materialCode);
                        material.OriginalCode = materialCode;
                        material.Description = Util.ConvertDynamicToString(dynamicDescription);

                        material.Total = Util.ConvertDynamicToDouble(dynamicAmount);
                        material.Provider = Util.ConvertDynamicToString(dynamicMarca) == "?" ? "" : Util.ConvertDynamicToString(dynamicMarca);
                        material.Ktl = sheet.Name;
                        //Add Valid material to list
                        if (!Util.IsEmptyString(material.Code))
                        {
                            if (Util.IsEmptyString(material.Unit))
                            {
                                material.Unit = "Unidad";
                            }
                            materialList.Add(material);
                            materialMessageItem = $"\nPROCESANDO: Hoja: {sheet.Name} Num Parte: {material.Code} Cantidad: {material.Total} Unidad {material.Unit}";
                        }
                        else if (!Util.IsEmptyString(material.Code))
                        {
                            errorItem = $"No hay información en la columna de {Defs.COL_AMOUNT} en la fila {rowIndex} de la hoja {sheet.Name.ToString()}";
                            errorList.Add(errorItem);
                        }

                    }
                }
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(sheet);
            }
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();


            //close and release
            workbook.Close(0);
            Marshal.ReleaseComObject(workbook);

            //quit and release
            app.Quit();
            Marshal.ReleaseComObject(app);

            if (errorList.Count > 0)
            {
                materialList.Clear();
                Util.ShowMessage(AlarmType.ERROR, errorList);
            }

            return materialList;
        }


        //End


        private static void ShowChangesRichTextBox(RichTextBox processTextBox, string messageItem)
        {
            Font font = new Font("Arial Narrow", 13, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            processTextBox.Font = font;
            if(!Util.IsEmptyString(messageItem)) processTextBox.AppendText($"\n {messageItem}");
            processTextBox.SelectionStart = processTextBox.Text.Length;
            // scroll it automatically
            processTextBox.ScrollToCaret();
        }

        public static void DownloadDataGrid(DataGridView data)
        {
            data.SelectAll();
            DataObject dataObj = data.GetClipboardContent();
            if (dataObj != null) Clipboard.SetDataObject(dataObj);
            Excel.Application xlexcel;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Excel.Application();
            
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            //
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Name = "Inventario";
            Excel.Range headerRange = (Excel.Range)xlWorkSheet.Cells[1, 2];
            int colIndex = 1;

            foreach (DataGridViewColumn col in data.Columns)
            {
                if (col.Visible)
                {
                    headerRange.Cells[1, colIndex].Value = col.Name;
                    colIndex++;
                }
            }

            Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[2, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

            Excel.Range range = xlWorkSheet.UsedRange;
            FormatAsTable(range, "Table", "TableStyleMedium15");
            range.Cells.WrapText = true;
            range.Cells.HorizontalAlignment =  Excel.XlHAlign.xlHAlignCenter;
            range.Cells.VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;
            xlexcel.Visible = true;
        }


        public static void FormatAsTable(Excel.Range SourceRange, string TableName, string TableStyleName)
        {
            SourceRange.Worksheet.ListObjects.Add(Excel.XlListObjectSourceType.xlSrcRange,
            SourceRange, System.Type.Missing, Excel.XlYesNoGuess.xlYes, System.Type.Missing).Name =
                TableName;
            SourceRange.Select();
            SourceRange.Worksheet.ListObjects[TableName].TableStyle = TableStyleName;
        }

        public static void UpdateBOMExcelFile(string filePath, List<MaterialOrder> materialList)
        {
            string copyFilePath = filePath.ToLower().Replace(".xlsx", Defs.EXCEL_FILE_POSTFIX);
            

            bool retry = true;
            while (retry)
            {
                try
                {
                    Util.DeleteFileIfExist(copyFilePath);
                    File.Copy(filePath, copyFilePath, true);
                    retry = false;
                }
                catch (Exception exc)
                {
                    if (!(MessageBox.Show($"Verifique que el archivo no este siendo usado por otro usuario ({copyFilePath})", "Archivo siento utilizado",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
                    {
                        retry = false;
                    }
                }
            }
            
            CurrentExcelOpenPath = copyFilePath;

            Excel.Application app = new Excel.Application();
            app.WorkbookBeforeClose += CloseUpdatedExcel;
            Excel.Workbook workbook = app.Workbooks.Open(copyFilePath, UpdateLinks: 3);
            List<string> errorList = new List<string>();
            foreach (Excel._Worksheet sheet in workbook.Sheets)
            {
                if (sheet.Visible == Excel.XlSheetVisibility.xlSheetHidden) continue;
                Excel.Range range = sheet.UsedRange;
                int rowCount = range.Rows.Count;
                int colCount = range.Columns.Count;
                List<Column> headercolumns = new List<Column>(colCount);
                string sheetName = sheet.Name.ToUpper();
                if (sheetName == Defs.EXCLUDED_SHEET_COSTOS || sheetName == Defs.EXCLUDED_SHEET_LEYENDA)
                {
                    continue;
                }
                Excel.Range rows = range.Rows;
                int rowToleranceIndex = 0;
                for (int rowIndex = 1; rowIndex <= rowCount; rowIndex++)
                {
                    if (headercolumns.Count < MIN_COLUMNS_BOM_AMOUNT) //Get Columns names
                    {
                        colCount = colCount > NORMAL_COLUMN_AMOUNT ? NORMAL_COLUMN_AMOUNT : colCount; 
                        for (int colIndex = 1; colIndex <= colCount; colIndex++)
                        {
                            if (range.Cells[rowIndex, colIndex] != null && range.Cells[rowIndex, colIndex].Value2 != null)
                            {
                                var colNameTemp = range.Cells[rowIndex, colIndex].Value2.ToString();
                                string columnName = Util.ConvertDynamicToString(colNameTemp);
                                Column column = new Column(columnName, colIndex);
                                headercolumns.Add(column);
                            }
                        }
                        if (rowIndex == HEADER_COLUMN_TOLERANCE) break;
                    }
                    else
                    {
                        Column colMaterialCode = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_MATERIAL_CODE));
                        Column colAmount = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_AMOUNT));

                        if (colMaterialCode == null)
                        {
                            MessageBox.Show($"Formato no reconocido en {sheetName} en la columna de: Clave del material");
                            break;
                        }

                        var dynamicCode = range.Cells[rowIndex, colMaterialCode.Index].Value;
                        var dynamicAmount = range.Cells[rowIndex, colAmount.Index].Value;

                        string materialCode = Util.ConvertDynamicToString(dynamicCode);
                        materialCode = Util.NormalizeString(materialCode);
                        if (Util.IsEmptyString(materialCode))
                        {
                            rowToleranceIndex++;
                            if (EMPTINESS_ROW_TOLERANCE < rowToleranceIndex) break;
                        }
                        MaterialOrder material = materialList.Find(item => item.Code == materialCode);
                        if (material == null) continue;
                        double desiredAmount = Util.ConvertDynamicToDouble(dynamicAmount); ;

                        //Add Valid material to list
                        if (!Util.IsEmptyString(materialCode) && desiredAmount != 0)
                        {
                            Column colStatus = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_STATUS));
                            Column colUnit = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_UNIT));
                            DeleteOverRowFromExcel(rows);

                            double leftAmount = Math.Abs(desiredAmount - material.ChosenAmount);

                            int currentRowIndex = rowIndex;
                            int previousRowIndex = currentRowIndex - 1;
                            int nextRowIndex = currentRowIndex + 1;

                            //Add row upon
                            try
                            {
                                rows[currentRowIndex].Insert(Excel.XlInsertShiftDirection.xlShiftDown, rows[currentRowIndex]);
                            }
                            catch(Exception e)
                            {
                                errorList.Add($"En la hoja: {sheetName} hubo un problema, posiblemente quedó incompleta, por favor revisarla");
                                workbook.Save();
                                continue;
                            }
                            
                            Excel.Range currentLine = (Excel.Range)rows[nextRowIndex];
                            Excel.Range newLine = (Excel.Range)rows[currentRowIndex];
                            currentLine.Copy(newLine);
                            range.Cells[currentRowIndex, colStatus.Index].Value = Defs.STATE_ORIGINAL;

                            currentRowIndex++;
                            previousRowIndex = currentRowIndex - 1;
                            nextRowIndex = currentRowIndex + 1;

                            if (desiredAmount > material.ChosenAmount)
                            {
                                range.Cells[currentRowIndex, colAmount.Index].Value = material.ChosenAmount;


                                Excel.Range currentLineTemp = (Excel.Range)rows[currentRowIndex];
                                //Add row below 
                                try
                                {
                                    rows[nextRowIndex].Insert(Excel.XlInsertShiftDirection.xlShiftDown, rows[currentRowIndex]);
                                }
                                catch (Exception e)
                                {
                                    errorList.Add($"En la hoja: {sheetName} hubo un problema, posiblemente quedó incompleta, por favor revisarla");
                                    workbook.Save();
                                    continue;
                                }
                                

                                Excel.Range newLineTemp = (Excel.Range)rows[nextRowIndex];
                                currentLineTemp.Copy(newLineTemp);


                                range.Cells[nextRowIndex, colAmount.Index].Value = leftAmount;
                                range.Cells[nextRowIndex, colStatus.Index].Value = Defs.STATE_PENDING;
                                rowIndex++;
                            }
                            else
                            {
                                range.Cells[currentRowIndex, colAmount.Index].Value = material.ChosenAmount;
                            }
                            rows[currentRowIndex].Interior.Color = Color.Green;
                            range.Cells[currentRowIndex, colStatus.Index].Value = Defs.STATE_RESERVED;
                            range.Cells[currentRowIndex, colUnit.Index].Interior.Color = Color.Red;
                            rows[previousRowIndex].Interior.Color = Color.White;
                            range.Cells[currentRowIndex, colUnit.Index].Value = material.Unit;

                            rowIndex++;
                            materialList.Remove(material);
                        }

                    }
                }
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(sheet);
            }

            if (errorList.Count != 0)
            {
                Util.ShowMessage(AlarmType.WARNING, errorList);
            }
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();


            //close and release
            workbook.Save();
            app.Visible = true;

            string excelName = workbook.Name;
            bool condition = !("Sheet1" == excelName);
            if (condition)
            {
                EmailConfirmation windows = new EmailConfirmation(CurrentExcelOpenPath);
                windows.Show();
            }

            Marshal.ReleaseComObject(workbook);

            //quit and release
            //app.Quit();
            Marshal.ReleaseComObject(app);
        }

        private static void DeleteOverRowFromExcel(Excel.Range range)
        {
            int rowCount = range.Rows.Count;
            if (rowCount > 1000000)
            {
                for (int index = (rowCount-15); rowCount > index;index++)
                {
                    range.Rows[index].Delete();
                }
            }
        }

        private static void CloseUpdatedExcel(Excel.Workbook Wb, ref bool Cancel)
        {
            bool condition = !("Sheet1" == Wb.Name);
            if (condition)
            {
                Wb.Save();
                Wb.Close();
            }
        }
        

        public static List<MaterialStuck> ConvertExcelToMaterialListFromWarehouseExcel(string filePath)
        {
            List<MaterialStuck> materialList = new List<MaterialStuck>();
            List<string> errorList = new List<string>();
            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook = app.Workbooks.Open(filePath);

            foreach (Excel._Worksheet sheet in workbook.Sheets)
            {
                Excel.Range range = sheet.UsedRange;
                int rowCount = range.Rows.Count;
                int colCount = range.Columns.Count;
                List<Column> headercolumns = new List<Column>(colCount);
                string sheetName = sheet.Name.ToUpper();
                if (sheetName == Defs.EXCLUDED_SHEET_COSTOS || sheetName == Defs.EXCLUDED_SHEET_LEYENDA)
                {
                    continue;
                }
                
                for (int rowIndex = 1; rowIndex <= rowCount; rowIndex++)
                {
                    MaterialStuck material = new MaterialStuck();

                    if (headercolumns.Count < MIN_COLUMNS_BOM_AMOUNT) //Get Columns names
                    {
                        for (int colIndex = 1; colIndex <= colCount; colIndex++)
                        {
                            if (range.Cells[rowIndex, colIndex] != null && range.Cells[rowIndex, colIndex].Value2 != null)
                            {
                                var colNameTemp = range.Cells[rowIndex, colIndex].Value2.ToString();
                                string columnName = Util.ConvertDynamicToString(colNameTemp);
                                Column column = new Column(columnName, colIndex);
                                headercolumns.Add(column);
                            }
                        }
                        if (rowIndex == HEADER_COLUMN_TOLERANCE) break;
                    }
                    else
                    {
                        Column colWaterhouseId = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_WATERHOUSE_IDS));
                        Column colAmount = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_TOTAL));
                        Column colUnit = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_UNIT));
                        Column colLocation = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_LOCATION));
                        Column colProvider = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_PROVIDER));
                        Column colKlt = headercolumns.Find(col => Util.IsLike(col.Name, Defs.COL_KLT));

                        if (colWaterhouseId == null)
                        {
                            MessageBox.Show($"Formato no reconocido en {sheetName}");
                            break;
                        }

                        var dynamicCode = range.Cells[rowIndex, colWaterhouseId.Index].Value;
                        var dynamicAmount = range.Cells[rowIndex, colAmount.Index].Value;
                        var dynamicUnit = range.Cells[rowIndex, colUnit.Index].Value;
                        var dynamicLocation = range.Cells[rowIndex, colLocation.Index].Value;
                        var dynamicProvider = range.Cells[rowIndex, colProvider.Index].Value;
                        var dynamicKlt = range.Cells[rowIndex, colKlt.Index].Value;

                        string materialCode = Util.ConvertDynamicToString(dynamicCode);
                        material.Code = Util.NormalizeString(materialCode);
                        material.OriginalCode = materialCode;

                        material.Total = Util.ConvertDynamicToDouble(dynamicAmount);
                        material.Unit = Util.ConvertDynamicToString(dynamicUnit);
                        material.Location = Util.ConvertDynamicToString(dynamicLocation);
                        material.Provider = Util.ConvertDynamicToString(dynamicProvider);
                        material.Ktl = Util.ConvertDynamicToString(dynamicKlt);

                        //Add Valid material to list
                        if (!Util.IsEmptyString(material.Code) )
                        {
                            materialList.Add(material);
                        }

                    }
                }

            }


            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();


            //close and release
            workbook.Close(0);
            Marshal.ReleaseComObject(workbook);
            //quit and release
            app.Quit();
            Marshal.ReleaseComObject(app);
            return materialList;
        }


        public static List<MaterialStuck> ConvertExcelToMaterialListFromLetfOvers(string filePath)
        {
            List<MaterialStuck> materialList = new List<MaterialStuck>();
            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook = app.Workbooks.Open(filePath);
            foreach (Excel._Worksheet sheet in workbook.Sheets)
            {
                Excel.Range range = sheet.UsedRange;
                int rowCount = range.Rows.Count;
                int colCount = range.Columns.Count;
                if (sheet.Name.ToUpper() == Defs.EXCLUDED_SHEET_COSTOS || sheet.Name.ToUpper() == Defs.EXCLUDED_SHEET_COLORES)
                {
                    continue;
                }
                for (int rowIndex = 2; rowIndex <= rowCount; rowIndex++)
                {
                    MaterialStuck material = new MaterialStuck();
                    for (int colIndex = 1; colIndex <= 2; colIndex++)
                    {
                        int colorNumber = System.Convert.ToInt32(((Excel.Range)sheet.Cells[rowIndex, colIndex]).Interior.Color);
                        Color color = System.Drawing.ColorTranslator.FromOle(colorNumber);
                        
                        if (!Util.IsLike(color.Name, Defs.COLOR_GREEN))
                        {
                            var value = range.Cells[rowIndex, colIndex].Value2;
                            string code = (string)(value + "");
                            break;
                        }
                        if (range.Cells[rowIndex, colIndex] != null && range.Cells[rowIndex, colIndex].Value2 != null)
                        {
                            var value = range.Cells[rowIndex, colIndex].Value2;
                            if (colIndex == Defs.NUM_COL_MATERIAL_CODE)
                            {
                                string code = (string)(value+"");
                                material.Code = Util.NormalizeString(code);
                                material.OriginalCode = code;
                            }
                            else if(colIndex == Defs.NUM_COL_AMOUNT)
                            {
                                double amount;
                                try
                                {
                                    amount = (double)value;
                                }
                                catch (Exception e)
                                {
                                    amount = 0;
                                }
                                material.Total = amount;
                            }
                            if (rowIndex != 1 && material.Code != null && material.Total != 0)
                            {
                                material.Location = sheet.Name;
                                materialList.Add(material);
                                break;
                            }
                        }
                    }

                }
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(sheet);
            }
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();


            //close and release
            workbook.Close(0);
            Marshal.ReleaseComObject(workbook);
            //quit and release
            app.Quit();
            Marshal.ReleaseComObject(app);
            return materialList;
        }

    }
}
