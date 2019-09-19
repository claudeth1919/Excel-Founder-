using System;
using System.Drawing;
using System.Windows.Forms;
using BOM.Model;
namespace BOM.Tool
{
    class NumberTextBox : TextBox
    {
        private MaterialOrder material;

        public NumberTextBox()
        {
        }
        protected override void OnTextChanged(System.EventArgs args)
        {
            base.OnTextChanged(args);
            double number;
            string value = this.Text;
            if (double.TryParse(value, out number))
            {
            }
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                base.OnKeyPress(e);
            }
        }
        public MaterialOrder Material
        {
            get { return material; }
            set { material = value; }
        }
    }
}
