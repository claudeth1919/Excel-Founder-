namespace BOM.View
{
    partial class AllAssignedOrdes
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
            this.Input = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Panel
            // 
            this.Panel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Panel.Location = new System.Drawing.Point(27, 53);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(763, 324);
            this.Panel.TabIndex = 1;
            // 
            // Input
            // 
            this.Input.Location = new System.Drawing.Point(214, 15);
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(515, 20);
            this.Input.TabIndex = 2;
            this.Input.TextChanged += new System.EventHandler(this.TextChanged_OrderIdInput);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Buscar por nombre";
            // 
            // AllAssignedOrdes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 389);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Input);
            this.Controls.Add(this.Panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AllAssignedOrdes";
            this.Text = "Ordenes Cerradas";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel Panel;
        private System.Windows.Forms.TextBox Input;
        private System.Windows.Forms.Label label1;
    }
}