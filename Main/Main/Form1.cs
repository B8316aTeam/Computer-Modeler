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
                    CompModelEx.MachMem.InitProgram(mach_mem, mach_source);
                    UpdateMemory(true);
                }
                code_block.ReadOnly = true;
                this.label9.Visible = true;
                this.label11.Visible = true;
                this.res_start_state.Visible = true;
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
            this.res_start_state.Visible = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            CompModelEx.Structs.SAVE_DATA save_data = new CompModelEx.Structs.SAVE_DATA();
            if (save_file_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               
                unsafe
                {
                    save_data.source_code = Marshal.StringToHGlobalUni(code_block.Text);
                    save_data.mach_state = CompModelEx.MachCore.GetMachSate(mach_core);
                    save_data.memory_ = mach_mem;
                    CompModelEx.SaveLoad.Save(save_file_dialog.FileName, save_data);
                    CompModelEx.MachCore.DeleteMachState(save_data.mach_state);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int number = InputData(combo_box_data_type_processor, textbox_input_reg);
                unsafe
                {
                    if (CompModelEx.MachCore.SetInput(mach_core, number))
                    {
                        MessageBox.Show("Превышено максимальное значение");
                        is_env_change = false;
                        textbox_input_reg.Text = Convert.ToString(CompModelEx.MachCore.GetInput(mach_core),10);
                    }
                }
            }
            catch (InvalidOperationException err)
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            unsafe
            {
                mach_mem = CompModelEx.MachMem.Init(1000);
                mach_core = CompModelEx.MachCore.Init(mach_mem);
                combo_box_data_type_processor.Text = "Десятичное";
                combo_box_data_type_memory_i.Text = "Десятичное";
                combo_box_data_type_memory_o.Text = "Десятичное";
                for (int i = 0; i < 100; i++)
                {
                    memory_table.Rows.Add();
                    memory_table.Rows[i].HeaderCell.Value = Convert.ToString(i * 10);
                }
                UpdateMemory(true);
            }
            
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            try
            {
                unsafe
                {
                    CompModelEx.MachCore.SetCommandNumber(mach_core, InputUData(combo_box_data_type_processor, textbox_com_counter));
                }
            }
            catch (InvalidOperationException err)
            {

            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int number = InputData(combo_box_data_type_processor, textbox_accum);
                unsafe
                {
                    if (CompModelEx.MachCore.SetAcum(mach_core, number))
                    {
                        MessageBox.Show("Превышено максимальное значение");
                        is_env_change = false;
                        textbox_accum.Text = Convert.ToString(0, 10);
                    }
                }
            }
            catch (InvalidOperationException err)
            {

            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                unsafe
                {
                    if (CompModelEx.MachCore.SetRegData(mach_core,0, InputData(combo_box_data_type_processor, textbox_reg0)))
                    {
                        MessageBox.Show("Превышено максимальное значение");
                        is_env_change = false;
                        textbox_reg0.Text = Convert.ToString(0, 10);
                    }
                }
            }
            catch (InvalidOperationException err)
            {

            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            try
            {
                unsafe
                {
                    if (CompModelEx.MachCore.SetRegData(mach_core, 1, InputData(combo_box_data_type_processor, textbox_reg1)))
                    {
                        MessageBox.Show("Превышено максимальное значение");
                        is_env_change = false;
                        textbox_reg1.Text = Convert.ToString(0, 10);
                    }
                }
            }
            catch (InvalidOperationException err)
            {

            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            try
            {
                unsafe
                {
                    if (CompModelEx.MachCore.SetRegData(mach_core, 2, InputData(combo_box_data_type_processor, textbox_reg2)))
                    {
                        MessageBox.Show("Превышено максимальное значение");
                        is_env_change = false;
                        textbox_reg2.Text = Convert.ToString(0, 10);
                    }
                }
            }
            catch (InvalidOperationException err)
            {

            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            try
            {
                unsafe
                {
                    if (CompModelEx.MachCore.SetRegData(mach_core, 3, InputData(combo_box_data_type_processor, textbox_reg3)))
                    {
                        MessageBox.Show("Превышено максимальное значение");
                        is_env_change = false;
                        textbox_reg3.Text = Convert.ToString(0, 10);
                    }
                }
            }
            catch (InvalidOperationException err)
            {

            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                unsafe
                {
                    if (CompModelEx.MachCore.SetRegData(mach_core, 4, InputData(combo_box_data_type_processor, textbox_4)))
                    {
                        MessageBox.Show("Превышено максимальное значение");
                        is_env_change = false;
                        textbox_4.Text = Convert.ToString(0, 10);
                    }
                }
            }
            catch (InvalidOperationException err)
            {

            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                unsafe
                {
                    if (CompModelEx.MachCore.SetRegData(mach_core, 5, InputData(combo_box_data_type_processor, textbox_reg5)))
                    {
                        MessageBox.Show("Превышено максимальное значение");
                        is_env_change = false;
                        textbox_reg5.Text = Convert.ToString(0, 10);
                    }
                }
            }
            catch (InvalidOperationException err)
            {

            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                unsafe
                {
                    if (CompModelEx.MachCore.SetRegData(mach_core, 6, InputData(combo_box_data_type_processor, textbox_reg6)))
                    {
                        MessageBox.Show("Превышено максимальное значение");
                        is_env_change = false;
                        textbox_reg6.Text = Convert.ToString(0, 10);
                    }
                }
            }
            catch (InvalidOperationException err)
            {

            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                unsafe
                {
                    if (CompModelEx.MachCore.SetRegData(mach_core, 7, InputData(combo_box_data_type_processor, textbox_reg7)))
                    {
                        MessageBox.Show("Превышено максимальное значение");
                        is_env_change = false;
                        textbox_reg7.Text = Convert.ToString(0, 10);
                    }
                }
            }
            catch (InvalidOperationException err)
            {

            }
        }

        private void combo_box_data_type_processor_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCoreState();
        }

        private void combo_box_data_type_memory_o_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMemory(true);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int osn;
            switch (combo_box_data_type_memory_i.Text)
            {
                case osn16:
                    {
                        osn = 16;
                        break;
                    }
                case osn2:
                    {
                        osn = 2;
                        break;
                    }
                case osn10:
                    {
                        osn = 10;
                        break;
                    }
                default:
                    {
                        System.Windows.Forms.MessageBox.Show("Unknown state");
                        combo_box_data_type_memory_i.Text = osn10;
                        return;
                    }
            }
            int data = 0;
            try
            {
                data = Convert.ToInt32(input_memory_data.Text, osn);
            }
            catch (InvalidOperationException err)
            {
                MessageBox.Show("Uncorrect data");
                return;
            }
            uint cell = 0;
            try
            {
               cell = Convert.ToUInt32(input_memory_cell.Text, 10);
            }
            catch (InvalidOperationException err)
            {
                MessageBox.Show("Uncorrect cell id");
                return;
            }
            unsafe
            {
                try
                {
                    CompModelEx.MachMem.SetData(mach_mem, cell, data);
                }
                catch (ArgumentOutOfRangeException err)
                {
                    MessageBox.Show("Out of memory");
                    return;
                }
            }
            switch (combo_box_data_type_memory_o.Text)
            {
                case osn16:
                    {
                        osn = 16;
                        break;
                    }
                case osn2:
                    {
                        osn = 2;
                        break;
                    }
                case osn10:
                    {
                        osn = 10;
                        break;
                    }
                default:
                    {
                        System.Windows.Forms.MessageBox.Show("Неопознаное основание");
                        combo_box_data_type_memory_o.Text = osn10;
                        return;
                    }
            }
            memory_table.Rows[Convert.ToInt32(cell) / 10].Cells[Convert.ToInt32(cell) % 10].Value = Convert.ToString(data, osn);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            unsafe
            {
                open_file_dialog.ShowDialog();
                CompModelEx.Structs.SAVE_DATA tmp = CompModelEx.SaveLoad.Load(open_file_dialog.FileName);
                // load core state
                CompModelEx.MachCore.SetMachState(mach_core, tmp.mach_state);
                CompModelEx.MachCore.DeleteMachState(tmp.mach_state);
                // load memry
                CompModelEx.MachMem.DeleteMemory(mach_mem);
                mach_mem = tmp.memory_;
                CompModelEx.MachCore.NewMemory(mach_core, mach_mem);
                // load source code
                code_block.Text = Marshal.PtrToStringUni(tmp.source_code);
                UpdateMemory(true);
                UpdateCoreState();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            uint now_com = 1;
            unsafe
            {
                CompModelEx.MachCore.Tick(mach_core);
                now_com = CompModelEx.MachCore.GetCommandNumber(mach_core)+1;
            }
            for (uint i = 0; i < now_com - 1; i++)
                select_start += code_block.Lines[i].Length;
            select_end = code_block.Lines[now_com - 1].Length + 1;
            code_block.Select(select_start, select_end);
            code_block.SelectionColor = Color.Red;
            UpdateCoreState();
            UpdateMemory(false);
            
            
        }
    }

}

