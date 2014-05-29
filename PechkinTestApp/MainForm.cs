using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Pechkin;

namespace Html2PdfTestApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            this.InitializeComponent();
        }

        public void SetText(string text)
        {
            this.Text = text;
        }

        private void OnConvertButtonClick(object sender, EventArgs e)
        {
            PerformanceCollector pc = new PerformanceCollector("PDF creation");

            var document = new HtmlToPdfDocument
            {
                GlobalSettings = {
                    ProduceOutline = true,
                    DocumentTitle = "Business Document",
                    Margins =
                    {
                        Top = 1.5,
                        Right = 1,
                        Bottom = 1,
                        Left = 1.25,
                        Unit = Unit.Centimeters
                    }
                },
                Objects = {
                    new ObjectSettings { HtmlText = this.htmlText.Text },
                    new ObjectSettings { PageUrl = "www.google.com" },
                    new ObjectSettings { PageUrl = "www.microsoft.com" }
                }
            };

            IPechkin sc2 = Factory.Create();
            var buf = sc2.Convert(document);

            MessageBox.Show("All conversions done");

            pc.FinishAction("conversion finished");

            if (buf == null)
            {
                MessageBox.Show("Error converting!");

                return;
            }

            try
            {
                string fn = string.Format("{0}.pdf", Path.GetTempFileName());

                FileStream fs = new FileStream(fn, FileMode.Create);
                fs.Write(buf, 0, buf.Length);
                fs.Close();

                pc.FinishAction("dumped file to disk");

                Process myProcess = new Process();
                myProcess.StartInfo.FileName = fn;
                myProcess.Start();

                pc.FinishAction("opened it");
            }
            catch
            {
            }

            pc.ShowInMessageBox(null);
        }

        private void OnLoad(object sender, EventArgs e)
        {
        }

        private void OnScBegin(IPechkin converter, int expectedphasecount)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)(() => { Text = string.Format("Begin, PhaseCount: {0}", expectedphasecount); }));
            }
            else
            {
                this.Text = string.Format("Begin, PhaseCount: {0}", expectedphasecount);
            }
        }

        private void OnScError(IPechkin converter, string errorText)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)(() => { MessageBox.Show(string.Format("Error: {0}", errorText)); }));
            }
            else
            {
                MessageBox.Show(string.Format("Error: {0}", errorText));
            }
        }

        private void OnScFinished(IPechkin converter, bool success)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)(() => { Text = string.Format("Finished, Success: {0}", success); }));
            }
            else
            {
                this.Text = string.Format("Finished, Success: {0}", success);
            }
        }

        private void OnScPhase(IPechkin converter, int phasenumber, string phasedescription)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)(() => { Text = string.Format("New Phase {0}: {1}", phasenumber, phasedescription); }));
            }
            else
            {
                this.Text = string.Format("New Phase {0}: {1}", phasenumber, phasedescription);
            }
        }

        private void OnScProgress(IPechkin converter, int progress, string progressdescription)
        {
            if (this.InvokeRequired)
            {
                // simple Invoke WILL NEVER SUCCEDE, because we're in the button click handler. Invoke will simply deadlock everything
                this.BeginInvoke((Action<string>)SetText, string.Format("Progress {0}: {1}", progress, progressdescription));
            }
            else
            {
                this.Text = string.Format("Progress {0}: {1}", progress, progressdescription);
            }
        }

        private void OnScWarning(IPechkin converter, string warningtext)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)(() => { MessageBox.Show(string.Format("Warning: {0}", warningtext)); }));
            }
            else
            {
                MessageBox.Show(string.Format("Warning: {0}", warningtext));
            }
        }
    }
}