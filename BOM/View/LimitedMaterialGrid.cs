using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BOM.DataAccess;
using BOM.Tool;
using BOM.Model;

namespace BOM.View
{
    public partial class LimitedMaterialGrid : Form
    {
        private List<MaterialStuck> DeletedMaterialList;
        public LimitedMaterialGrid()
        {
            InitializeComponent();
            
            FillDataGrid();
            DeletedMaterialList = new List<MaterialStuck>();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            ///Changing
            this.SelectInput.Items.Add("OriginalCode");
            this.SelectInput.Items.Add("Location");
            this.SelectInput.Items.Add("KLT");
            this.SelectInput.Items.Add("Description");
            this.SelectInput.DropDownStyle = ComboBoxStyle.DropDownList;
            this.SelectInput.SelectedIndex= 0;
            FillDataGrid();
            Input.Focus();
            this.ActiveControl = Input;
        }

        private void FillDataGrid()
        {
            var dataSet = DataDB.GetMaterial(); 
            DataGrid.DataSource = dataSet.Tables[0];
            DataGrid.Columns["Id"].Visible = false;
            DataGrid.Columns["Code"].Visible = false;
            DataGrid.Columns["IsActive"].Visible = false;
            DataGrid.Columns["Origin_FK"].Visible = false;

            DataGrid.AllowUserToResizeColumns = true;
            DataGrid.Columns["OriginalCode"].HeaderText = "Code";
            DataGrid.Columns["OriginalCode"].Width = 200;

            DataGrid.Columns["StockTotal"].HeaderText = "Total";
            DataGrid.Columns["StockTotal"].Width = 50;
            DataGrid.DataError += DataGrid_DataError;

            DataGrid.Columns["Unit"].Width = 75;


            //DataGrid.Columns["OriginalCode"].ReadOnly = true;
            //DataGrid.Columns["Description"].ReadOnly = true;
            //DataGrid.Columns["ProviderName"].ReadOnly = true;
            //DataGrid.Columns["StockTotal"].ReadOnly = true;
            //DataGrid.Columns["Unit"].ReadOnly = true;
            //DataGrid.Columns["KLT"].ReadOnly = true;
            //DataGrid.Columns["Location"].ReadOnly = true;

            DataGrid.AllowUserToDeleteRows = false;
            DataGrid.AllowUserToAddRows = false;
            DataGrid.Columns["KLT"].Width = 150;

            DataGrid.UserDeletingRow += DataGrid_UserDeletingRow;
        }

        private void DataGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DataGridViewRow row = e.Row;
            MaterialStuck material = new MaterialStuck
            {
                Id = (Guid)row.Cells["Id"].Value,
                OriginalCode = Util.ConvertDynamicToString(row.Cells["OriginalCode"].Value),
                Name = Util.ConvertDynamicToString(row.Cells["Name"].Value),
                Description = Util.ConvertDynamicToString(row.Cells["Description"].Value),
                Provider = Util.ConvertDynamicToString(row.Cells["ProviderName"].Value),
                Total = Util.ConvertDynamicToDouble(row.Cells["StockTotal"].Value),
                Unit = Util.ConvertDynamicToString(row.Cells["Unit"].Value),
                Location = Util.ConvertDynamicToString(row.Cells["Location"].Value),
                Ktl = Util.ConvertDynamicToString(row.Cells["KLT"].Value),
                IsActive = false
            };
            MaterialStuck tempMaterial = DeletedMaterialList.Find(item=> item.Id== material.Id);
            if (!DeletedMaterialList.Contains(tempMaterial)) DeletedMaterialList.Add(material);
        }

        
        

        private void DataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DataGridView data = (DataGridView)sender;
            DataGridViewCell cell = data.CurrentCell;

            Util.ShowMessage(AlarmType.WARNING, $"Verifique que el tipo de dato sea el correcto: {cell.EditedFormattedValue} no es un {cell.ValueType.Name}");
        }
        
        
        private bool IsAlreadyInsideList(List<MaterialStuck> list, MaterialStuck material)
        {
            MaterialStuck tempMaterial = list.Find(item => item.Id == material.Id);
            if (!list.Contains(tempMaterial)) return false;
            return true;
        }
        private void Click_Download(object sender, EventArgs e)
        {
            Input.Text = "";
            ExcelUtil.DownloadDataGrid(DataGrid);
        }

        private void Click_Cancel(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextChanged_Input(object sender, EventArgs e)
        {
            string filterby = (string) SelectInput.SelectedItem;
            (DataGrid.DataSource as DataTable).DefaultView.RowFilter = string.Format(filterby + " LIKE '%{0}%'", Input.Text);
        }

        private void ChangedValue_Select(object sender, EventArgs e)
        {
            string filterby = (string)SelectInput.SelectedItem;
            (DataGrid.DataSource as DataTable).DefaultView.RowFilter = string.Format(filterby + " LIKE '%{0}%'", Input.Text);
        }
        
    }
}
