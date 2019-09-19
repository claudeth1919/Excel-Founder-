namespace BOM.View
{
    partial class MaterialGrid
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
            this.DataGrid = new System.Windows.Forms.DataGridView();
            this.Save = new System.Windows.Forms.Button();
            this.DownloadBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.Input = new System.Windows.Forms.TextBox();
            this.SelectInput = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGrid
            // 
            this.DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid.Location = new System.Drawing.Point(27, 71);
            this.DataGrid.Name = "DataGrid";
            this.DataGrid.Size = new System.Drawing.Size(1064, 419);
            this.DataGrid.TabIndex = 0;
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(325, 507);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 1;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Click_Save);
            // 
            // DownloadBtn
            // 
            this.DownloadBtn.Location = new System.Drawing.Point(503, 507);
            this.DownloadBtn.Name = "DownloadBtn";
            this.DownloadBtn.Size = new System.Drawing.Size(75, 23);
            this.DownloadBtn.TabIndex = 2;
            this.DownloadBtn.Text = "Download";
            this.DownloadBtn.UseVisualStyleBackColor = true;
            this.DownloadBtn.Click += new System.EventHandler(this.Click_Download);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(694, 507);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.Click_Cancel);
            // 
            // Input
            // 
            this.Input.Location = new System.Drawing.Point(325, 24);
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(285, 20);
            this.Input.TabIndex = 4;
            this.Input.TextChanged += new System.EventHandler(this.TextChanged_Input);
            // 
            // SelectInput
            // 
            this.SelectInput.FormattingEnabled = true;
            this.SelectInput.Location = new System.Drawing.Point(637, 23);
            this.SelectInput.Name = "SelectInput";
            this.SelectInput.Size = new System.Drawing.Size(121, 21);
            this.SelectInput.TabIndex = 5;
            this.SelectInput.ValueMemberChanged += new System.EventHandler(this.ChangedValue_Select);
            this.SelectInput.TextChanged += new System.EventHandler(this.ChangedValue_Select);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.label1.Location = new System.Drawing.Point(202, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Buscador";
            // 
            // MaterialGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 542);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectInput);
            this.Controls.Add(this.Input);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.DownloadBtn);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.DataGrid);
            this.Name = "MaterialGrid";
            this.Text = "MaterialGrid";
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DataGrid;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button DownloadBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.TextBox Input;
        private System.Windows.Forms.ComboBox SelectInput;
        private System.Windows.Forms.Label label1;
    }
}