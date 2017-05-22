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

namespace CompModelEx
{
    class Structs
    {
        public struct MACH_STATE
        {
            public unsafe fixed int registers[8];
            public int accum;
            public uint com_counter;
            public int input_reg;
            public bool is_end_work;
        };
        public struct SAVE_DATA
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpStr;
            public MACH_STATE mach_state;
            public unsafe int* memory_state;
            public uint memory_size;
        };
    }
    class SaveLoad
    {
        [DllImport("SaveLoad.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern unsafe bool Save(string path, Structs.SAVE_DATA data);
        [DllImport("SaveLoad.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern unsafe Structs.SAVE_DATA Load(string path);
    }
    class MachMem
    {
        [DllImport("MachMem.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void* Init(uint size);
        [DllImport("MachMem.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe bool GetData(void* memory, uint id_cell, int* data);
        [DllImport("MachMem.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe bool SetData(void* memory, uint id_cell, int data);
        [DllImport("MachMem.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe bool InitProgram(void* memory, void* mach_source);
        [DllImport("MachMem.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void DeleteMemory(void* memory);
    }
    class MachCore
    {
        [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe Structs.MACH_STATE Tick(void* mach);
        [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void Reset(void* mach);
        [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void* Init(void* memory);
        [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void DeleteMach(void* mach);
        [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe bool SetRegData(void* mach, uint reg_id, int data);
        [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void SetCommandNumber(void* mach, uint number);
        [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe bool SetInput(void* mach, int data);
        [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe bool SetAcum(void* mach, int data);
    }
    class Compiler
    {
        [DllImport("Compiler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern unsafe int Compile(string source, void** mach_source);
        [DllImport("Compiler.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void DeleteMachSource(void* mach_source);
    }
}
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

            this.label9.Visible = true;
            this.label10.Visible = true;
            this.label11.Visible = true;
            this.label12.Visible = true;

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

            this.label9.Visible = false;
            this.label10.Visible = false;
            this.label11.Visible = false;
            this.label12.Visible = false;


            this.dataGridView2.Visible = true;

            this.richTextBox6.Visible = false;
            this.richTextBox7.Visible = false;
            this.richTextBox8.Visible = false;
            this.richTextBox9.Visible = false;
            this.richTextBox10.Visible = false;
        }
    }

}

