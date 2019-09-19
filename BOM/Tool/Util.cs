using BOM.Model;
using BOM.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BOM.Tool
{
    public static class Util
    {
        public static readonly string CURRENT_PATH = Path.GetDirectoryName(Application.ExecutablePath);
        public static List<MyMessageBox> MyMessages = new List<MyMessageBox>();
        public static MyMessageBox lastMessage;


        public static bool IsLike(string completeString, string conteinedString)
        {
            completeString = NormalizeString(completeString);
            conteinedString = NormalizeString(conteinedString);
            if (completeString.IndexOf(conteinedString)!=-1)
            {
                return true;
            }
            return false;
        }
        
        public static void DeleteFileIfExist(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (System.IO.IOException ex)
                {
                    return;
                }
            }
        }

        public static bool ExistFile(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            return false;
        }

        public static double ConvertDynamicToDouble(dynamic dynamicNumber)
        {
           

            double amount;
            try
            {
                amount = (double)dynamicNumber;
            }
            catch (Exception e)
            {
                amount = 0;
            }
            if(amount == 0)
            {
                string numberString;
                try
                {
                    numberString = (string)dynamicNumber;
                    amount = double.Parse(numberString);
                }
                catch (Exception e)
                {
                    amount = 0;
                }
            }
            return amount;
        }

        public static string ConvertDynamicToString(dynamic dynamicString)
        {
            string newString;
            try
            {
                newString = (string)(dynamicString + "");
            }
            catch (Exception e)
            {
                newString = String.Empty;
            }
            
            return newString;
        }

        public static List<Order> CloneOrderList(List<Order> orderList)
        {
            List<Order> cloneList = new List<Order>();
            foreach (Order item in orderList)
            {
                cloneList.Add(item);
            }
            return cloneList;
        }

        public static string NormalizeString(string chain)
        {
            string newString = RemoveDiacritics(chain);
            newString = RemoveSpecialCharacters(chain).ToUpper();
            return newString;
        }

        public static string NormalizeStringList(List<string> list)
        {
            string newString = String.Empty;
            foreach (string chain in list)
            {
                newString += NormalizeString(chain) + ' ';
            }
            return newString;
        }

        public static string GetStringListDummie(List<string> list)
        {
            string newString = String.Empty;
            foreach (string chain in list)
            {
                newString += chain + ' ';
            }
            return newString;
        }

        public static bool IsEmptyString(string chain)
        {
            if (chain == ""||chain==null)
            {
                return true;
            }
            return false;
        }

        public static bool IsEmptyGuid(Guid chain)
        {
            if (chain == null)
            {
                return true;
            }
            if (chain.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                return true;
            }
            return false;
        }

        public static bool IsEmail(string chain)
        {
            if (IsEmptyString(chain))
            {
                return false;
            }
            if (chain.IndexOf("@")!=-1&&chain.IndexOf(".")!=-1)
            {
                return true;
            }
            return false;
        }

        public static void ShowMessage(AlarmType alarm, List<string> messages)
        {
            MyMessageBox message = new MyMessageBox(alarm, messages);
            MyMessages.Add(message);
            message.Show();
            message.FormClosed += Message_FormClosed;
            lastMessage = message;
        }

        private static void Message_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        public static void CloseMessages()
        {
            foreach (MyMessageBox message in MyMessages)
            {
                MyMessages.Remove(message);
                message.Close();
            }
        }

        public static void CloseMessagesByType(AlarmType alarmType)
        {
            foreach (MyMessageBox message in MyMessages)
            {
                MyMessages.Remove(message);
                if (message.Type == alarmType) message.Close();
            }
        }

        public static bool StringToBool(string chain)
        {
            if (chain == "1" || chain.ToUpper() == "TRUE") return true;
            return false;
        }

        public static void ShowMessage(AlarmType alarm, string message)
        {
            string messageString = message;
            MyMessageBox myMessage = new MyMessageBox(alarm, new List<string>() { messageString });
            myMessage.Show();
            lastMessage = myMessage;
        }

        public static bool FindAndKillProcess(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.StartsWith(name))
                {
                    clsProcess.Kill();
                    return true;
                }
            }
            return false;
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static Dictionary<string, string> ConcatBOMMaterialList(string splitter, List<MaterialBOM> list )
        {
            Dictionary<string, string> concatedList = new Dictionary<string, string>();
            string concatedCodeList = String.Empty;
            string concatedAmountList = String.Empty;
            string concatedUnitList = String.Empty;
            string concatedsheetName = String.Empty;
            string concatedExcelRow = String.Empty;
            int index = 1;
            foreach (var item in list)
            {
                concatedCodeList += item.Code;
                concatedAmountList += item.Amount;
                concatedUnitList += item.Unit;
                concatedsheetName += item.Sheet;
                concatedExcelRow += item.Row;
                if (!(list.Count==index))
                {
                    concatedCodeList += splitter;
                    concatedAmountList += splitter;
                    concatedUnitList += splitter;
                    concatedsheetName += splitter;
                    concatedExcelRow += splitter;
                }
                index++;
            }
            concatedList[Defs.COL_MATERIAL_CODE] = concatedCodeList;
            concatedList[Defs.COL_AMOUNT] = concatedAmountList;
            concatedList[Defs.COL_UNIT] = concatedUnitList;
            concatedList[Defs.SHEET_NAME] = concatedsheetName;
            concatedList[Defs.EXCEL_ROW] = concatedExcelRow;

            return concatedList;
        }

        public static string GetNetworkPath(string uncPath, string initialString, string rootString)
        {
            try
            {
                // remove the "\\" from the UNC path and split the path
                string path = String.Empty;
                uncPath = uncPath.Replace(@"\\", "");
                string[] uncParts = uncPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                bool isRoot = false;
                foreach (string part in uncParts)
                {
                    if (isRoot|| part.ToUpper()==rootString.ToUpper())
                    {
                        path += $@"\{part}";
                        isRoot = true;
                    }
                }
                if (isRoot) path = initialString + path;
                return path;
            }
            catch (Exception ex)
            {
                return "[ERROR RESOLVING UNC PATH: " + uncPath + ": " + ex.Message + "]";
            }
        }
        
        public static Dictionary<string, string> ConcatMaterialList(string splitter, List<MaterialOrder> list)
        {
            Dictionary<string, string> concatedList = new Dictionary<string, string>();
            string concatedWarehouseList = String.Empty;
            string concatedAmountList = String.Empty;
            int index = 1;
            foreach (var item in list)
            {
                concatedWarehouseList += item.WarehouseId;
                concatedAmountList += item.ChosenAmount;
                if (!(list.Count == index))
                {
                    concatedWarehouseList += splitter;
                    concatedAmountList += splitter;
                }
                index++;
            }
            concatedList[Defs.COL_WATERHOUSE_IDS] = concatedWarehouseList;
            concatedList[Defs.COL_TOTAL] = concatedAmountList;
            return concatedList;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
