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
using BOM.Model;
using BOM.Tool;
using BOM.View;

namespace BOM.View
{
    public partial class AllAssignedOrdes : Form
    {
        List<Order> shownOrdersList;
        List<Order> ordersList;
        public AllAssignedOrdes()
        {
            InitializeComponent();
            Panel.Width = this.Width - 40;
            Panel.HorizontalScroll.Maximum = 0;
            Panel.AutoScroll = false;
            Panel.VerticalScroll.Visible = true;
            Panel.AutoScroll = true;
            Panel.WrapContents = false;

            ordersList = DataDB.GetassignedOrders();
            shownOrdersList = Util.CloneOrderList(ordersList);
            SetItemList();
            Input.Focus();
            this.ActiveControl = Input;
        }
        private void SetItemList()
        {
            int index = 0;
            foreach (Order order in shownOrdersList)
            {
                string orderId = order.Id.ToString();
                FlowLayoutPanel flowItem = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight
                };
                Btn btnPrint = new Btn
                {
                    Width = 50,
                    Height = 35,
                    Order = order,
                    Text = "Print",
                    Index = index
                };
                btnPrint.Click += BtnPrint_Click;
                Label info = new Label
                {
                    Text = $"{PDFUtil.GetProyectName(order.ExcelPath)}",
                    Width = Panel.Width-100,
                    Height = 40,
                    Font = new System.Drawing.Font("Arial Narrow", 12, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))
                };

                flowItem.Height = 40;
                flowItem.Width = Panel.Width;
                flowItem.Name = orderId;
                flowItem.Controls.Add(info);
                flowItem.Controls.Add(btnPrint);
                Panel.Controls.Add(flowItem);
                index++;
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Btn btn = (Btn)sender;
            Order order = btn.Order;
            string path = $@"{Util.CURRENT_PATH}\Order_{order.Id}.pdf";
            if (Util.ExistFile(path))
            {
                System.Diagnostics.Process.Start(path);
            }
            else
            {
                order = DataDB.GetOrderById(order.Id);
                PDFUtil.CreateAssignedOrderreport(order);
            }
        }

        private void TextChanged_OrderIdInput(object sender, EventArgs e)
        {
            string orderIdString = Input.Text;
            Panel.Controls.Clear();
            shownOrdersList.Clear();
            foreach (Order order in ordersList)
            {
                string itemId = order.ExcelPath + "";
                if (Util.IsLike(itemId, orderIdString))
                {
                    shownOrdersList.Add(order);
                }
            }
            SetItemList();
        }
    }
}
