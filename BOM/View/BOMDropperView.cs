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
using System.Threading;
using BOM.DataAccess;

namespace BOM
{
    public partial class BOMDropperView : DropperView
    {
        public BOMDropperView()
        {
            InitializeComponent();
            this.Text = "Drop BOM";
            this.ProcessTextBox.Visible = false;
            this.ProcessTextBox.Enabled = false;
            this.BtnCancel.Visible = false;
        }
        
        public override void DropFileAction(string filesPathItem)
        {
            //Thread thread = new Thread(() => CreatePreOrder(filesPathItem));
            //thread.Name = "DropFileActionThread";
            //thread.Start();
            CreatePreOrder(filesPathItem);
        }
        
        public override void UploadFileAction(string filesPathItem)
        {
            CreatePreOrder(filesPathItem);
        }
        private void CreatePreOrder(string filesPathItem)
        {
            if (DataDB.IsExcelAlreadyOrdered(filesPathItem))
            {
                Util.ShowMessage(AlarmType.WARNING, $"El archivo cargado ya ha sido ordenado anteriormente ({filesPathItem})");
                return;
            }
            this.ProcessTextBox.Visible = true;
            List<MaterialBOM> materialList = ExcelUtil.ConvertExcelToMaterialListFromBOM(filesPathItem, this.ProcessTextBox);
            this.ProcessTextBox.Visible = false;
            this.ProcessTextBox.Text = string.Empty;
            if (materialList==null) return;
            if (materialList.Count == 0) return;
            Order receiveOrder = DataDB.InsertBOMMaterialList(materialList, filesPathItem);
            List<MaterialOrder> preorderList = receiveOrder.MaterialList;
            if (preorderList.Count!=0)
            {
                PDFUtil.CreatePreOrder(receiveOrder);
                Util.ShowMessage(AlarmType.SUCCESS, $"Se encontraron {preorderList.Count} materiales en almacén");
            }
            else
            {
                MessageBox.Show("No hay ningún producto disponible en almacén");
            }
            
        }

        private void TextChanged_PathLink(object sender, EventArgs e)
        {
            ImageBox.Hide();
            this.LoadingImage.Show();
            string link = this.LinkInput.Text;
            if (!Util.IsEmptyString(link))
            {
                string newPath = Util.GetNetworkPath(link, "O:", "SMB-Mexiko");
                CreatePreOrder(newPath);
                this.LinkInput.Text = String.Empty;
            }
            this.ImageBox.Show();
            this.LoadingImage.Hide();
        }
    }
}
