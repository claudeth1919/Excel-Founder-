namespace BOM.View
{
    partial class AssignOrder
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
            this.Tittle = new System.Windows.Forms.Label();
            this.Panel = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnAssign = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Tittle
            // 
            this.Tittle.AutoSize = true;
            this.Tittle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.Tittle.Location = new System.Drawing.Point(53, 38);
            this.Tittle.Name = "Tittle";
            this.Tittle.Size = new System.Drawing.Size(165, 31);
            this.Tittle.TabIndex = 0;
            this.Tittle.Text = "N° de Order ";
            // 
            // Panel
            // 
            this.Panel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Panel.Location = new System.Drawing.Point(12, 88);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(1342, 510);
            this.Panel.TabIndex = 1;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(748, 604);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.Click_Cancel);
            // 
            // BtnAssign
            // 
            this.BtnAssign.Location = new System.Drawing.Point(525, 603);
            this.BtnAssign.Name = "BtnAssign";
            this.BtnAssign.Size = new System.Drawing.Size(75, 23);
            this.BtnAssign.TabIndex = 3;
            this.BtnAssign.Text = "Asignar";
            this.BtnAssign.UseVisualStyleBackColor = true;
            this.BtnAssign.Click += new System.EventHandler(this.Click_AssignOrder);
            // 
            // AssignOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 638);
            this.Controls.Add(this.BtnAssign);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.Panel);
            this.Controls.Add(this.Tittle);
            this.Name = "AssignOrder";
            this.Text = "AssignOrder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Tittle;
        private System.Windows.Forms.FlowLayoutPanel Panel;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnAssign;
    }
}