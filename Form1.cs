using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfPatcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog1.FileName))
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }
        
        private void buttonRun_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(textBox1.Text);
            labelStatus.Text = "正在编辑 " + fileInfo.Name + " ...";
            labelStatus.Refresh();

            PdfService.PatchPdfFile(textBox1.Text);

            labelStatus.Text = fileInfo.Name + " 编辑完毕";
            labelStatus.Refresh();
        }
    }
}
