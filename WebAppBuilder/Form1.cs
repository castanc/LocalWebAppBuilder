using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private void btnGenerateLocalWebApp_Click(object sender, EventArgs e)
        {
            if ( txMainFiles.Lines.Length > 0 )
            {
                Cursor = Cursors.WaitCursor;
                var result = txMainFiles.Lines.GenerateLocalApp(txExckudeJS.Text);
                Cursor = Cursors.Arrow;
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
            var result = txMainFiles.Lines.AddMinimized();
            Cursor = Cursors.Arrow;
        }
    }
}
