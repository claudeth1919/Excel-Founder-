namespace BOM
{
    partial class DropperView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DropperView));
            this.BtnUpload = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.LoadingImage = new System.Windows.Forms.PictureBox();
            this.ImageBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnUpload
            // 
            this.BtnUpload.Location = new System.Drawing.Point(121, 248);
            this.BtnUpload.Name = "BtnUpload";
            this.BtnUpload.Size = new System.Drawing.Size(75, 23);
            this.BtnUpload.TabIndex = 1;
            this.BtnUpload.Text = "Upload File";
            this.BtnUpload.UseVisualStyleBackColor = true;
            this.BtnUpload.Click += new System.EventHandler(this.UploadFileBtn);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(259, 248);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.Click_Cancel);
            // 
            // LoadingImage
            // 
            this.LoadingImage.BackColor = System.Drawing.SystemColors.Control;
            this.LoadingImage.BackgroundImage = global::BOM.Properties.Resources.clock;
            this.LoadingImage.InitialImage = global::BOM.Properties.Resources.clock;
            this.LoadingImage.Location = new System.Drawing.Point(103, -4);
            this.LoadingImage.Name = "LoadingImage";
            this.LoadingImage.Size = new System.Drawing.Size(253, 256);
            this.LoadingImage.TabIndex = 3;
            this.LoadingImage.TabStop = false;
            // 
            // ImageBox
            // 
            this.ImageBox.Image = ((System.Drawing.Image)(resources.GetObject("ImageBox.Image")));
            this.ImageBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("ImageBox.InitialImage")));
            this.ImageBox.Location = new System.Drawing.Point(42, 43);
            this.ImageBox.Name = "ImageBox";
            this.ImageBox.Size = new System.Drawing.Size(400, 198);
            this.ImageBox.TabIndex = 0;
            this.ImageBox.TabStop = false;
            // 
            // DropperView
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 286);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnUpload);
            this.Controls.Add(this.LoadingImage);
            this.Controls.Add(this.ImageBox);
            this.Name = "DropperView";
            this.Text = "Warehouse (Drop Material)";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropFile);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.DragOverFile);
            ((System.ComponentModel.ISupportInitialize)(this.LoadingImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox ImageBox;
        public System.Windows.Forms.Button BtnUpload;
        public System.Windows.Forms.Button BtnCancel;
        public System.Windows.Forms.PictureBox LoadingImage;
    }
}

