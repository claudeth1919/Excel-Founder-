using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using BOM.View;
using BOM.Tool;
using BOM.Model;
using BOM.DataAccess;
using System.IO;

namespace BOM
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;



            //Order order = DataDB.GetOrderById(26);
            //ExcelUtil.UpdateBOMExcelFile(@"\\schaeffler.com\puebla\DATA\NL-PPM-O\Projects\CME\SMB-Mexiko\PROYECTOS\2019\S-023812_6080+ZS+MX+Turbine_Riveting_line_V12\11_proj-struct-purch\01_BOM\01_Conveyor\BOM_Electrica\Conveyor Turbina.xlsx", order.MaterialList);

            //MaterialGrid windows = new MaterialGrid();
            //windows.Show();
        }

        private void Click_OptionAddWarehouse(object sender, EventArgs e)
        {
            WarehouseDropperView window = new WarehouseDropperView();
            window.Show();
        }

        private void Click_OptionGetMaterial(object sender, EventArgs e)
        {
            GetMaterialView window = new GetMaterialView();
            window.Show();
        }

        private void Click_OptionCheckExistence(object sender, EventArgs e)
        {
            BOMDropperView window = new BOMDropperView();
            window.Show();
        }

        private void Click_Inventario(object sender, EventArgs e)
        {
            MaterialGrid window = new MaterialGrid();
            window.Show();
        }
    }
}
