using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TuesPechkin;
using TuesPechkin.Wkhtmltox;
using HtmlDocument = TuesPechkin.HtmlDocument;

namespace Html2PdfTestApp
{
    public partial class MainForm : Form
    {
        private IAssembly assembly = new EmbeddedAssembly();

        private HtmlDocument Document = new HtmlDocument
        {
            Objects =
            {
                new ObjectSettings { PageUrl = "www.google.com" }
            }
        };

        public MainForm()
        {
            this.InitializeComponent();

            this.globalSettingsPropertyGrid.SelectedObject = this.Document;
        }

        private void OnConvertButtonClick(object sender, EventArgs e)
        {
            byte[] buf = null;

            try
            {
                IConverter sc2 = new StandardConverter(assembly);
                buf = sc2.Convert(this.Document);
                MessageBox.Show("All conversions done");

                if (buf == null)
                {
                    MessageBox.Show(
                        "No exceptions were raised but wkhtmltopdf failed to convert.",
                        "Error Converting");

                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception Occurred");
            }

            try
            {
                string fn = string.Format("{0}.pdf", Path.GetTempFileName());

                FileStream fs = new FileStream(fn, FileMode.Create);
                fs.Write(buf, 0, buf.Length);
                fs.Close();

                Process myProcess = new Process();
                myProcess.StartInfo.FileName = fn;
                myProcess.Start();
            }
            catch
            {
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
        }
    }
}