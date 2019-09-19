namespace BOM
{
    partial class Menu
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
            this.BtnOptionWarehouse = new System.Windows.Forms.Button();
            this.BtnOptionCheckExistence = new System.Windows.Forms.Button();
            this.BtnOptionGetMaterial = new System.Windows.Forms.Button();
            this.InventarioBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnOptionWarehouse
            // 
            this.BtnOptionWarehouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.BtnOptionWarehouse.Location = new System.Drawing.Point(285, 34);
            this.BtnOptionWarehouse.Name = "BtnOptionWarehouse";
            this.BtnOptionWarehouse.Size = new System.Drawing.Size(177, 62);
            this.BtnOptionWarehouse.TabIndex = 0;
            this.BtnOptionWarehouse.Text = "Agregar a Almacén";
            this.BtnOptionWarehouse.UseVisualStyleBackColor = true;
            this.BtnOptionWarehouse.Click += new System.EventHandler(this.Click_OptionAddWarehouse);
            // 
            // BtnOptionCheckExistence
            // 
            this.BtnOptionCheckExistence.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.BtnOptionCheckExistence.Location = new System.Drawing.Point(54, 29);
            this.BtnOptionCheckExistence.Name = "BtnOptionCheckExistence";
            this.BtnOptionCheckExistence.Size = new System.Drawing.Size(177, 82);
            this.BtnOptionCheckExistence.TabIndex = 1;
            this.BtnOptionCheckExistence.Text = "Insertar BOM";
            this.BtnOptionCheckExistence.UseVisualStyleBackColor = true;
            this.BtnOptionCheckExistence.Click += new System.EventHandler(this.Click_OptionCheckExistence);
            // 
            // BtnOptionGetMaterial
            // 
            this.BtnOptionGetMaterial.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.BtnOptionGetMaterial.Location = new System.Drawing.Point(54, 129);
            this.BtnOptionGetMaterial.Name = "BtnOptionGetMaterial";
            this.BtnOptionGetMaterial.Size = new System.Drawing.Size(177, 84);
            this.BtnOptionGetMaterial.TabIndex = 2;
            this.BtnOptionGetMaterial.Text = "Reservar Material";
            this.BtnOptionGetMaterial.UseVisualStyleBackColor = true;
            this.BtnOptionGetMaterial.Click += new System.EventHandler(this.Click_OptionGetMaterial);
            // 
            // InventarioBtn
            // 
            this.InventarioBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.InventarioBtn.Location = new System.Drawing.Point(54, 235);
            this.InventarioBtn.Name = "InventarioBtn";
            this.InventarioBtn.Size = new System.Drawing.Size(177, 82);
            this.InventarioBtn.TabIndex = 3;
            this.InventarioBtn.Text = "Inventario";
            this.InventarioBtn.UseVisualStyleBackColor = true;
            this.InventarioBtn.Click += new System.EventHandler(this.Click_Inventario);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 343);
            this.Controls.Add(this.InventarioBtn);
            this.Controls.Add(this.BtnOptionGetMaterial);
            this.Controls.Add(this.BtnOptionCheckExistence);
            this.Controls.Add(this.BtnOptionWarehouse);
            this.Name = "Menu";
            this.Text = "Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnOptionWarehouse;
        private System.Windows.Forms.Button BtnOptionCheckExistence;
        private System.Windows.Forms.Button BtnOptionGetMaterial;
        private System.Windows.Forms.Button InventarioBtn;
    }
}