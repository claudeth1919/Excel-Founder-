namespace BOM.View
{
    partial class MaterialGridForDummies
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
            this.Input = new System.Windows.Forms.TextBox();
            this.SelectInput = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DataGridCodes = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.NameInput = new System.Windows.Forms.TextBox();
            this.KLTInput = new System.Windows.Forms.TextBox();
            this.LocationInput = new System.Windows.Forms.TextBox();
            this.DescriptionInput = new System.Windows.Forms.RichTextBox();
            this.AmountInput = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SupplierComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.UnitComboBox = new System.Windows.Forms.ComboBox();
            this.UnitLabel = new System.Windows.Forms.Label();
            this.RefreshBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridCodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmountInput)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGrid
            // 
            this.DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid.Location = new System.Drawing.Point(43, 50);
            this.DataGrid.Name = "DataGrid";
            this.DataGrid.Size = new System.Drawing.Size(1049, 247);
            this.DataGrid.TabIndex = 0;
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(519, 507);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 1;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Click_Save);
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
            // DataGridCodes
            // 
            this.DataGridCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridCodes.Location = new System.Drawing.Point(227, 353);
            this.DataGridCodes.Name = "DataGridCodes";
            this.DataGridCodes.Size = new System.Drawing.Size(282, 127);
            this.DataGridCodes.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.label2.Location = new System.Drawing.Point(40, 314);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "Agregar Material";
            // 
            // NameInput
            // 
            this.NameInput.Location = new System.Drawing.Point(530, 349);
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(163, 20);
            this.NameInput.TabIndex = 9;
            // 
            // KLTInput
            // 
            this.KLTInput.Location = new System.Drawing.Point(718, 348);
            this.KLTInput.Name = "KLTInput";
            this.KLTInput.Size = new System.Drawing.Size(148, 20);
            this.KLTInput.TabIndex = 10;
            // 
            // LocationInput
            // 
            this.LocationInput.Location = new System.Drawing.Point(718, 383);
            this.LocationInput.Name = "LocationInput";
            this.LocationInput.Size = new System.Drawing.Size(148, 20);
            this.LocationInput.TabIndex = 11;
            // 
            // DescriptionInput
            // 
            this.DescriptionInput.Location = new System.Drawing.Point(532, 388);
            this.DescriptionInput.Name = "DescriptionInput";
            this.DescriptionInput.Size = new System.Drawing.Size(163, 92);
            this.DescriptionInput.TabIndex = 12;
            this.DescriptionInput.Text = "";
            // 
            // AmountInput
            // 
            this.AmountInput.DecimalPlaces = 2;
            this.AmountInput.Location = new System.Drawing.Point(719, 420);
            this.AmountInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AmountInput.Name = "AmountInput";
            this.AmountInput.Size = new System.Drawing.Size(120, 20);
            this.AmountInput.TabIndex = 13;
            this.AmountInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(224, 337);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Número del producto";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(530, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Nombre";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(531, 372);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Descripción";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(718, 332);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "KLT";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(719, 369);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Location";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(718, 404);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Cantidad";
            // 
            // SupplierComboBox
            // 
            this.SupplierComboBox.FormattingEnabled = true;
            this.SupplierComboBox.Location = new System.Drawing.Point(718, 458);
            this.SupplierComboBox.Name = "SupplierComboBox";
            this.SupplierComboBox.Size = new System.Drawing.Size(148, 21);
            this.SupplierComboBox.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(719, 443);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Proveedor";
            // 
            // UnitComboBox
            // 
            this.UnitComboBox.FormattingEnabled = true;
            this.UnitComboBox.Location = new System.Drawing.Point(896, 347);
            this.UnitComboBox.Name = "UnitComboBox";
            this.UnitComboBox.Size = new System.Drawing.Size(121, 21);
            this.UnitComboBox.TabIndex = 22;
            // 
            // UnitLabel
            // 
            this.UnitLabel.AutoSize = true;
            this.UnitLabel.Location = new System.Drawing.Point(893, 332);
            this.UnitLabel.Name = "UnitLabel";
            this.UnitLabel.Size = new System.Drawing.Size(26, 13);
            this.UnitLabel.TabIndex = 23;
            this.UnitLabel.Text = "Unit";
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Location = new System.Drawing.Point(896, 456);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.RefreshBtn.TabIndex = 24;
            this.RefreshBtn.Text = "Refrescar";
            this.RefreshBtn.UseVisualStyleBackColor = true;
            this.RefreshBtn.Click += new System.EventHandler(this.Click_RefreshBtn);
            // 
            // MaterialGridForDummies
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 542);
            this.Controls.Add(this.RefreshBtn);
            this.Controls.Add(this.UnitLabel);
            this.Controls.Add(this.UnitComboBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.SupplierComboBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AmountInput);
            this.Controls.Add(this.DescriptionInput);
            this.Controls.Add(this.LocationInput);
            this.Controls.Add(this.KLTInput);
            this.Controls.Add(this.NameInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DataGridCodes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectInput);
            this.Controls.Add(this.Input);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.DataGrid);
            this.Name = "MaterialGridForDummies";
            this.Text = "MaterialGrid";
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridCodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmountInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DataGrid;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.TextBox Input;
        private System.Windows.Forms.ComboBox SelectInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView DataGridCodes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox NameInput;
        private System.Windows.Forms.TextBox KLTInput;
        private System.Windows.Forms.TextBox LocationInput;
        private System.Windows.Forms.RichTextBox DescriptionInput;
        private System.Windows.Forms.NumericUpDown AmountInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox SupplierComboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox UnitComboBox;
        private System.Windows.Forms.Label UnitLabel;
        private System.Windows.Forms.Button RefreshBtn;
    }
}