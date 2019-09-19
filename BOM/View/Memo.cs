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
    public partial class Memo : Form
    {
        private List<MaterialStuck> DeletedMaterialList;
        public Memo()
        {
            InitializeComponent();
            FillDataGrid();
            DeletedMaterialList = new List<MaterialStuck>();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            string originalCode = "OriginalCode";
            this.SelectInput.Items.Add(originalCode);
            this.SelectInput.Items.Add("Location");
            this.SelectInput.Items.Add("KLT");
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

            DataGrid.AllowUserToResizeColumns = true;
            DataGrid.Columns["OriginalCode"].HeaderText = "Code";
            DataGrid.Columns["OriginalCode"].Width = 200;

            DataGrid.Columns["StockTotal"].HeaderText = "Total";
            DataGrid.Columns["StockTotal"].Width = 50;

            DataGrid.Columns["Unit"].Width = 75;

            DataGrid.Columns["KLT"].Width = 150;
            DataGrid.AllowUserToAddRows = false;

            DataGrid.AllowUserToDeleteRows = false;
            

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
