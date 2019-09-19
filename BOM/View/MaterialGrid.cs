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
    public partial class MaterialGrid : Form
    {
        private List<MaterialStuck> DeletedMaterialList;
        public MaterialGrid()
        {
            InitializeComponent();
            FillDataGrid();
            DeletedMaterialList = new List<MaterialStuck>();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
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
            DataGrid.Columns["Name"].Visible = false;
            DataGrid.Columns["WhenInserted"].Visible = false;

            DataGrid.AllowUserToResizeColumns = true;
            DataGrid.Columns["OriginalCode"].HeaderText = "Code";
            DataGrid.Columns["OriginalCode"].Width = 200;

            DataGrid.Columns["Description"].Width = 200;

            DataGrid.Columns["StockTotal"].HeaderText = "Total";
            DataGrid.Columns["StockTotal"].Width = 50;
            DataGrid.DataError += DataGrid_DataError;

            DataGrid.Columns["Unit"].Width = 75;

            DataGrid.Columns["KLT"].Width = 150;

            DataGrid.UserDeletingRow += DataGrid_UserDeletingRow;
        }

        private void DataGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DataGridViewRow row = e.Row;
            try
            {
                MaterialStuck material = new MaterialStuck
                {
                    Id = (Guid)row.Cells["Id"].Value,
                    OriginalCode = Util.ConvertDynamicToString(row.Cells["OriginalCode"].Value),
                    //Name = Util.ConvertDynamicToString(row.Cells["Name"].Value),
                    Description = Util.ConvertDynamicToString(row.Cells["Description"].Value),
                    Provider = Util.ConvertDynamicToString(row.Cells["ProviderName"].Value),
                    Total = Util.ConvertDynamicToDouble(row.Cells["StockTotal"].Value),
                    Unit = Util.ConvertDynamicToString(row.Cells["Unit"].Value),
                    Location = Util.ConvertDynamicToString(row.Cells["Location"].Value),
                    Ktl = Util.ConvertDynamicToString(row.Cells["KLT"].Value),
                    IsActive = false
                };
                MaterialStuck tempMaterial = DeletedMaterialList.Find(item => item.Id == material.Id);
                //MaterialStuck tempMaterial = DeletedMaterialList.Find(item => (item.Id == material.Id && (Util.IsEmptyString(material.OriginalCode) ? true : item.OriginalCode == material.OriginalCode)));
                if (!DeletedMaterialList.Contains(tempMaterial)) DeletedMaterialList.Add(material);
            }
            catch (Exception ex)
            {
                //
            }
        }

        private void DataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (Util.lastMessage == null)
            {
                ShowFilledDataError(sender);
            }
            else if(Util.lastMessage.IsClosed)
            {
                ShowFilledDataError(sender);
            }
        }

        private void ShowFilledDataError(object sender)
        {
            DataGridView data = (DataGridView)sender;
            DataGridViewCell cell = data.CurrentCell;
            Util.ShowMessage(AlarmType.WARNING, $"Verifique que el tipo de dato sea el correcto: {cell.EditedFormattedValue} no es un {cell.ValueType.Name}");
        }
        

        private void Click_Save(object sender, EventArgs e)
        {
            List<MaterialStuck> materialList = new List<MaterialStuck>();
            DataTable dataTable = (DataTable)DataGrid.DataSource;
            DataTable dataTableChanges = dataTable.GetChanges(DataRowState.Modified);
            DataTable dataTableInserted = dataTable.GetChanges(DataRowState.Added);
            
            if (dataTableChanges == null&&DeletedMaterialList.Count==0&& dataTableInserted==null)
            {
                MessageBox.Show("No hay ningún cambio hecho");
                return;
            }
            materialList.AddRange(DeletedMaterialList);
            if (dataTableChanges != null)
            {
                foreach (System.Data.DataRow row in dataTableChanges.Rows)
                {
                    MaterialStuck material = null;
                    try
                    {
                        material = new MaterialStuck
                        {
                            Id = new Guid(row["Id"].ToString()),
                            OriginalCode = Util.ConvertDynamicToString(row["OriginalCode"]),
                            //Name = Util.ConvertDynamicToString(row["Name"]),
                            Description = Util.ConvertDynamicToString(row["Description"]),
                            Provider = Util.ConvertDynamicToString(row["ProviderName"]),
                            Total = Util.ConvertDynamicToDouble(row["StockTotal"]),
                            Unit = Util.ConvertDynamicToString(row["Unit"]),
                            Location = Util.ConvertDynamicToString(row["Location"]),
                            Ktl = Util.ConvertDynamicToString(row["KLT"]),
                            IsActive = Util.StringToBool(row["IsActive"].ToString())
                        };
                    }
                    catch 
                    {
                        Util.ShowMessage(AlarmType.WARNING, "No puede cambiar una fila recién insertada, salga de la ventana e intentelo de nuevo por favor");
                        continue;
                    }
                   
                    if (material.OriginalCode != String.Empty)
                    {
                        if(!IsAlreadyInsideList(materialList, material)) materialList.Add(material);
                    }
                    else
                    {
                        Util.ShowMessage(AlarmType.WARNING, "No puede dejar la clave (Code) de un producto vacio");
                        return;
                    }
                    materialList.Add(material);
                }
            }
            if (dataTableInserted != null)
            {
                foreach (System.Data.DataRow row in dataTableInserted.Rows)
                {
                    MaterialStuck material = new MaterialStuck
                    {
                        OriginalCode = Util.ConvertDynamicToString(row["OriginalCode"]),
                        //Name = Util.ConvertDynamicToString(row["Name"]),
                        Description = Util.ConvertDynamicToString(row["Description"]),
                        Provider = Util.ConvertDynamicToString(row["ProviderName"]),
                        Total = Util.ConvertDynamicToDouble(row["StockTotal"]),
                        Unit = Util.ConvertDynamicToString(row["Unit"]),
                        Location = Util.ConvertDynamicToString(row["Location"]),
                        Ktl = Util.ConvertDynamicToString(row["KLT"]),
                        IsActive = true
                    };
                    
                    if (material.OriginalCode!= String.Empty)
                    {
                        if (!IsAlreadyInsideList(materialList, material)) materialList.Add(material);
                    }
                    else
                    {
                        Util.ShowMessage(AlarmType.WARNING,"No puede dejar la clave (Code) de un producto vacio");
                        return;
                    }
                    
                    
                }
            }
            
            DataDB.UpdateMaterialList(materialList);
            Util.ShowMessage(AlarmType.SUCCESS, $"Se han cambiado los registro(s)");
            dataTable.AcceptChanges();
            //if (dataTableInserted != null)
            //{
            //    FillDataGrid();
            //    DataGrid.FirstDisplayedScrollingRowIndex = DataGrid.RowCount - 1;
            //}

        }
        private bool IsAlreadyInsideList(List<MaterialStuck> list, MaterialStuck material)
        {
            MaterialStuck tempMaterial = list.Find(item => (item.Id == material.Id && !Util.IsEmptyGuid(material.Id)));
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
