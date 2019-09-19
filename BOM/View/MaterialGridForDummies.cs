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
    public partial class MaterialGridForDummies : Form
    {
        private List<MaterialStuck> DeletedMaterialList;
        public MaterialGridForDummies()
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

            DataGridCodes.ColumnCount = 1;
            DataGridCodes.Columns[0].Name = "Code";
            DataGridCodes.Columns[0].Width = 200;
            SupplierComboBox.DataSource = DataDB.GetSuppliers();
            UnitComboBox.DataSource = DataDB.GetUnits();
            UnitComboBox.SelectedIndex = 3;
            this.SupplierComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.UnitComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void FillDataGrid()
        {
            var dataSet = DataDB.GetMaterialFordummies(); 
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


            DataGrid.Columns["OriginalCode"].ReadOnly = true;
            DataGrid.Columns["Description"].ReadOnly = true;
            DataGrid.Columns["ProviderName"].ReadOnly = true;
            DataGrid.Columns["StockTotal"].ReadOnly = true;
            DataGrid.Columns["Unit"].ReadOnly = true;
            DataGrid.Columns["KLT"].ReadOnly = true;
            DataGrid.Columns["Location"].ReadOnly = true;

            DataGrid.AllowUserToDeleteRows = false;
            DataGrid.AllowUserToAddRows = false;
            

            DataGrid.Columns["KLT"].Width = 150;

            DataGrid.UserDeletingRow += DataGrid_UserDeletingRow;
            DataGridCodes.Rows.Clear();
            this.KLTInput.Text = String.Empty;
            this.LocationInput.Text = String.Empty;
            this.DescriptionInput.Text = String.Empty;
            this.NameInput.Text = String.Empty;
            this.AmountInput.Value = 1;
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
        

        private void Click_Save(object sender, EventArgs e)
        {
            if (!IsValidToSave()) return;
            string stringOriginalCode = String.Empty;
            string stringCode = String.Empty;

            foreach (DataGridViewRow row in DataGridCodes.Rows)
            {
                string stringTemp = Util.ConvertDynamicToString(row.Cells[0].Value);
                stringOriginalCode += stringTemp + " ";
                stringCode += Util.NormalizeString(stringTemp) + " ";
            }
            stringOriginalCode = stringOriginalCode.TrimEnd(' ');
            MaterialStuck materialStuck = new MaterialStuck()
            {
                OriginalCode = stringOriginalCode,
                Name = Util.NormalizeString(NameInput.Text),
                Code = stringCode,
                Ktl = Util.NormalizeString(KLTInput.Text),
                Description = DescriptionInput.Text,
                Unit = Util.ConvertDynamicToString(UnitComboBox.SelectedItem),
                Location = Util.NormalizeString(LocationInput.Text),
                ProviderName = Util.ConvertDynamicToString(SupplierComboBox.SelectedItem),
                Total = Util.ConvertDynamicToDouble(AmountInput.Value)
            };
            
            DataDB.UpdateMaterialListForDummies(materialStuck);
            Util.ShowMessage(AlarmType.SUCCESS, $"Se han insertado el registro");
            FillDataGrid();
        }
        private bool IsValidToSave()
        {
            List<string> errorList = new List<string>();
            bool isValid = true;
            if (DataGridCodes.Rows.Count == 0)
            {
                errorList.Add("No ha insertado el Número de parte del producto");
                isValid =  false;
            }else if (Util.IsEmptyString(Util.ConvertDynamicToString(DataGridCodes.Rows[0].Cells[0].Value)))
            {
                errorList.Add("No ha insertado el Número de parte del producto");
                isValid = false;
            }
            if (Util.IsEmptyString(LocationInput.Text))
            {
                errorList.Add("* Location es requerido");
                isValid = false;
            }
            string supplier = Util.ConvertDynamicToString(SupplierComboBox.SelectedItem);
            if (Util.IsEmptyString(supplier))
            {
                errorList.Add("* Supplier es requerido");
                isValid = false;
            }
            if(errorList.Count>0) Util.ShowMessage(AlarmType.WARNING, errorList);
            return isValid;
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

        private void Click_RefreshBtn(object sender, EventArgs e)
        {
            SupplierComboBox.DataSource = DataDB.GetSuppliers();
        }
    }
}
