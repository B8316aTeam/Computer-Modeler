
using System.Runtime.InteropServices;
using System;

namespace Main
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        unsafe void OneTick()
        {
            uint now_com = 0;
            unsafe
            {
                try
                {
                    CompModelEx.MachCore.Tick(mach_core);
                }
                catch (SEHException err)
                {
                    button10_Click(1, EventArgs.Empty);
                }
                now_com = CompModelEx.MachCore.GetCommandNumber(mach_core);
            }
            try
            {
                commands_list.Rows[last_select_line].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                last_select_line = Convert.ToInt32(now_com);
                commands_list.Rows[last_select_line].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
            }
            catch (ArgumentOutOfRangeException err)
            {

            }
            UpdateCoreState();
            UpdateMemory(false);
            
        }
        unsafe void LoadSaveFile(string path)
        {
            CompModelEx.Structs.SAVE_DATA tmp = CompModelEx.SaveLoad.Load(path);
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
        unsafe void SaveSaveFile(string path)
        {
            CompModelEx.Structs.SAVE_DATA save_data = new CompModelEx.Structs.SAVE_DATA();
            unsafe
            {
                save_data.source_code = Marshal.StringToHGlobalUni(code_block.Text);
                save_data.mach_state = CompModelEx.MachCore.GetMachSate(mach_core);
                save_data.memory_ = mach_mem;
                CompModelEx.SaveLoad.Save(path, save_data);
                CompModelEx.MachCore.DeleteMachState(save_data.mach_state);
            }
        }
        unsafe int InputData(System.Windows.Forms.ComboBox combo_box, System.Windows.Forms.TextBox text_box)
        {
            int osn;
            switch (combo_box.Text)
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
                        combo_box.Text = osn10;
                        throw new InvalidOperationException("Don`t use data");
                    }
            }
            try
            {
                if (is_env_change)
                {
                    return Convert.ToInt32(text_box.Text, osn);
                }
                else
                {
                    is_env_change = true;
                    throw new InvalidOperationException("Don`t use data");
                }
            }
            catch (FormatException er)
            {
                int car_pos = textbox_input_reg.SelectionStart;
                try
                {
                    if (!((textbox_input_reg.Text == "-") && (osn == 10)))
                    {
                        text_box.Text = text_box.Text.Replace(text_box.Text[text_box.TextLength - 1], '0');
                        text_box.SelectionStart = car_pos;
                    }
                }
                catch (IndexOutOfRangeException err)
                {
                }
                throw new InvalidOperationException("Don`t use data");
            }
            catch (ArgumentOutOfRangeException er)
            {
                throw new InvalidOperationException("Don`t use data");
            }
            return 0;
        }
        unsafe uint InputUData (System.Windows.Forms.ComboBox combo_box, System.Windows.Forms.TextBox text_box)
        {
            int osn;
            switch (combo_box.Text)
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
                        combo_box.Text = osn10;
                        throw new InvalidOperationException("Don`t use data");
                    }
            }
            try
            {
                if (is_env_change)
                {
                    return Convert.ToUInt32(text_box.Text, osn);
                }
                else
                {
                    is_env_change = true;
                    throw new InvalidOperationException("Don`t use data");
                }
            }
            catch (FormatException er)
            {
                int car_pos = textbox_input_reg.SelectionStart;
                try
                {
                    text_box.Text = text_box.Text.Replace(text_box.Text[text_box.TextLength - 1], '0');
                    text_box.SelectionStart = car_pos;
                }
                catch (IndexOutOfRangeException err)
                {
                }
                throw new InvalidOperationException("Don`t use data");
            }
            catch (ArgumentOutOfRangeException er)
            {
                throw new InvalidOperationException("Don`t use data");
            }
            return 0;
        }
        void UpdateMemory (bool is_all)
        {

            int osn;
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
                        osn = 10;
                        break;
                    }
            }
            if (is_all)
                for (uint i = 0; ; i++)
                {
                    unsafe
                    {
                        try
                        {
                            memory_table.Rows[Convert.ToInt32(i) / 10].Cells[Convert.ToInt32(i) % 10].Value = 
                                Convert.ToString(CompModelEx.MachMem.GetData(mach_mem, i), osn);
                        }
                        catch (ArgumentOutOfRangeException err)
                        {
                            break;
                        }
                    }
                }
            else
            {
                int data;
                uint id_cell;
                unsafe
                {
                    id_cell = CompModelEx.MachMem.GetLastChange(mach_mem);
                    data = CompModelEx.MachMem.GetData(mach_mem, id_cell);
                    memory_table.Rows[Convert.ToInt32(id_cell) / 10].Cells[Convert.ToInt32(id_cell) % 10].Value = 
                        Convert.ToString(data, osn);
                }
                
            }
        }
        void UpdateCoreState ()
        {
            int osn;
            switch (combo_box_data_type_processor.Text)
            {
                case osn10:
                    osn = 10;
                    break;
                case osn2:
                    osn = 2;
                    break;
                case osn16:
                    osn = 16;
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Неизвестное основание");
                    combo_box_data_type_processor.Text = osn10;
                    osn = 10;
                    break;
            }
            unsafe
            {
                processor_osn = osn;
                is_env_change = false;
                textbox_accum.Text = Convert.ToString(CompModelEx.MachCore.GetAcum(mach_core), osn);

                is_env_change = false;
                textbox_input_reg.Text = Convert.ToString(CompModelEx.MachCore.GetInput(mach_core), osn);

                is_env_change = false;
                textbox_com_counter.Text = Convert.ToString(CompModelEx.MachCore.GetCommandNumber(mach_core), osn);

                is_env_change = false;
                textbox_reg0.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 0), osn);

                is_env_change = false;
                textbox_reg1.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 1), osn);

                is_env_change = false;
                textbox_reg2.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 2), osn);

                is_env_change = false;
                textbox_reg3.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 3), osn);

                is_env_change = false;
                textbox_4.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 4), osn);

                is_env_change = false;
                textbox_reg5.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 5), osn);

                is_env_change = false;
                textbox_reg6.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 6), osn);

                is_env_change = false;
                textbox_reg7.Text = Convert.ToString(CompModelEx.MachCore.GetRegData(mach_core, 7), osn);

                textbox_full_reg.Text = Convert.ToString(CompModelEx.MachCore.GetFullReg(mach_core));
                is_env_change = true;
            }
        }
        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.open_file_dialog = new System.Windows.Forms.OpenFileDialog();
            this.save_file_dialog = new System.Windows.Forms.SaveFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.combo_box_data_type_processor = new System.Windows.Forms.ComboBox();
            this.memory_table = new System.Windows.Forms.DataGridView();
            this.Column0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Панель = new System.Windows.Forms.Panel();
            this.textbox_full_reg = new System.Windows.Forms.TextBox();
            this.textbox_com_counter = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.reset = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.res_start_state = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textbox_reg1 = new System.Windows.Forms.TextBox();
            this.textbox_reg2 = new System.Windows.Forms.TextBox();
            this.textbox_reg3 = new System.Windows.Forms.TextBox();
            this.textbox_4 = new System.Windows.Forms.TextBox();
            this.textbox_reg5 = new System.Windows.Forms.TextBox();
            this.textbox_reg6 = new System.Windows.Forms.TextBox();
            this.textbox_reg7 = new System.Windows.Forms.TextBox();
            this.textbox_reg0 = new System.Windows.Forms.TextBox();
            this.textbox_accum = new System.Windows.Forms.TextBox();
            this.textbox_input_reg = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.combo_box_data_type_memory_i = new System.Windows.Forms.ComboBox();
            this.combo_box_data_type_memory_o = new System.Windows.Forms.ComboBox();
            this.button7 = new System.Windows.Forms.Button();
            this.input_memory_data = new System.Windows.Forms.RichTextBox();
            this.input_memory_cell = new System.Windows.Forms.RichTextBox();
            this.code_block = new System.Windows.Forms.RichTextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.commands_list = new System.Windows.Forms.DataGridView();
            this.cl_command = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl_adress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cl_data = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.memory_table)).BeginInit();
            this.Панель.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commands_list)).BeginInit();
            this.SuspendLayout();
            // 
            // open_file_dialog
            // 
            this.open_file_dialog.DefaultExt = "CMsave";
            this.open_file_dialog.Filter = "Файл сохранения (*.CMsave)|*.CMsave";
            this.open_file_dialog.FileOk += new System.ComponentModel.CancelEventHandler(this.open_file_dialog_FileOk);
            // 
            // save_file_dialog
            // 
            this.save_file_dialog.DefaultExt = "CMsave";
            this.save_file_dialog.Filter = "Файл сохранения (*.CMsave)|*.CMsave";
            this.save_file_dialog.FileOk += new System.ComponentModel.CancelEventHandler(this.save_file_dialog_FileOk);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button1.Location = new System.Drawing.Point(547, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(223, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // combo_box_data_type_processor
            // 
            this.combo_box_data_type_processor.Items.AddRange(new object[] {
            "Двоичное",
            "Десятичное",
            "Шестнадцетиричное"});
            this.combo_box_data_type_processor.Location = new System.Drawing.Point(194, 67);
            this.combo_box_data_type_processor.Name = "combo_box_data_type_processor";
            this.combo_box_data_type_processor.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.combo_box_data_type_processor.Size = new System.Drawing.Size(122, 21);
            this.combo_box_data_type_processor.Sorted = true;
            this.combo_box_data_type_processor.TabIndex = 3;
            this.combo_box_data_type_processor.SelectedIndexChanged += new System.EventHandler(this.combo_box_data_type_processor_SelectedIndexChanged);
            // 
            // memory_table
            // 
            this.memory_table.AllowUserToAddRows = false;
            this.memory_table.AllowUserToDeleteRows = false;
            this.memory_table.AllowUserToResizeColumns = false;
            this.memory_table.AllowUserToResizeRows = false;
            this.memory_table.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.memory_table.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.memory_table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.memory_table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column0,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9});
            this.memory_table.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.memory_table.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.memory_table.Location = new System.Drawing.Point(0, 454);
            this.memory_table.MultiSelect = false;
            this.memory_table.Name = "memory_table";
            this.memory_table.ReadOnly = true;
            this.memory_table.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.memory_table.RowTemplate.ReadOnly = true;
            this.memory_table.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.memory_table.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.memory_table.ShowCellErrors = false;
            this.memory_table.ShowEditingIcon = false;
            this.memory_table.Size = new System.Drawing.Size(1134, 250);
            this.memory_table.TabIndex = 5;
            // 
            // Column0
            // 
            this.Column0.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column0.HeaderText = "0";
            this.Column0.Name = "Column0";
            this.Column0.ReadOnly = true;
            this.Column0.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "3";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "4";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "5";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "6";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "7";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "8";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "9";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Панель
            // 
            this.Панель.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Панель.Controls.Add(this.textbox_full_reg);
            this.Панель.Controls.Add(this.textbox_com_counter);
            this.Панель.Controls.Add(this.label21);
            this.Панель.Controls.Add(this.label20);
            this.Панель.Controls.Add(this.label19);
            this.Панель.Controls.Add(this.label14);
            this.Панель.Controls.Add(this.label11);
            this.Панель.Controls.Add(this.label13);
            this.Панель.Controls.Add(this.label8);
            this.Панель.Controls.Add(this.reset);
            this.Панель.Controls.Add(this.label9);
            this.Панель.Controls.Add(this.res_start_state);
            this.Панель.Controls.Add(this.label7);
            this.Панель.Controls.Add(this.label6);
            this.Панель.Controls.Add(this.label5);
            this.Панель.Controls.Add(this.label4);
            this.Панель.Controls.Add(this.label3);
            this.Панель.Controls.Add(this.label2);
            this.Панель.Controls.Add(this.label1);
            this.Панель.Controls.Add(this.textbox_reg1);
            this.Панель.Controls.Add(this.textbox_reg2);
            this.Панель.Controls.Add(this.textbox_reg3);
            this.Панель.Controls.Add(this.textbox_4);
            this.Панель.Controls.Add(this.textbox_reg5);
            this.Панель.Controls.Add(this.textbox_reg6);
            this.Панель.Controls.Add(this.textbox_reg7);
            this.Панель.Controls.Add(this.textbox_reg0);
            this.Панель.Controls.Add(this.textbox_accum);
            this.Панель.Controls.Add(this.textbox_input_reg);
            this.Панель.Controls.Add(this.combo_box_data_type_processor);
            this.Панель.Dock = System.Windows.Forms.DockStyle.Right;
            this.Панель.Location = new System.Drawing.Point(793, 16);
            this.Панель.Name = "Панель";
            this.Панель.Size = new System.Drawing.Size(338, 429);
            this.Панель.TabIndex = 6;
            this.Панель.Tag = "";
            // 
            // textbox_full_reg
            // 
            this.textbox_full_reg.Location = new System.Drawing.Point(194, 117);
            this.textbox_full_reg.Name = "textbox_full_reg";
            this.textbox_full_reg.Size = new System.Drawing.Size(122, 20);
            this.textbox_full_reg.TabIndex = 29;
            this.textbox_full_reg.TextChanged += new System.EventHandler(this.textBox12_TextChanged);
            // 
            // textbox_com_counter
            // 
            this.textbox_com_counter.Location = new System.Drawing.Point(31, 117);
            this.textbox_com_counter.Name = "textbox_com_counter";
            this.textbox_com_counter.Size = new System.Drawing.Size(122, 20);
            this.textbox_com_counter.TabIndex = 28;
            this.textbox_com_counter.TextChanged += new System.EventHandler(this.textBox11_TextChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label21.Location = new System.Drawing.Point(76, 9);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(173, 18);
            this.label21.TabIndex = 27;
            this.label21.Text = "Область процессора";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(135, 201);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(56, 13);
            this.label20.TabIndex = 26;
            this.label20.Text = "Регистры";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(135, 152);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(73, 13);
            this.label19.TabIndex = 25;
            this.label19.Text = "Аккумулятор";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(201, 51);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(66, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Тип данных";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(201, 101);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Переполнение";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(41, 51);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(92, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "Входной регистр";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(167, 345);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "7";
            // 
            // reset
            // 
            this.reset.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.reset.Location = new System.Drawing.Point(186, 380);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(130, 35);
            this.reset.TabIndex = 26;
            this.reset.Text = "Сброс";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(41, 101);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Счётчик команд";
            // 
            // res_start_state
            // 
            this.res_start_state.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.res_start_state.Location = new System.Drawing.Point(15, 380);
            this.res_start_state.Name = "res_start_state";
            this.res_start_state.Size = new System.Drawing.Size(138, 35);
            this.res_start_state.TabIndex = 27;
            this.res_start_state.Text = "Возврат в исх.";
            this.res_start_state.UseVisualStyleBackColor = true;
            this.res_start_state.Visible = false;
            this.res_start_state.Click += new System.EventHandler(this.res_start_state_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(167, 312);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "6";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(167, 278);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(167, 241);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 345);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 311);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 278);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "0";
            // 
            // textbox_reg1
            // 
            this.textbox_reg1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_reg1.Location = new System.Drawing.Point(31, 275);
            this.textbox_reg1.Name = "textbox_reg1";
            this.textbox_reg1.Size = new System.Drawing.Size(122, 20);
            this.textbox_reg1.TabIndex = 13;
            this.textbox_reg1.TextChanged += new System.EventHandler(this.textBox10_TextChanged);
            // 
            // textbox_reg2
            // 
            this.textbox_reg2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_reg2.Location = new System.Drawing.Point(31, 308);
            this.textbox_reg2.Name = "textbox_reg2";
            this.textbox_reg2.Size = new System.Drawing.Size(122, 20);
            this.textbox_reg2.TabIndex = 12;
            this.textbox_reg2.TextChanged += new System.EventHandler(this.textBox9_TextChanged);
            // 
            // textbox_reg3
            // 
            this.textbox_reg3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_reg3.Location = new System.Drawing.Point(31, 342);
            this.textbox_reg3.Name = "textbox_reg3";
            this.textbox_reg3.Size = new System.Drawing.Size(122, 20);
            this.textbox_reg3.TabIndex = 11;
            this.textbox_reg3.TextChanged += new System.EventHandler(this.textBox8_TextChanged);
            // 
            // textbox_4
            // 
            this.textbox_4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_4.Location = new System.Drawing.Point(186, 238);
            this.textbox_4.Name = "textbox_4";
            this.textbox_4.Size = new System.Drawing.Size(130, 20);
            this.textbox_4.TabIndex = 10;
            this.textbox_4.TextChanged += new System.EventHandler(this.textBox7_TextChanged);
            // 
            // textbox_reg5
            // 
            this.textbox_reg5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_reg5.Location = new System.Drawing.Point(186, 275);
            this.textbox_reg5.Name = "textbox_reg5";
            this.textbox_reg5.Size = new System.Drawing.Size(130, 20);
            this.textbox_reg5.TabIndex = 9;
            this.textbox_reg5.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // textbox_reg6
            // 
            this.textbox_reg6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_reg6.Location = new System.Drawing.Point(186, 309);
            this.textbox_reg6.Name = "textbox_reg6";
            this.textbox_reg6.Size = new System.Drawing.Size(130, 20);
            this.textbox_reg6.TabIndex = 8;
            this.textbox_reg6.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // textbox_reg7
            // 
            this.textbox_reg7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_reg7.Location = new System.Drawing.Point(186, 342);
            this.textbox_reg7.Name = "textbox_reg7";
            this.textbox_reg7.Size = new System.Drawing.Size(130, 20);
            this.textbox_reg7.TabIndex = 7;
            this.textbox_reg7.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textbox_reg0
            // 
            this.textbox_reg0.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_reg0.Location = new System.Drawing.Point(31, 238);
            this.textbox_reg0.Name = "textbox_reg0";
            this.textbox_reg0.Size = new System.Drawing.Size(122, 20);
            this.textbox_reg0.TabIndex = 6;
            this.textbox_reg0.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textbox_accum
            // 
            this.textbox_accum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_accum.Location = new System.Drawing.Point(31, 172);
            this.textbox_accum.Name = "textbox_accum";
            this.textbox_accum.Size = new System.Drawing.Size(285, 20);
            this.textbox_accum.TabIndex = 5;
            this.textbox_accum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textbox_accum.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // textbox_input_reg
            // 
            this.textbox_input_reg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_input_reg.Location = new System.Drawing.Point(31, 67);
            this.textbox_input_reg.Name = "textbox_input_reg";
            this.textbox_input_reg.Size = new System.Drawing.Size(122, 20);
            this.textbox_input_reg.TabIndex = 4;
            this.textbox_input_reg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textbox_input_reg.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button2.Location = new System.Drawing.Point(547, 64);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(223, 37);
            this.button2.TabIndex = 7;
            this.button2.Text = "загрузить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button3.Location = new System.Drawing.Point(547, 107);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(223, 34);
            this.button3.TabIndex = 8;
            this.button3.Text = "выполнить";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel2.Controls.Add(this.button8);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.label18);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.label17);
            this.panel2.Controls.Add(this.combo_box_data_type_memory_i);
            this.panel2.Controls.Add(this.combo_box_data_type_memory_o);
            this.panel2.Controls.Add(this.button7);
            this.panel2.Controls.Add(this.input_memory_data);
            this.panel2.Controls.Add(this.input_memory_cell);
            this.panel2.Location = new System.Drawing.Point(547, 260);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(223, 185);
            this.panel2.TabIndex = 9;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(133, 155);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(81, 21);
            this.button8.TabIndex = 29;
            this.button8.Text = "Сброс";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 90);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(98, 13);
            this.label16.TabIndex = 26;
            this.label16.Text = "Тип отобр.данных";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(3, 49);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(149, 13);
            this.label18.TabIndex = 28;
            this.label18.Text = "Данные,вводимые в ячейку";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 11);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(111, 13);
            this.label15.TabIndex = 25;
            this.label15.Text = "Ячейка куда вводим";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 137);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(96, 13);
            this.label17.TabIndex = 27;
            this.label17.Text = "Тип введ. данных";
            // 
            // combo_box_data_type_memory_i
            // 
            this.combo_box_data_type_memory_i.FormattingEnabled = true;
            this.combo_box_data_type_memory_i.Items.AddRange(new object[] {
            "Десятичное",
            "Двоичное",
            "Шестнадцетиричное"});
            this.combo_box_data_type_memory_i.Location = new System.Drawing.Point(6, 155);
            this.combo_box_data_type_memory_i.Name = "combo_box_data_type_memory_i";
            this.combo_box_data_type_memory_i.Size = new System.Drawing.Size(121, 21);
            this.combo_box_data_type_memory_i.TabIndex = 6;
            // 
            // combo_box_data_type_memory_o
            // 
            this.combo_box_data_type_memory_o.FormattingEnabled = true;
            this.combo_box_data_type_memory_o.Items.AddRange(new object[] {
            "Десятичное",
            "Двоичное",
            "Шестнадцетиричное"});
            this.combo_box_data_type_memory_o.Location = new System.Drawing.Point(6, 106);
            this.combo_box_data_type_memory_o.Name = "combo_box_data_type_memory_o";
            this.combo_box_data_type_memory_o.Size = new System.Drawing.Size(121, 21);
            this.combo_box_data_type_memory_o.TabIndex = 5;
            this.combo_box_data_type_memory_o.SelectedIndexChanged += new System.EventHandler(this.combo_box_data_type_memory_o_SelectedIndexChanged);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(133, 106);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(81, 21);
            this.button7.TabIndex = 4;
            this.button7.Text = "Ввод";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // input_memory_data
            // 
            this.input_memory_data.Location = new System.Drawing.Point(6, 66);
            this.input_memory_data.Name = "input_memory_data";
            this.input_memory_data.Size = new System.Drawing.Size(208, 21);
            this.input_memory_data.TabIndex = 1;
            this.input_memory_data.Text = "";
            // 
            // input_memory_cell
            // 
            this.input_memory_cell.Location = new System.Drawing.Point(6, 27);
            this.input_memory_cell.Name = "input_memory_cell";
            this.input_memory_cell.Size = new System.Drawing.Size(208, 21);
            this.input_memory_cell.TabIndex = 0;
            this.input_memory_cell.Text = "";
            // 
            // code_block
            // 
            this.code_block.AcceptsTab = true;
            this.code_block.Dock = System.Windows.Forms.DockStyle.Left;
            this.code_block.Location = new System.Drawing.Point(3, 16);
            this.code_block.Name = "code_block";
            this.code_block.Size = new System.Drawing.Size(521, 429);
            this.code_block.TabIndex = 11;
            this.code_block.Text = "";
            this.code_block.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.code_block_KeyPress);
            // 
            // button4
            // 
            this.button4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button4.Location = new System.Drawing.Point(547, 26);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(223, 36);
            this.button4.TabIndex = 16;
            this.button4.Tag = "";
            this.button4.Text = "ост. выполнение";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button5.Location = new System.Drawing.Point(547, 64);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(223, 37);
            this.button5.TabIndex = 17;
            this.button5.Text = "непр. выполнение";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button6.Location = new System.Drawing.Point(547, 106);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(223, 34);
            this.button6.TabIndex = 18;
            this.button6.Text = "один такт";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Visible = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.code_block);
            this.groupBox1.Controls.Add(this.Панель);
            this.groupBox1.Controls.Add(this.commands_list);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1134, 448);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            // 
            // label22
            // 
            this.label22.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(745, 241);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(31, 13);
            this.label22.TabIndex = 25;
            this.label22.Text = "2000";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(543, 241);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(25, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "100";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(594, 196);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(120, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Скорость выполнения";
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.trackBar1.LargeChange = 10;
            this.trackBar1.Location = new System.Drawing.Point(546, 209);
            this.trackBar1.Maximum = 2000;
            this.trackBar1.Minimum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(223, 45);
            this.trackBar1.TabIndex = 22;
            this.trackBar1.Value = 100;
            // 
            // button10
            // 
            this.button10.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button10.Location = new System.Drawing.Point(547, 64);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(222, 36);
            this.button10.TabIndex = 21;
            this.button10.Text = "Стоп";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Visible = false;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button9
            // 
            this.button9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button9.Location = new System.Drawing.Point(546, 147);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(223, 33);
            this.button9.TabIndex = 20;
            this.button9.Text = "Новая эмуляция";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // commands_list
            // 
            this.commands_list.AllowUserToAddRows = false;
            this.commands_list.AllowUserToDeleteRows = false;
            this.commands_list.AllowUserToResizeRows = false;
            this.commands_list.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.commands_list.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.commands_list.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.commands_list.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cl_command,
            this.cl_adress,
            this.cl_data});
            this.commands_list.Location = new System.Drawing.Point(5, 16);
            this.commands_list.Name = "commands_list";
            this.commands_list.ReadOnly = true;
            this.commands_list.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.commands_list.ShowEditingIcon = false;
            this.commands_list.Size = new System.Drawing.Size(519, 426);
            this.commands_list.TabIndex = 19;
            this.commands_list.Visible = false;
            // 
            // cl_command
            // 
            this.cl_command.HeaderText = "Command";
            this.cl_command.Name = "cl_command";
            this.cl_command.ReadOnly = true;
            // 
            // cl_adress
            // 
            this.cl_adress.HeaderText = "Adress type";
            this.cl_adress.Name = "cl_adress";
            this.cl_adress.ReadOnly = true;
            // 
            // cl_data
            // 
            this.cl_data.HeaderText = "Data";
            this.cl_data.Name = "cl_data";
            this.cl_data.ReadOnly = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 704);
            this.Controls.Add(this.memory_table);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(1150, 743);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memory_table)).EndInit();
            this.Панель.ResumeLayout(false);
            this.Панель.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commands_list)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView memory_table;
        private System.Windows.Forms.Panel Панель;
        private System.Windows.Forms.TextBox textbox_reg1;
        private System.Windows.Forms.TextBox textbox_reg2;
        private System.Windows.Forms.TextBox textbox_reg3;
        private System.Windows.Forms.TextBox textbox_4;
        private System.Windows.Forms.TextBox textbox_reg5;
        private System.Windows.Forms.TextBox textbox_reg6;
        private System.Windows.Forms.TextBox textbox_reg7;
        private System.Windows.Forms.TextBox textbox_reg0;
        private System.Windows.Forms.TextBox textbox_accum;
        private System.Windows.Forms.TextBox textbox_input_reg;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox input_memory_data;
        private System.Windows.Forms.RichTextBox input_memory_cell;
        private System.Windows.Forms.ComboBox combo_box_data_type_processor;
        private System.Windows.Forms.RichTextBox code_block;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ComboBox combo_box_data_type_memory_i;
        private System.Windows.Forms.ComboBox combo_box_data_type_memory_o;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button reset;
        private System.Windows.Forms.Button res_start_state;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textbox_full_reg;
        private System.Windows.Forms.TextBox textbox_com_counter;
        private unsafe void* mach_source;
        private unsafe void* mach_core;
        private unsafe void* mach_mem;
        private unsafe void* start_state;
        private int last_select_line = 0;
        int processor_osn = 10;
        int select_start = 0;
        int select_end = 0;
        bool is_env_change = true;
        const string osn16 = "Шестнадцетиричное";
        const string osn10 = "Десятичное";
        const string osn2 = "Двоичное";
        public System.Windows.Forms.OpenFileDialog open_file_dialog;
        public System.Windows.Forms.SaveFileDialog save_file_dialog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column0;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridView commands_list;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl_command;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl_adress;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl_data;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label12;
    }
    namespace CompModelEx
    {

        class Structs
        {
            public unsafe struct COMMAND_LIST
            {
                public System.IntPtr ptr;
            };
            public struct SAVE_DATA
            {
                public IntPtr source_code;
                public unsafe void * mach_state;
                public unsafe void * memory_;
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
            public static extern unsafe int GetData(void* memory, uint id_cell);
            [DllImport("MachMem.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe bool SetData(void* memory, uint id_cell, int data);
            [DllImport("MachMem.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe bool InitProgram(void* memory, void* mach_source);
            [DllImport("MachMem.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe void DeleteMemory(void* memory);
            [DllImport("MachMem.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe uint GetLastChange(void* memory);
        }
        class MachCore
        {
            [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe bool Tick(void* mach);
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
            [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe int GetAcum(void* mach);
            [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe uint GetCommandNumber(void* mach);
            [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe int GetRegData(void* mach, uint reg_id);
            [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe int GetInput(void* mach);
            [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe bool GetFullReg(void* mach);
            [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe void* GetMachSate(void* mach);
            [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe void SetMachState(void* mach, void* mach_state);
            [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe void DeleteMachState(void* mach_state);
            [DllImport("MachCore.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe void NewMemory(void* mach, void* memory);
        }
        class Compiler
        {
            [DllImport("Compiler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern unsafe void* Compile(string source, int* error_line);
            [DllImport("Compiler.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern unsafe System.IntPtr GetCommadList(void* mach_source);
            [DllImport("Compiler.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe void DeleteMachSource(void* mach_source);
        }
    }
}

