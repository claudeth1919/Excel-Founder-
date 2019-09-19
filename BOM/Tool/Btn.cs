using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BOM.Model;
using BOM.View;

namespace BOM.Tool
{
    class Btn : Button
    {
        private Order order;
        private int index;

        public Order Order
        {
            get { return order; }
            set { order = value; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        //protected override void OnClick(EventArgs e)
        //{
        //    base.OnClick(e);
        //    AssignOrder window = new AssignOrder(order);
        //    window.Show();
        //}
    }
}
