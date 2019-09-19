using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOM.Model
{
    public class Order
    {
        private ulong id;
        private bool isAssigned;
        private List<MaterialOrder> materialList;
        private string excelPath;

        public ulong Id
        {
            get { return id; }
            set { id = value; }
        }
        public bool IsAssigned
        {
            get { return isAssigned; }
            set { isAssigned = value; }
        }
        public string ExcelPath
        {
            get { return excelPath; }
            set { excelPath = value; }
        }
        public List<MaterialOrder> MaterialList
        {
            get { return materialList; }
            set { materialList = value; }
        }
    }
}
