using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawNassiOpenGL.Models;

namespace DrawNassiOpenGL.Views
{
    public partial class SetBlock : Form
    {
        private string TempString;
        private Block editBlock;
        public Block EditBlock 
        {
            get { return editBlock; }
            set
            {
                if (value != null)
                {
                    editBlock = value;
                    this.textBox1.Text = editBlock.TextRenderer.Text;
                }
            }
        }
        public SetBlock()
        {
            InitializeComponent();
        }

        private void SetBlock_FormClosed(object sender, FormClosedEventArgs e)
        {
            editBlock.ReInit();
            editBlock = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            editBlock.TextRenderer.Text = TempString;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TempString = textBox1.Text;
        }
    }
}
