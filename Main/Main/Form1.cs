using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
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
            int error_line = 0;
            unsafe
            {
                mach_source = CompModelEx.Compiler.Compile(code_block.Text, &error_line);
            }
            if (error_line == 0)
            {
                source_code = code_block.Text;
                unsafe
                {
                    code_block.Text = Marshal.PtrToStringUni(CompModelEx.Compiler.GetCommadList(mach_source));
                }
                code_block.ReadOnly = true;
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
            else
            {
                for (int i = 0; i < error_line - 1; i++)
                    select_start += code_block.Lines[i].Length;
                select_end = code_block.Lines[error_line - 1].Length + 1;
                code_block.Select(select_start, select_end);
                code_block.SelectionColor = Color.Red;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            code_block.Text = source_code;
            this.button4.Visible = false;
            this.button5.Visible = false;
            this.button6.Visible = false;
            this.button1.Visible = true;
            this.button2.Visible = true;
            this.button3.Visible = true;

            this.label9.Visible = false;
            
            this.label11.Visible = false;
        }

        private void code_block_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (select_end != 0)
            {
                int pos = code_block.SelectionStart;
                code_block.Select(select_start, select_end);
                code_block.SelectionColor = Color.Black;
                code_block.Select(0, 0);
                code_block.SelectionStart = pos;
                select_end = 0;
                select_start = 0;
            }
        }
    }

}

