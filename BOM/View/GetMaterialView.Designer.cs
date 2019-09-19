using BOM.Tool;
namespace BOM
{
    partial class GetMaterialView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Panel = new System.Windows.Forms.FlowLayoutPanel();
            this.InputLabel = new System.Windows.Forms.Label();
            this.EmptyOrderLabel = new System.Windows.Forms.Label();
            this.BtnLastEmail = new System.Windows.Forms.Button();
            this.BtnLastExcel = new System.Windows.Forms.Button();
            this.AssignedOrdersBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Panel
            // 
            this.Panel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Panel.Location = new System.Drawing.Point(12, 66);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(226, 324);
            this.Panel.TabIndex = 0;
            // 
            // InputLabel
            // 
            this.InputLabel.AutoSize = true;
            this.InputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.InputLabel.Location = new System.Drawing.Point(12, 15);
            this.InputLabel.Name = "InputLabel";
            this.InputLabel.Size = new System.Drawing.Size(93, 17);
            this.InputLabel.TabIndex = 3;
            this.InputLabel.Text = "Buscar Order";
            // 
            // EmptyOrderLabel
            // 
            this.EmptyOrderLabel.AutoSize = true;
            this.EmptyOrderLabel.Location = new System.Drawing.Point(98, 50);
            this.EmptyOrderLabel.Name = "EmptyOrderLabel";
            this.EmptyOrderLabel.Size = new System.Drawing.Size(140, 13);
            this.EmptyOrderLabel.TabIndex = 4;
            this.EmptyOrderLabel.Text = "No hay Ordenes Pendientes";
            // 
            // BtnLastEmail
            // 
            this.BtnLastEmail.Location = new System.Drawing.Point(80, 422);
            this.BtnLastEmail.Name = "BtnLastEmail";
            this.BtnLastEmail.Size = new System.Drawing.Size(141, 23);
            this.BtnLastEmail.TabIndex = 5;
            this.BtnLastEmail.Text = "Email Last Order ";
            this.BtnLastEmail.UseVisualStyleBackColor = true;
            this.BtnLastEmail.Click += new System.EventHandler(this.Click_EmailLastAssignedOrder);
            // 
            // BtnLastExcel
            // 
            this.BtnLastExcel.Location = new System.Drawing.Point(82, 448);
            this.BtnLastExcel.Name = "BtnLastExcel";
            this.BtnLastExcel.Size = new System.Drawing.Size(137, 23);
            this.BtnLastExcel.TabIndex = 6;
            this.BtnLastExcel.Text = "Redo Excel Order ";
            this.BtnLastExcel.UseVisualStyleBackColor = true;
            this.BtnLastExcel.Click += new System.EventHandler(this.Click_RedoLastExcelAssignedOrder);
            // 
            // AssignedOrdersBtn
            // 
            this.AssignedOrdersBtn.Location = new System.Drawing.Point(80, 396);
            this.AssignedOrdersBtn.Name = "AssignedOrdersBtn";
            this.AssignedOrdersBtn.Size = new System.Drawing.Size(141, 23);
            this.AssignedOrdersBtn.TabIndex = 7;
            this.AssignedOrdersBtn.Text = "Assigned Orders";
            this.AssignedOrdersBtn.UseVisualStyleBackColor = true;
            this.AssignedOrdersBtn.Click += new System.EventHandler(this.Click_ShowAssignedOrderView);
            // 
            // GetMaterialView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 479);
            this.Controls.Add(this.AssignedOrdersBtn);
            this.Controls.Add(this.BtnLastExcel);
            this.Controls.Add(this.BtnLastEmail);
            this.Controls.Add(this.EmptyOrderLabel);
            this.Controls.Add(this.InputLabel);
            this.Controls.Add(this.Panel);
            this.Name = "GetMaterialView";
            this.Text = "Tomar Material";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel Panel;
        
        private System.Windows.Forms.Label InputLabel;
        private System.Windows.Forms.Label EmptyOrderLabel;
        private System.Windows.Forms.Button BtnLastEmail;
        private System.Windows.Forms.Button BtnLastExcel;
        private System.Windows.Forms.Button AssignedOrdersBtn;
    }
}