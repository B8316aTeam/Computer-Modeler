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
                last_select_line = 0;
                SaveSaveFile("LastSession.CMsave");
                unsafe
                {
                    commands_list.Rows.Clear();
                    string raw_cm_list = Marshal.PtrToStringUni(CompModelEx.Compiler.GetCommadList(mach_source));
                    int chek = 0, line_id = 0;
                    string[] com_l = new string[3];
                    for (int i = 0; i < raw_cm_list.Length ; i++)
                    {
                        if (raw_cm_list[i] == '\t')
                        {
                            chek = (chek + 1) % 3;
                            continue;
                        }
                        if (raw_cm_list[i] == '\n')
                        {
                            chek = 0;
                            commands_list.Rows.Add(com_l);
                            commands_list.Rows[line_id].HeaderCell.Value = Convert.ToString(line_id);
                            line_id++;
                            com_l = new string[3];
                            continue;
                        }
                        com_l[chek] += raw_cm_list[i];
                    }
                    CompModelEx.MachMem.InitProgram(mach_mem, mach_source);
                    unsafe
                    {
                        start_state = CompModelEx.MachCore.GetMachSate(mach_core);
                    }
                    UpdateMemory(true);
                }
                this.label9.Visible = true;
                this.label11.Visible = true;
                this.res_start_state.Visible = true;
                this.button4.Visible = true;
                this.button5.Visible = true;
                this.button6.Visible = true;
                this.button1.Visible = false;
                this.button2.Visible = false;
                this.button3.Visible = false;
                this.button9.Visible = false;
                this.code_block.Visible = false;
                this.commands_list.Visible = true;
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
            LoadSaveFile("LastSession.CMsave");
            unsafe
            {
                CompModelEx.MachCore.DeleteMachState(start_state);
            }
            this.button9.Visible = true;
            this.button4.Visible = false;
            this.button5.Visible = false;
            this.button6.Visible = false;
            this.button1.Visible = true;
            this.button2.Visible = true;
            this.button3.Visible = true;
            this.res_start_state.Visible = false;
            this.code_block.Visible = true;
            this.commands_list.Visible = false;
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

            if (save_file_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveSaveFile(save_file_dialog.FileName);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int number = InputData(combo_box_data_type_processor, textbox_input_reg);
                unsafe
                {
                    CompModelEx.MachCore.SetInput(mach_core, number);
                    textbox_input_reg.Text = Convert.ToString(CompModelEx.MachCore.GetInput(mach_core), processor_osn);
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
                if (System.IO.File.Exists("LastSession.CMsave"))
                    LoadSaveFile("LastSession.CMsave");
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
                    CompModelEx.MachCore.SetAcum(mach_core, number);
                    textbox_accum.Text = Convert.ToString(CompModelEx.MachCore.GetAcum(mach_core), processor_osn);
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
                    CompModelEx.MachCore.SetRegData(mach_core, 0, InputData(combo_box_data_type_processor, textbox_reg0));
                    textbox_reg0.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core,0), processor_osn);
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
                    CompModelEx.MachCore.SetRegData(mach_core, 1, InputData(combo_box_data_type_processor, textbox_reg1));
                    textbox_reg1.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 1), processor_osn);
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
                    CompModelEx.MachCore.SetRegData(mach_core, 2, InputData(combo_box_data_type_processor, textbox_reg2));
                    textbox_reg2.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 2), processor_osn);
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
                    CompModelEx.MachCore.SetRegData(mach_core, 3, InputData(combo_box_data_type_processor, textbox_reg3));
                    textbox_reg3.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 3), processor_osn);
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
                    CompModelEx.MachCore.SetRegData(mach_core, 4, InputData(combo_box_data_type_processor, textbox_4));
                    textbox_4.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 4), processor_osn);
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
                    CompModelEx.MachCore.SetRegData(mach_core, 5, InputData(combo_box_data_type_processor, textbox_reg5));
                    textbox_reg5.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 5), processor_osn);
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
                    CompModelEx.MachCore.SetRegData(mach_core, 6, InputData(combo_box_data_type_processor, textbox_reg6));
                    textbox_reg6.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 6), processor_osn);
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
                    CompModelEx.MachCore.SetRegData(mach_core, 7, InputData(combo_box_data_type_processor, textbox_reg7));
                    textbox_reg7.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 7), processor_osn);
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
            if (open_file_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadSaveFile(open_file_dialog.FileName);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OneTick();
        }

        private void save_file_dialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void open_file_dialog_FileOk(object sender, CancelEventArgs e)
        {
            LoadSaveFile(open_file_dialog.FileName);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSaveFile("LastSession.CMsave");
        }

        private void res_start_state_Click(object sender, EventArgs e)
        {
            unsafe
            {
                CompModelEx.MachCore.SetMachState(mach_core, start_state);
                UpdateCoreState();
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            unsafe
            {
                CompModelEx.MachCore.DeleteMach(mach_core);
                mach_core = CompModelEx.MachCore.Init(mach_mem);
                UpdateCoreState();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            unsafe
            {
                CompModelEx.MachMem.DeleteMemory(mach_mem);
                mach_mem = CompModelEx.MachMem.Init(1000);
                CompModelEx.MachCore.NewMemory(mach_core,mach_mem);
                UpdateMemory(true);
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            unsafe
            {
                button1_Click(1, EventArgs.Empty);
                CompModelEx.MachMem.DeleteMemory(mach_mem);
                CompModelEx.MachCore.DeleteMach(mach_core);
                mach_mem = CompModelEx.MachMem.Init(1000);
                mach_core = CompModelEx.MachCore.Init(mach_mem);
                UpdateCoreState();
                UpdateMemory(true);
                code_block.Text = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            OneTick();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Interval = trackBar1.Value;
            timer1.Start();
            button4.Visible = false;
            button5.Visible = false;
            button10.Visible = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button4.Visible = true;
            button5.Visible = true;
            button10.Visible = false;
        }
    }

}

