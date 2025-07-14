using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analyzer_WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Resulter result = AnalyzerCycle.Check(textBox1.Text);
            if (result.errPos != -1)
            {
                this.ActiveControl = textBox1;
                textBox1.SelectionStart = result.errPos;
                textBox1.SelectionLength = 0;
                label4.Text = " ";
            }
            else
            {
                label4.Text = AnalyzerCycle.Semantics();
            }
            label2.Text = result.ErrMessage;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
