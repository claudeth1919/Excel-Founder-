namespace BOM
{
    partial class BOMDropperView
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
            this.LinkInput = new System.Windows.Forms.TextBox();
            this.LinkLabelIndication = new System.Windows.Forms.Label();
            this.ProcessTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingImage)).BeginInit();
            this.SuspendLayout();
            // 
            // ImageBox
            // 
            this.ImageBox.Location = new System.Drawing.Point(110, 44);
            this.ImageBox.Size = new System.Drawing.Size(440, 198);
            // 
            // BtnUpload
            // 
            this.BtnUpload.Location = new System.Drawing.Point(260, 248);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(420, 248);
            // 
            // LoadingImage
            // 
            this.LoadingImage.BackgroundImage = null;
            this.LoadingImage.Image = global::BOM.Properties.Resources.clock;
            this.LoadingImage.Location = new System.Drawing.Point(27, 69);
            this.LoadingImage.Size = new System.Drawing.Size(137, 136);
            // 
            // LinkInput
            // 
            this.LinkInput.Location = new System.Drawing.Point(246, 9);
            this.LinkInput.Name = "LinkInput";
            this.LinkInput.Size = new System.Drawing.Size(201, 20);
            this.LinkInput.TabIndex = 5;
            this.LinkInput.TextChanged += new System.EventHandler(this.TextChanged_PathLink);
            // 
            // LinkLabelIndication
            // 
            this.LinkLabelIndication.AutoSize = true;
            this.LinkLabelIndication.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.LinkLabelIndication.Location = new System.Drawing.Point(97, 11);
            this.LinkLabelIndication.Name = "LinkLabelIndication";
            this.LinkLabelIndication.Size = new System.Drawing.Size(83, 18);
            this.LinkLabelIndication.TabIndex = 6;
            this.LinkLabelIndication.Text = "Copiar Link";
            // 
            // ProcessTextBox
            // 
            this.ProcessTextBox.Location = new System.Drawing.Point(170, 44);
            this.ProcessTextBox.Name = "ProcessTextBox";
            this.ProcessTextBox.Size = new System.Drawing.Size(423, 198);
            this.ProcessTextBox.TabIndex = 7;
            this.ProcessTextBox.Text = "";
            // 
            // BOMDropperView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(646, 286);
            this.Controls.Add(this.ProcessTextBox);
            this.Controls.Add(this.LinkLabelIndication);
            this.Controls.Add(this.LinkInput);
            this.Name = "BOMDropperView";
            this.Controls.SetChildIndex(this.ImageBox, 0);
            this.Controls.SetChildIndex(this.LoadingImage, 0);
            this.Controls.SetChildIndex(this.BtnUpload, 0);
            this.Controls.SetChildIndex(this.BtnCancel, 0);
            this.Controls.SetChildIndex(this.LinkInput, 0);
            this.Controls.SetChildIndex(this.LinkLabelIndication, 0);
            this.Controls.SetChildIndex(this.ProcessTextBox, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LinkInput;
        private System.Windows.Forms.Label LinkLabelIndication;
        private System.Windows.Forms.RichTextBox ProcessTextBox;
    }
}

