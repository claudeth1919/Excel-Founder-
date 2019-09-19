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

namespace BOM
{
    public partial class GetMaterialView : Form
    {

        List<Order> ordersList;
        List<Order> shownOrdersList;
        private Order lastAssginedOrder;
        public GetMaterialView()
        {
            InitializeComponent();
            InitializeComponentCustomize();
            lastAssginedOrder = DataDB.GetLastAssignedOrder();
            BtnLastEmail.Text +=$"({lastAssginedOrder.Id})";
            BtnLastExcel.Text += $"({lastAssginedOrder.Id})";
            EmptyOrderLabel.Hide();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            Panel.Width = this.Width-40;
            Panel.HorizontalScroll.Maximum = 0;
            Panel.AutoScroll = false;
            Panel.VerticalScroll.Visible = true;
            Panel.AutoScroll = true;
            Panel.WrapContents = false;

            ordersList =  DataDB.GetPendingOrders();
            
            OrderIdInput.Focus();
            this.ActiveControl = OrderIdInput;
            shownOrdersList = Util.CloneOrderList(ordersList);
            SetItemList();
        }

        private void SetItemList()
        {
            if (shownOrdersList.Count == 0)
            {
                EmptyOrderLabel.Show();
            }
            else
            {
                EmptyOrderLabel.Hide();
            }
            int index = 0;
            foreach (Order order in shownOrdersList)
            {
                string orderId = order.Id.ToString();
                FlowLayoutPanel flowItem = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight
                };
                Btn btnByProduct = new Btn
                {
                    Width = 50,
                    Height = 35,
                    Order = order,
                    Text = "Asignar"
                };
                Btn btnDeleteOrder = new Btn
                {
                    Width = 50,
                    Height = 35,
                    Order = order,
                    Text = "Delete",
                    Index = index
                };
                Btn btnPrint = new Btn
                {
                    Width = 50,
                    Height = 35,
                    Order = order,
                    Text = "Print",
                    Index = index
                };
                btnDeleteOrder.Click += BtnDeleteOrder_Click;   
                btnByProduct.Click += BtnByProduct_Click;
                btnPrint.Click += BtnPrint_Click;
                Label info = new Label
                {
                    Text = $"Order N° {orderId}",
                    Width = 80,
                    Height = 40,
                    Font = new System.Drawing.Font("Arial Narrow", 12, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))
                };

                flowItem.Height = 40;
                flowItem.Width = Panel.Width;
                flowItem.Name = orderId;
                flowItem.Controls.Add(info);
                flowItem.Controls.Add(btnByProduct);
                flowItem.Controls.Add(btnDeleteOrder);
                flowItem.Controls.Add(btnPrint);
                Panel.Controls.Add(flowItem);
                index++;
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Btn btn = (Btn)sender;
            Order order = btn.Order;
            string path = $@"{Util.CURRENT_PATH}\PreOrder_{order.Id}.pdf";
            if (Util.ExistFile(path))
            {
                System.Diagnostics.Process.Start(path);
            }
            else
            {
                order = DataDB.GetPreOrderById(order);
                PDFUtil.CreatePreOrder(order);
            }
        }

        private void BtnDeleteOrder_Click(object sender, EventArgs e)
        {
            Btn btn = (Btn)sender;
            Order order = btn.Order;
            
            if ((MessageBox.Show($"¿Realmente Deseas eliminar la Order {order.Id}?", "Borrar Orden",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
            {
                DataDB.DeleteOrderById(order.Id);
                string orderId = order.Id.ToString();
                Panel.Controls.RemoveAt(btn.Index);
            }
        }

        private void BtnComplete_Click(object sender, EventArgs e)
        {
            Btn btn = (Btn)sender;
            Order order = btn.Order;
        }

        private void BtnByProduct_Click(object sender, EventArgs e)
        {
            Btn btn = (Btn)sender;
            Order order = btn.Order;
            AssignOrder window = new AssignOrder(order);
            window.Show();
            this.Close();
        }

        private void Click_Cancel(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextChanged_OrderIdInput(object sender, EventArgs e)
        {
            string orderIdString = OrderIdInput.Text;
            Panel.Controls.Clear();
            shownOrdersList.Clear();
            foreach (Order order in ordersList)
            {
                string itemId = order.Id + "";
                if (itemId.IndexOf(orderIdString) !=-1)
                {
                    shownOrdersList.Add(order);
                }
            }
            SetItemList();
        }

        private void Click_EmailLastAssignedOrder(object sender, EventArgs e)
        {
            string lastExcelOpenPathAssignedOrder = lastAssginedOrder.ExcelPath.Replace(".xlsx", Defs.EXCEL_FILE_POSTFIX);
            EmailConfirmation windows = new EmailConfirmation(lastExcelOpenPathAssignedOrder);
            windows.Show();
        }

        private void Click_RedoLastExcelAssignedOrder(object sender, EventArgs e)
        {
            ExcelUtil.UpdateBOMExcelFile(lastAssginedOrder.ExcelPath, lastAssginedOrder.MaterialList);
        }

        private void Click_ShowAssignedOrderView(object sender, EventArgs e)
        {
            AllAssignedOrdes oldOrders = new AllAssignedOrdes();
            oldOrders.Show();
        }
    }
}
