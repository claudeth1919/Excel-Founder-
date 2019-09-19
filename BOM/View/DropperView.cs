using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using BOM.Model;
using BOM.Tool;
using BOM.DataAccess;
using System.Threading;

namespace BOM
{
    public partial class DropperView : Form
    {
        public DropperView()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.LoadingImage.Hide();
        }

        private void DragDropFile(object sender, DragEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate () {
                this.ImageBox.Hide();
                this.LoadingImage.Show();
                this.BtnUpload.Enabled = false;
                string[] filePathArray = e.Data.GetData(DataFormats.FileDrop) as string[]; // get all files path droppeds  
                if (filePathArray != null && filePathArray.Any())
                {
                    foreach (string filesPathItem in filePathArray)
                    {
                        if (filesPathItem.ToUpper().IndexOf(".XLSX") != -1)
                        {
                            //Thread thread = new Thread(() => DropFileAction(filesPathItem));
                            //thread.Name = "DropFileActionThread";
                            //thread.Start();
                            DropFileAction(filesPathItem);
                            //MessageBox.Show("Anexo exitoso");
                        }
                        else
                        {
                            MessageBox.Show("El archivo no es del tipo solicitado");
                        }
                    }
                }
                this.ImageBox.Show();
                this.LoadingImage.Hide();
                this.BtnUpload.Enabled = true;
            }));
            
        }


        public virtual void DropFileAction(string filesPathItem)
        {
            //
        }


        private void DragOverFile(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void UploadFileBtn(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel | *.xlsx"; // file types, that will be allowed to upload
            dialog.Multiselect = false; // allow/deny user to upload more than one file at a time
            if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
                this.ImageBox.Hide();
                this.LoadingImage.Show();
                this.BtnUpload.Enabled = false;
                String path = dialog.FileName; // get name of file
                //Thread thread = new Thread(() => UploadFileAction(path));
                //thread.Name = "UploadFileActionThread";
                //thread.Start();
                UploadFileAction(path);
                //MessageBox.Show("Anexo exitoso");
            }
            this.ImageBox.Show();
            this.LoadingImage.Hide();
            this.BtnUpload.Enabled = true;
        }

        public virtual void UploadFileAction(string filesPathItem)
        {
            //
        }
        private void Click_Cancel(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
