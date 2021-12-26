namespace TM_PresetManager
{
    partial class MainForm
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.strengthValue = new System.Windows.Forms.NumericUpDown();
            this.constantGainValue = new System.Windows.Forms.NumericUpDown();
            this.periodicGainValue = new System.Windows.Forms.NumericUpDown();
            this.springGainValue = new System.Windows.Forms.NumericUpDown();
            this.damperGainValue = new System.Windows.Forms.NumericUpDown();
            this.WheelNameLbl = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.presetLbl = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.wheelAngleValue = new System.Windows.Forms.NumericUpDown();
            this.presetListBox = new System.Windows.Forms.ListBox();
            this.createNewBtn = new System.Windows.Forms.Button();
            this.updatePresetBtn = new System.Windows.Forms.Button();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.strengthValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.constantGainValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.periodicGainValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.springGainValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.damperGainValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wheelAngleValue)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Overall Strength";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Constant Gain";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Periodic Gain";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Spring Gain";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Damper Gain";
            // 
            // strengthValue
            // 
            this.strengthValue.Location = new System.Drawing.Point(140, 104);
            this.strengthValue.Name = "strengthValue";
            this.strengthValue.Size = new System.Drawing.Size(94, 22);
            this.strengthValue.TabIndex = 5;
            this.strengthValue.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // constantGainValue
            // 
            this.constantGainValue.Location = new System.Drawing.Point(140, 132);
            this.constantGainValue.Name = "constantGainValue";
            this.constantGainValue.Size = new System.Drawing.Size(94, 22);
            this.constantGainValue.TabIndex = 6;
            this.constantGainValue.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // periodicGainValue
            // 
            this.periodicGainValue.Location = new System.Drawing.Point(140, 160);
            this.periodicGainValue.Name = "periodicGainValue";
            this.periodicGainValue.Size = new System.Drawing.Size(94, 22);
            this.periodicGainValue.TabIndex = 7;
            this.periodicGainValue.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // springGainValue
            // 
            this.springGainValue.Location = new System.Drawing.Point(140, 188);
            this.springGainValue.Name = "springGainValue";
            this.springGainValue.Size = new System.Drawing.Size(94, 22);
            this.springGainValue.TabIndex = 8;
            this.springGainValue.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // damperGainValue
            // 
            this.damperGainValue.Location = new System.Drawing.Point(140, 216);
            this.damperGainValue.Name = "damperGainValue";
            this.damperGainValue.Size = new System.Drawing.Size(94, 22);
            this.damperGainValue.TabIndex = 9;
            this.damperGainValue.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // WheelNameLbl
            // 
            this.WheelNameLbl.AutoSize = true;
            this.WheelNameLbl.Location = new System.Drawing.Point(78, 13);
            this.WheelNameLbl.Name = "WheelNameLbl";
            this.WheelNameLbl.Size = new System.Drawing.Size(114, 17);
            this.WheelNameLbl.TabIndex = 12;
            this.WheelNameLbl.Text = "No Wheel Found";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "Preset";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Wheel";
            // 
            // presetLbl
            // 
            this.presetLbl.AutoSize = true;
            this.presetLbl.Location = new System.Drawing.Point(78, 39);
            this.presetLbl.Name = "presetLbl";
            this.presetLbl.Size = new System.Drawing.Size(42, 17);
            this.presetLbl.TabIndex = 15;
            this.presetLbl.Text = "None";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(45, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 17);
            this.label8.TabIndex = 16;
            this.label8.Text = "Wheel Angle";
            // 
            // wheelAngleValue
            // 
            this.wheelAngleValue.Location = new System.Drawing.Point(140, 76);
            this.wheelAngleValue.Maximum = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            this.wheelAngleValue.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.wheelAngleValue.Name = "wheelAngleValue";
            this.wheelAngleValue.Size = new System.Drawing.Size(94, 22);
            this.wheelAngleValue.TabIndex = 17;
            this.wheelAngleValue.Value = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            // 
            // presetListBox
            // 
            this.presetListBox.FormattingEnabled = true;
            this.presetListBox.ItemHeight = 16;
            this.presetListBox.Location = new System.Drawing.Point(251, 76);
            this.presetListBox.Name = "presetListBox";
            this.presetListBox.Size = new System.Drawing.Size(210, 164);
            this.presetListBox.TabIndex = 18;
            this.presetListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.presetListBox_MouseDoubleClick);
            // 
            // createNewBtn
            // 
            this.createNewBtn.Location = new System.Drawing.Point(251, 253);
            this.createNewBtn.Name = "createNewBtn";
            this.createNewBtn.Size = new System.Drawing.Size(89, 23);
            this.createNewBtn.TabIndex = 19;
            this.createNewBtn.Text = "Create New";
            this.createNewBtn.UseVisualStyleBackColor = true;
            this.createNewBtn.Click += new System.EventHandler(this.createNewBtn_Click);
            // 
            // updatePresetBtn
            // 
            this.updatePresetBtn.Location = new System.Drawing.Point(346, 253);
            this.updatePresetBtn.Name = "updatePresetBtn";
            this.updatePresetBtn.Size = new System.Drawing.Size(115, 23);
            this.updatePresetBtn.TabIndex = 20;
            this.updatePresetBtn.Text = "Update Preset";
            this.updatePresetBtn.UseVisualStyleBackColor = true;
            this.updatePresetBtn.Click += new System.EventHandler(this.updatePresetBtn_Click);
            // 
            // deleteBtn
            // 
            this.deleteBtn.Location = new System.Drawing.Point(346, 47);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(115, 23);
            this.deleteBtn.TabIndex = 21;
            this.deleteBtn.Text = "Delete Preset";
            this.deleteBtn.UseVisualStyleBackColor = true;
            this.deleteBtn.Click += new System.EventHandler(this.deleteBtn_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "TM Preset Manager";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 290);
            this.Controls.Add(this.deleteBtn);
            this.Controls.Add(this.updatePresetBtn);
            this.Controls.Add(this.createNewBtn);
            this.Controls.Add(this.presetListBox);
            this.Controls.Add(this.wheelAngleValue);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.presetLbl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.WheelNameLbl);
            this.Controls.Add(this.damperGainValue);
            this.Controls.Add(this.springGainValue);
            this.Controls.Add(this.periodicGainValue);
            this.Controls.Add(this.constantGainValue);
            this.Controls.Add(this.strengthValue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "TM Preset Manager";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.strengthValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.constantGainValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.periodicGainValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.springGainValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.damperGainValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wheelAngleValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown strengthValue;
        private System.Windows.Forms.NumericUpDown constantGainValue;
        private System.Windows.Forms.NumericUpDown periodicGainValue;
        private System.Windows.Forms.NumericUpDown springGainValue;
        private System.Windows.Forms.NumericUpDown damperGainValue;
        private System.Windows.Forms.Label WheelNameLbl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label presetLbl;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown wheelAngleValue;
        private System.Windows.Forms.ListBox presetListBox;
        private System.Windows.Forms.Button createNewBtn;
        private System.Windows.Forms.Button updatePresetBtn;
        private System.Windows.Forms.Button deleteBtn;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

