using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using BOM.Model;
using BOM.Tool;
using BOM.DataAccess;

namespace BOM
{
    public partial class WarehouseDropperView : DropperView
    {
        public WarehouseDropperView()
        {
            InitializeComponent();
            this.Text = "Warehouse";
        }
        
        public override void DropFileAction(string filesPathItem)
        {
            //List<MaterialStuck> materialList = ExcelUtil.ConvertExcelToMaterialListFromAvila(filesPathItem);
            List<MaterialStuck> materialList = ExcelUtil.ConvertExcelToMaterialListFromMemo(filesPathItem);
            //List<MaterialStuck> materialList = ExcelUtil.ConvertExcelToMaterialListFromWarehouseExcel(filesPathItem);
            DataDB.InsertMaterialListAvila(materialList);
            if (materialList.Count != 0)
            {
                MessageBox.Show($"Se insertaron {materialList.Count} materiales en almacén");
            }
            else
            {
                MessageBox.Show($"No se inserto ningún registro cheque el formato");
            }
        }

        public override void UploadFileAction(string filesPathItem)
        {
            List<MaterialStuck> materialList = ExcelUtil.ConvertExcelToMaterialListFromLetfOvers(filesPathItem);
            DataDB.InsertMaterialList(materialList);
            if (materialList.Count != 0)
            {
                MessageBox.Show($"Se insertaron o actualizaron {materialList.Count} materiales en almacén");
            }
            else
            {
                MessageBox.Show("No se inserto ningún registro, cheque el formato");
            }
            
        }
    }
}
