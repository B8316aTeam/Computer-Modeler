using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Main
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
        }

     

    private void button3_Click(object sender, EventArgs e)
        {


            /*Form1 form2 = new Form1();
            this.Visible = false;
            form2.Show();*/
            this.richTextBox6.Visible = true;
            this.richTextBox7.Visible = true;
            this.richTextBox8.Visible = true;
            this.richTextBox9.Visible = true;
            this.richTextBox10.Visible = true;

            this.button1.Text = "остановить выполн.";
            this.button2.Text = "непр выполн.";
            this.button3.Text = "один такт";
            this.dataGridView2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView2.Visible = true;


            this.button1.Text = "сохр.";
            this.button2.Text = "загр.";
            this.button3.Text = "выполн.";

            this.richTextBox6.Visible = false;
            this.richTextBox7.Visible = false;
            this.richTextBox8.Visible = false;
            this.richTextBox9.Visible = false;
            this.richTextBox10.Visible = false;
        }
    }

}

