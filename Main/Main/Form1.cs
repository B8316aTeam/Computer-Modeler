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

            this.label9.Visible = true;
           
            this.label11.Visible = true;
            

           

            this.button8.Visible = true;
            this.button9.Visible = true;

            this.button4.Visible = true;
            this.button5.Visible = true;
            this.button6.Visible = true;
            this.button1.Visible = false;
            this.button2.Visible = false;
            this.button3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {


            this.button4.Visible = false;
            this.button5.Visible = false;
            this.button6.Visible = false;
            this.button1.Visible = true;
            this.button2.Visible = true;
            this.button3.Visible = true;

            this.label9.Visible = false;
            
            this.label11.Visible = false;
            

            this.dataGridView2.Visible = true;
        }
    }

}

