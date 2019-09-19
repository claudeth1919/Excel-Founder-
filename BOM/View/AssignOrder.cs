using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BOM.Model;
using BOM.DataAccess;
using BOM.Tool;
using System.IO;

namespace BOM.View
{
    public partial class AssignOrder : Form
    {
        List<MaterialOrder> materialList;
        private Order order;
        public AssignOrder(Order order)
        {
            InitializeComponent();
            this.order = order;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            Panel.HorizontalScroll.Maximum = 0;
            Panel.AutoScroll = false;
            Panel.VerticalScroll.Visible = false;
            Panel.AutoScroll = true;
            Panel.WrapContents = false;
            Panel.Width = this.Width-40;
            this.Tittle.Text += order.Id;
            materialList = DataDB.GetMaterialByOrderId(order.Id);
            SetItemList();
        }

        private void SetItemList()
        {
            Font boldFont = new System.Drawing.Font("Arial Narrow", 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Font standarFont = new System.Drawing.Font("Arial Narrow", 12, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            int heightSpace = 40;
            FlowLayoutPanel flowItemHeader = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Height = 40,
                Width = Panel.Width
            };

            int padingSpace = (int) Panel.Width / 10;
            Label CodeHeader = new Label
            {
                Text = $"Code",
                Width = padingSpace-10,
                Height = heightSpace,
                Font = boldFont
            };

            Label WantedAmountHeader = new Label
            {
                Text = $"Desire Amount",
                Width = padingSpace-10,
                Height = heightSpace,
                Font = boldFont
            };

            Label location = new Label
            {
                Text = $"Location",
                Width = padingSpace - 10,
                Height = heightSpace,
                Font = boldFont
            };

            Label kltLabel = new Label
            {
                Text = $"KLT",
                Width = padingSpace - 10,
                Height = heightSpace,
                Font = boldFont
            };

            Label bomUnitLabel = new Label
            {
                Text = $"BOM unit",
                Width = padingSpace - 10,
                Height = heightSpace,
                Font = boldFont
            };

            Label warehouseUnitLabel = new Label
            {
                Text = $"Warehouse unit",
                Width = padingSpace - 10,
                Height = heightSpace,
                Font = boldFont
            };

            Label stockAmountLabel = new Label
            {
                Text = $"Stock Amount",
                Width = padingSpace - 10,
                Height = heightSpace,
                Font = boldFont
            };

            Label originLabel = new Label
            {
                Text = $"Origin",
                Width = padingSpace - 10,
                Height = heightSpace,
                Font = boldFont
            };

            Label availableAmountHeader = new Label
            {
                Text = $"ChosenAmount",
                Width = padingSpace,
                Height = heightSpace,
                Font = boldFont
            };

            Label selectedHeader = new Label
            {
                Text = $"",
                Width = padingSpace,
                Height = heightSpace,
                Font = boldFont
            };

            flowItemHeader.Controls.Add(CodeHeader);
            flowItemHeader.Controls.Add(kltLabel);
            flowItemHeader.Controls.Add(location);
            
            flowItemHeader.Controls.Add(WantedAmountHeader);
            flowItemHeader.Controls.Add(stockAmountLabel);
            flowItemHeader.Controls.Add(originLabel);
            flowItemHeader.Controls.Add(bomUnitLabel);
            flowItemHeader.Controls.Add(warehouseUnitLabel);
            flowItemHeader.Controls.Add(availableAmountHeader);
            flowItemHeader.Controls.Add(selectedHeader);

            Panel.Controls.Add(flowItemHeader);

            foreach (MaterialOrder material in materialList)
            {
                FlowLayoutPanel flowItem = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    Height = heightSpace,
                    Width = Panel.Width
                };

                Label code = new Label
                {
                    Text = $"{material.OriginalCode}",
                    Width = padingSpace,
                    Height = heightSpace,
                    Font = standarFont
                };

                Label locationItem = new Label
                {
                    Text = $"{material.Location}",
                    Width = padingSpace,
                    Height = heightSpace,
                    Font = standarFont
                };

                Label kltItem = new Label
                {
                    Text = $"{material.Ktl}",
                    Width = padingSpace,
                    Height = heightSpace,
                    Font = standarFont
                };

                Label wantedAmount = new Label
                {
                    Text = $"{material.WantedAmount}",
                    Width = padingSpace,
                    Height = heightSpace,
                    Font = standarFont
                };

                Label stockAmount = new Label
                {
                    Text = $"{material.StockTotal}",
                    Width = padingSpace,
                    Height = heightSpace,
                    Font = standarFont
                };

                Label bomUnit = new Label
                {
                    Text = $"{material.BOMUnit}",
                    Width = padingSpace,
                    Height = heightSpace,
                    Font = standarFont
                };

                Label warehouseUnit = new Label
                {
                    Text = $"{material.WarehouseUnit}",
                    Width = padingSpace,
                    Height = heightSpace,
                    Font = standarFont
                };
                Label origin = new Label
                {
                    Text = $"{material.OriginName}",
                    Width = padingSpace,
                    Height = heightSpace,
                    Font = standarFont
                };


                MyTextBox input = new MyTextBox(material);
                input.Text = material.AvailableAmount+"";
                //input.Width = padingSpace;
                MyCheckBox checkbox = new MyCheckBox(material);
                //checkbox.Width = padingSpace;
                

                flowItem.Controls.Add(code);
                flowItem.Controls.Add(kltItem);
                flowItem.Controls.Add(locationItem);
                
                flowItem.Controls.Add(wantedAmount);
                flowItem.Controls.Add(stockAmount);
                flowItem.Controls.Add(origin);

                flowItem.Controls.Add(bomUnit);
                flowItem.Controls.Add(warehouseUnit);

                flowItem.Controls.Add(input);
                flowItem.Controls.Add(checkbox);
                Panel.Controls.Add(flowItem);
            }
        }

        private void Click_Cancel(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Click_AssignOrder(object sender, EventArgs e)
        {
            List<MaterialOrder> selectedMaterial = new List<MaterialOrder>();
            List<string> errorList = new List<string>();
            foreach (MaterialOrder material in materialList)
            {
                if (material.IsSelected)
                {
                    if (material.StockTotal>=material.ChosenAmount)
                    {
                        selectedMaterial.Add(material);
                    }
                    else
                    {
                        errorList.Add($"El material {material.OriginalCode} no puede tener más que la cantidad en almacén\n");
                    }
                }
            }
            if (errorList.Count!=0)
            {
                Util.ShowMessage(AlarmType.WARNING, errorList);
                return;
            }
            Order newOrder = DataDB.UpdateAssignOrder(order.Id, selectedMaterial);
            List <MaterialOrder> materialOrderList = newOrder.MaterialList;
            this.Close();
            Util.ShowMessage(AlarmType.SUCCESS, "Se asignó con éxito, espere un momento mientras se abre el excel");
            ExcelUtil.UpdateBOMExcelFile(newOrder.ExcelPath, materialOrderList);
            
        }
    }
}
