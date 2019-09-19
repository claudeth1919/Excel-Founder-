using BOM.Tool;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace BOM.View
{
    public partial class EmailConfirmation : Form
    {
        public string html;
        public EmailConfirmation(string currentExcelOpenPath)
        {
            InitializeComponent();
            this.EmailInput.Text = "LUNAAADR@schaeffler.com;pontofrn@schaeffler.com";
            string personName = "Andrea";
            string excelPath = currentExcelOpenPath;
            string currentPath = Path.GetDirectoryName(Application.ExecutablePath);
            string pathBOM = String.Empty;
            if (excelPath.IndexOf("O:") != -1)
            {
                string prefix = @"file:///\\schaeffler.com\puebla\data\nl-ppm-o\projects\cme\";
                pathBOM = prefix + Path.GetFullPath(excelPath).Substring(Path.GetPathRoot(excelPath).Length);
            }
            else if(excelPath.IndexOf(@"\\") !=-1)
            {
                string prefix = @"file:///";
                pathBOM = prefix + currentExcelOpenPath;
            }
            
            html = File.ReadAllText(currentPath + @"\Email\Confirmation.html");

            html = html.Replace("{name}", personName);
            html = html.Replace("{bomPath}", pathBOM);
            html = html.Replace("{bomPathName}", currentExcelOpenPath);
        }

        private void Click_Send(object sender, EventArgs e)
        {
            string email = this.EmailInput.Text;
            string[] emailArray=new string[0];
            List<string> sentEmails = new List<string>();
            if (!Util.IsEmptyString(email))
            {
                emailArray = email.Split(';');
            }
            else
            {
                Util.ShowMessage(AlarmType.WARNING, "No tiene texto");
            }
            foreach (string emailItem in emailArray)
            {
                if (Util.IsEmail(emailItem))
                {
                    Email.Send(emailItem, "BOM Procesado", html);
                    sentEmails.Add($"Correo enviado de manera correcta a {emailItem}");
                    this.Close();
                }
                else
                {
                    Util.ShowMessage(AlarmType.WARNING, "No es un correo valido");
                }
            }
            if (sentEmails.Count>0)
            {
                Util.ShowMessage(AlarmType.SUCCESS, sentEmails);
            }
        }

        private void Click_Cancel(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
