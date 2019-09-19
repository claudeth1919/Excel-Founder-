using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOM.Tool;

namespace BOM
{
    partial class GetMaterialView
    {
        private MyTextBox OrderIdInput;
        
        private void InitializeComponentCustomize()
        {
            this.OrderIdInput = new MyTextBox(null);
            // 
            // OrderIdInput
            // 
            this.OrderIdInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.OrderIdInput.Location = new System.Drawing.Point(109, 15);
            this.OrderIdInput.Material = null;
            this.OrderIdInput.Name = "OrderIdInput";
            this.OrderIdInput.Size = new System.Drawing.Size(163, 21);
            this.OrderIdInput.TabIndex = 2;
            this.OrderIdInput.TextChanged += new System.EventHandler(this.TextChanged_OrderIdInput);
            this.Controls.Add(this.OrderIdInput);
        }
    }
}
