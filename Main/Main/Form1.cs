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
        struct MACH_STATE
        {
            public unsafe fixed int registers[8];
            public int accum;
            public uint com_counter;
            public int input_reg;
            public bool is_end_work;
        };
        struct SAVE_DATA
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpStr;
            public MACH_STATE mach_state;
            public unsafe int* memory_state;
            public uint memory_size;
        };
        [DllImport("SaveLoad.dll", CallingConvention = CallingConvention.Cdecl, CharSet=CharSet.Ansi)]
        static extern unsafe bool Save(string path, SAVE_DATA data);
        [DllImport("SaveLoad.dll", CallingConvention = CallingConvention.Cdecl, CharSet=CharSet.Unicode)]
        new static extern unsafe SAVE_DATA Load(char* path);
        [DllImport("MachMem.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern unsafe void* Init(uint size);
        public Form1()
        {
            unsafe
            {
                SAVE_DATA data = new SAVE_DATA();
                MACH_STATE state = new MACH_STATE();
                data.lpStr = "test\0";
                int* factorial = stackalloc int[2];
                data.memory_state = factorial;
                data.memory_state[0] = 1;
                data.memory_state[1] = 2;
                data.memory_size = 2;
                state.accum = 10;
                for (int i = 0; i<8;i++)
                    state.registers[i] = 189;
                state.is_end_work = true;
                state.input_reg = 89;
                state.com_counter = 20;
                data.mach_state = state;
                Init(3);
                Save("test.txt\0", data);
            }
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

            this.dataGridView2.Visible = false;

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


            this.dataGridView2.Visible = true;

            this.richTextBox6.Visible = false;
            this.richTextBox7.Visible = false;
            this.richTextBox8.Visible = false;
            this.richTextBox9.Visible = false;
            this.richTextBox10.Visible = false;
        }
    }

}

