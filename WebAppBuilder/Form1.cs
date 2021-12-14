using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LocalWebBuilder;

namespace WebAppBuilder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string[] getFiles(string path, string filter, bool multiSelect = true)
        {
            string[] files = "".Split('.');
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = path;
            ofd.Filter = filter;
            ofd.Multiselect = multiSelect;

            if ( ofd.ShowDialog() == DialogResult.OK)
            {
                files = ofd.FileNames;
            }
            

            return files;
        }

        private void btnLoadMainFiles_Click(object sender, EventArgs e)
        {
            txMainFiles.Lines = getFiles("", "Html files|*.htm*");
        }

        private void btnLoadCommonFiles_Click(object sender, EventArgs e)
        {
        }

        private async  void btnGenerateLocalWebApp_Click(object sender, EventArgs e)
        {
            if ( txMainFiles.Lines.Length > 0 )
            {
                Cursor = Cursors.WaitCursor;
                txObfuscatedJS.Text = "";
                if (chbAutoMinify.Checked)
                {
                    var result = await txMainFiles.Lines.GenerateLocalAppAutoMinify(txExckudeJS.Text);
                }
                else
                {
                    var result = txMainFiles.Lines.GenerateLocalApp2(txExckudeJS.Text);
                }
                Cursor = Cursors.Arrow;
                txObfuscatedJS.Text = lwb.FileName;
                if (lwb.FinalFolder.Length > 0)
                {
                    Clipboard.SetText(lwb.FinalFolder);
                    MessageBox.Show("Process Compelted:" + lwb.FileName);
                }
            }
        }

        private void txReferencesToDelete_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnObfuscated_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var result = txMainFiles.Lines.AddObfuscated(txObfuscatedJS.Text);
            Cursor = Cursors.Arrow;
            txObfuscatedJS.Text = lwb.FileName;

        }

        private void btnExcludeFromObfuscate_Click(object sender, EventArgs e)
        {
            txExckudeJS.Lines = getFiles("", "JavaScript files|*.js");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnMinimized_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var result = txMainFiles.Lines.AddMinified3();
            Clipboard.SetText(lwb.FileName);
            Cursor = Cursors.Arrow;
            Clipboard.SetText(lwb.path);
            if ( result < 0 )
            {
                if( lwb.Ex != null )
                {
                    MessageBox.Show(lwb.Ex.Message, "Error Processing file");
                }
                else MessageBox.Show(lwb.Message, "Error Processing file");
            }

        }

        private void btnBase64_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var result = txMainFiles.Lines.FileToBase64();
            txObfuscatedJS.Text = "Base64 finished.";
            Cursor = Cursors.Arrow;
        }
    }
}
