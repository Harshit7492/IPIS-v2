
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPIS
{
    partial class ucSingleLineBoardSetting
    {
   
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btndelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.lblboardHeader = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbDisplayinfo = new System.Windows.Forms.ComboBox();
            this.cmbEffect = new System.Windows.Forms.ComboBox();
            this.cmbDTO = new System.Windows.Forms.ComboBox();
            this.cmbScrollSpeed = new System.Windows.Forms.ComboBox();
            this.cmbDelayTime = new System.Windows.Forms.ComboBox();
            this.cmbBoardSize = new System.Windows.Forms.ComboBox();
            this.cmbFontSize = new System.Windows.Forms.ComboBox();
            this.cmbIntensity = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.lblDto = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.intensityLbl = new System.Windows.Forms.Label();
            this.lblSoftReset = new System.Windows.Forms.Label();
            this.checkBoxSoftReset = new System.Windows.Forms.CheckBox();

            // NEW module controls
            this.lblModule1 = new System.Windows.Forms.Label();
            this.txtModule1 = new System.Windows.Forms.TextBox();
            this.lblModule2 = new System.Windows.Forms.Label();
            this.txtModule2 = new System.Windows.Forms.TextBox();
            this.lblModule3 = new System.Windows.Forms.Label();
            this.txtModule3 = new System.Windows.Forms.TextBox();

            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.txtIpConfig = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbNoOfLine = new System.Windows.Forms.ComboBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.flow_layout_language = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.flow_layout_Plateform = new System.Windows.Forms.FlowLayoutPanel();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Beige;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblboardHeader, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.979073F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.20588F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.91176462F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1051, 1133);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 5;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel6, 2);
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel6.Controls.Add(this.btnSave, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.btndelete, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.btn_close, 4, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(4, 1057);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1043, 73);
            this.tableLayoutPanel6.TabIndex = 4;
            // 
            // btndelete
            // 
            this.btndelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btndelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndelete.ForeColor = System.Drawing.Color.Maroon;
            this.btndelete.Location = new System.Drawing.Point(420, 3);
            this.btndelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(200, 67);
            this.btndelete.TabIndex = 2;
            this.btndelete.Text = "Delete";
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.Maroon;
            this.btnSave.Location = new System.Drawing.Point(212, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(200, 67);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btn_close
            // 
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.ForeColor = System.Drawing.Color.Maroon;
            this.btn_close.Location = new System.Drawing.Point(628, 3);
            this.btn_close.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(200, 67);
            this.btn_close.TabIndex = 3;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // lblboardHeader
            // 
            this.lblboardHeader.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblboardHeader, 2);
            this.lblboardHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblboardHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblboardHeader.ForeColor = System.Drawing.Color.Maroon;
            this.lblboardHeader.Location = new System.Drawing.Point(4, 0);
            this.lblboardHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblboardHeader.Name = "lblboardHeader";
            this.lblboardHeader.Size = new System.Drawing.Size(1043, 67);
            this.lblboardHeader.TabIndex = 0;
            this.lblboardHeader.Text = "Single Line";
            this.lblboardHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.tableLayoutPanel4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(529, 70);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(518, 981);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Display Property";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Beige;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.cmbEffect, 1, 4);
            this.tableLayoutPanel4.Controls.Add(this.cmbScrollSpeed, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.cmbDelayTime, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.cmbBoardSize, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.cmbFontSize, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.cmbIntensity, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label23, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.label19, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.label15, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.label13, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label11, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.intensityLbl, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblDto, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.cmbDTO, 1, 5);
            this.tableLayoutPanel4.Controls.Add(this.lblSoftReset, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.checkBoxSoftReset, 1, 6);

            // NEW: module rows appended at the bottom
            this.tableLayoutPanel4.Controls.Add(this.lblModule1, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.txtModule1, 1, 7);
            this.tableLayoutPanel4.Controls.Add(this.lblModule2, 0, 8);
            this.tableLayoutPanel4.Controls.Add(this.txtModule2, 1, 8);
            this.tableLayoutPanel4.Controls.Add(this.lblModule3, 0, 9);
            this.tableLayoutPanel4.Controls.Add(this.txtModule3, 1, 9);

            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(4, 31);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 11;
            // Rows 0-6: Fixed compact height for standard display settings
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            // Rows 7-9: Module IP rows (shown as 42F for VDB/AVDB, collapsed to 0F otherwise)
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            // Row 10: Empty filler row that absorbs remaining vertical space in groupBox1
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(510, 947);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // cmbDisplayinfo
            // 
            this.cmbDisplayinfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDisplayinfo.FormattingEnabled = true;
            this.cmbDisplayinfo.Items.AddRange(new object[] { "BOTH", "ARR", "DEP" });
            this.cmbDisplayinfo.Location = new System.Drawing.Point(259, 378);
            this.cmbDisplayinfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbDisplayinfo.Name = "cmbDisplayinfo";
            this.cmbDisplayinfo.Size = new System.Drawing.Size(247, 36);
            this.cmbDisplayinfo.TabIndex = 28;
            // 
            // cmbEffect
            // 
            this.cmbEffect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbEffect.FormattingEnabled = true;
            this.cmbEffect.Location = new System.Drawing.Point(259, 303);
            this.cmbEffect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbEffect.Name = "cmbEffect";
            this.cmbEffect.Size = new System.Drawing.Size(247, 36);
            this.cmbEffect.TabIndex = 27;
            // 
            // cmbDTO
            // 
            this.cmbDTO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDTO.FormattingEnabled = true;
            this.cmbDTO.Items.AddRange(new object[] { "60", "90", "120", "150", "180", "210", "240" });
            this.cmbDTO.Location = new System.Drawing.Point(244, 230);
            this.cmbDTO.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbDTO.Name = "cmbDTO";
            this.cmbDTO.Size = new System.Drawing.Size(253, 36);
            this.cmbDTO.TabIndex = 18;
            // 
            // cmbScrollSpeed
            // 
            this.cmbScrollSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbScrollSpeed.FormattingEnabled = true;
            this.cmbScrollSpeed.Location = new System.Drawing.Point(259, 228);
            this.cmbScrollSpeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbScrollSpeed.Name = "cmbScrollSpeed";
            this.cmbScrollSpeed.Size = new System.Drawing.Size(247, 36);
            this.cmbScrollSpeed.TabIndex = 25;
            // 
            // cmbDelayTime
            // 
            this.cmbDelayTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDelayTime.FormattingEnabled = true;
            this.cmbDelayTime.Items.AddRange(new object[] { "5", "10", "20", "30", "40", "50", "60", "70", "80", "90", "100", "110", "120" });
            this.cmbDelayTime.Location = new System.Drawing.Point(259, 153);
            this.cmbDelayTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbDelayTime.Name = "cmbDelayTime";
            this.cmbDelayTime.Size = new System.Drawing.Size(247, 36);
            this.cmbDelayTime.TabIndex = 23;
            // 
            // cmbBoardSize
            // 
            this.cmbBoardSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBoardSize.FormattingEnabled = true;
            this.cmbBoardSize.Items.AddRange(new object[] { "16*336" });
            this.cmbBoardSize.Location = new System.Drawing.Point(259, 78);
            this.cmbBoardSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbBoardSize.Name = "cmbBoardSize";
            this.cmbBoardSize.Size = new System.Drawing.Size(247, 36);
            this.cmbBoardSize.TabIndex = 22;
            this.cmbBoardSize.Visible = false;
            // 
            // cmbFontSize
            // 
            this.cmbFontSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFontSize.FormattingEnabled = true;
            this.cmbFontSize.Location = new System.Drawing.Point(259, 3);
            this.cmbFontSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbFontSize.Name = "cmbFontSize";
            this.cmbFontSize.Size = new System.Drawing.Size(247, 36);
            this.cmbFontSize.TabIndex = 21;
            // 
            // cmbIntensity
            // 
            this.cmbIntensity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbIntensity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIntensity.FormattingEnabled = true;
            this.cmbIntensity.Items.AddRange(new object[] { "25", "50", "75", "100" });
            this.cmbIntensity.Location = new System.Drawing.Point(259, 78);
            this.cmbIntensity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbIntensity.Name = "cmbIntensity";
            this.cmbIntensity.Size = new System.Drawing.Size(247, 36);
            this.cmbIntensity.TabIndex = 29;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label25.Location = new System.Drawing.Point(4, 375);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(247, 75);
            this.label25.TabIndex = 18;
            this.label25.Text = "Info Displayed";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label23.Location = new System.Drawing.Point(4, 300);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(247, 36);
            this.label23.TabIndex = 16;
            this.label23.Text = "Effect";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Location = new System.Drawing.Point(4, 225);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(247, 36);
            this.label19.TabIndex = 12;
            this.label19.Text = "Scrolling Speed";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Location = new System.Drawing.Point(4, 150);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(247, 36);
            this.label15.TabIndex = 8;
            this.label15.Text = "PTO (in Sec)";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(4, 75);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(247, 36);
            this.label13.TabIndex = 6;
            this.label13.Text = "Board Size";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label13.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(4, 0);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(247, 36);
            this.label11.TabIndex = 4;
            this.label11.Text = "Font Size";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // intensityLbl
            // 
            this.intensityLbl.AutoSize = true;
            this.intensityLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.intensityLbl.Location = new System.Drawing.Point(4, 75);
            this.intensityLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.intensityLbl.Name = "intensityLbl";
            this.intensityLbl.Size = new System.Drawing.Size(247, 36);
            this.intensityLbl.TabIndex = 19;
            this.intensityLbl.Text = "Board Intensity";
            this.intensityLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSoftReset
            // 
            this.lblSoftReset.AutoSize = true;
            this.lblSoftReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSoftReset.Location = new System.Drawing.Point(4, 450);
            this.lblSoftReset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSoftReset.Name = "lblSoftReset";
            this.lblSoftReset.Size = new System.Drawing.Size(247, 36);
            this.lblSoftReset.TabIndex = 20;
            this.lblSoftReset.Text = "Soft Reset";
            this.lblSoftReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblModule1
            // 
            this.lblModule1.AutoSize = true;
            this.lblModule1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblModule1.Location = new System.Drawing.Point(4, 525);
            this.lblModule1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblModule1.Name = "lblModule1";
            this.lblModule1.Size = new System.Drawing.Size(247, 36);
            this.lblModule1.TabIndex = 30;
            this.lblModule1.Text = "Module 1 IP";
            this.lblModule1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblModule1.Visible = false;
            // 
            // txtModule1
            // 
            this.txtModule1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtModule1.Location = new System.Drawing.Point(259, 528);
            this.txtModule1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtModule1.Name = "txtModule1";
            this.txtModule1.Size = new System.Drawing.Size(247, 35);
            this.txtModule1.TabIndex = 31;
            this.txtModule1.Visible = false;
            // 
            // lblModule2
            // 
            this.lblModule2.AutoSize = true;
            this.lblModule2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblModule2.Location = new System.Drawing.Point(4, 600);
            this.lblModule2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblModule2.Name = "lblModule2";
            this.lblModule2.Size = new System.Drawing.Size(247, 36);
            this.lblModule2.TabIndex = 32;
            this.lblModule2.Text = "Module 2 IP";
            this.lblModule2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblModule2.Visible = false;
            // 
            // txtModule2
            // 
            this.txtModule2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtModule2.Location = new System.Drawing.Point(259, 603);
            this.txtModule2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtModule2.Name = "txtModule2";
            this.txtModule2.Size = new System.Drawing.Size(247, 35);
            this.txtModule2.TabIndex = 33;
            this.txtModule2.Visible = false;
            // 
            // lblModule3
            // 
            this.lblModule3.AutoSize = true;
            this.lblModule3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblModule3.Location = new System.Drawing.Point(4, 675);
            this.lblModule3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblModule3.Name = "lblModule3";
            this.lblModule3.Size = new System.Drawing.Size(247, 36);
            this.lblModule3.TabIndex = 34;
            this.lblModule3.Text = "Module 3 IP";
            this.lblModule3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblModule3.Visible = false;
            // 
            // txtModule3
            // 
            this.txtModule3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtModule3.Location = new System.Drawing.Point(259, 678);
            this.txtModule3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtModule3.Name = "txtModule3";
            this.txtModule3.Size = new System.Drawing.Size(247, 35);
            this.txtModule3.TabIndex = 35;
            this.txtModule3.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox4, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 70);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.6807747F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.6948853F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38.62434F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(517, 981);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(4, 3);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(509, 412);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Display Address";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.Beige;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52F));
            this.tableLayoutPanel3.Controls.Add(this.txtLocation, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtIpConfig, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label10, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmbNoOfLine, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 31);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(501, 378);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // txtLocation
            // 
            this.txtLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLocation.Location = new System.Drawing.Point(244, 153);
            this.txtLocation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(253, 35);
            this.txtLocation.TabIndex = 13;
            this.txtLocation.Text = "NA";
            // 
            // txtIpConfig
            // 
            this.txtIpConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIpConfig.Location = new System.Drawing.Point(244, 78);
            this.txtIpConfig.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtIpConfig.Name = "txtIpConfig";
            this.txtIpConfig.Size = new System.Drawing.Size(253, 35);
            this.txtIpConfig.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(4, 150);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(232, 75);
            this.label10.TabIndex = 8;
            this.label10.Text = "Installed Location";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(4, 75);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(232, 75);
            this.label6.TabIndex = 4;
            this.label6.Text = "IP Config";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(4, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 75);
            this.label2.TabIndex = 15;
            this.label2.Text = "No of Line";
            // 
            // cmbNoOfLine
            // 
            this.cmbNoOfLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbNoOfLine.FormattingEnabled = true;
            this.cmbNoOfLine.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            this.cmbNoOfLine.Location = new System.Drawing.Point(244, 5);
            this.cmbNoOfLine.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbNoOfLine.Name = "cmbNoOfLine";
            this.cmbNoOfLine.Size = new System.Drawing.Size(253, 36);
            this.cmbNoOfLine.TabIndex = 16;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkIsActive.Location = new System.Drawing.Point(244, 305);
            this.chkIsActive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(253, 68);
            this.chkIsActive.TabIndex = 14;
            this.chkIsActive.Text = "IsActive";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // lblDto
            // 
            this.lblDto.AutoSize = true;
            this.lblDto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDto.Location = new System.Drawing.Point(4, 225);
            this.lblDto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDto.Name = "lblDto";
            this.lblDto.Size = new System.Drawing.Size(232, 36);
            this.lblDto.TabIndex = 17;
            this.lblDto.Text = "DTO";
            this.lblDto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxSoftReset
            // 
            this.checkBoxSoftReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxSoftReset.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBoxSoftReset.Location = new System.Drawing.Point(259, 252);
            this.checkBoxSoftReset.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxSoftReset.Name = "checkBoxSoftReset";
            this.checkBoxSoftReset.Size = new System.Drawing.Size(247, 36);
            this.checkBoxSoftReset.TabIndex = 28;
            this.checkBoxSoftReset.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.flow_layout_language);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(4, 421);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Size = new System.Drawing.Size(509, 177);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Language Selection";
            // 
            // flow_layout_language
            // 
            this.flow_layout_language.BackColor = System.Drawing.Color.Beige;
            this.flow_layout_language.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flow_layout_language.Location = new System.Drawing.Point(4, 31);
            this.flow_layout_language.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.flow_layout_language.Name = "flow_layout_language";
            this.flow_layout_language.Size = new System.Drawing.Size(501, 143);
            this.flow_layout_language.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.tableLayoutPanel5);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(4, 604);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox4.Size = new System.Drawing.Size(509, 374);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Platform Selection";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.Color.Beige;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.flow_layout_Plateform, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(4, 31);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.5116272F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78.48837F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(501, 340);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // flow_layout_Plateform
            // 
            this.flow_layout_Plateform.BackColor = System.Drawing.Color.Beige;
            this.tableLayoutPanel5.SetColumnSpan(this.flow_layout_Plateform, 2);
            this.flow_layout_Plateform.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flow_layout_Plateform.Location = new System.Drawing.Point(4, 76);
            this.flow_layout_Plateform.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.flow_layout_Plateform.Name = "flow_layout_Plateform";
            this.flow_layout_Plateform.Size = new System.Drawing.Size(493, 261);
            this.flow_layout_Plateform.TabIndex = 2;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkSelectAll.Location = new System.Drawing.Point(4, 5);
            this.chkSelectAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(242, 63);
            this.chkSelectAll.TabIndex = 3;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // ucSingleLineBoardSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(740, 530);
            this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(740, 530);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ucSingleLineBoardSetting";
            this.Size = new System.Drawing.Size(1051, 1133);
            this.Load += new System.EventHandler(this.ucSingleLineBoardSetting_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label lblboardHeader;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ComboBox cmbDisplayinfo;
        private System.Windows.Forms.ComboBox cmbEffect;
        private System.Windows.Forms.ComboBox cmbDTO;
        private System.Windows.Forms.ComboBox cmbScrollSpeed;
        private System.Windows.Forms.ComboBox cmbDelayTime;
        private System.Windows.Forms.ComboBox cmbBoardSize;
        private System.Windows.Forms.ComboBox cmbFontSize;
        private System.Windows.Forms.ComboBox cmbIntensity;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label intensityLbl;
        private System.Windows.Forms.Label lblSoftReset;
        private System.Windows.Forms.CheckBox checkBoxSoftReset;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.TextBox txtIpConfig;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbNoOfLine;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Label lblDto;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.FlowLayoutPanel flow_layout_language;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.FlowLayoutPanel flow_layout_Plateform;
        private System.Windows.Forms.CheckBox chkSelectAll;

        // NEW module controls
        private System.Windows.Forms.Label lblModule1;
        private System.Windows.Forms.TextBox txtModule1;
        private System.Windows.Forms.Label lblModule2;
        private System.Windows.Forms.TextBox txtModule2;
        private System.Windows.Forms.Label lblModule3;
        private System.Windows.Forms.TextBox txtModule3;
    }
}