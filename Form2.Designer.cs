namespace Ybsfsb
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            rybh28d = new TextBox();
            cbssfzx28d = new TextBox();
            jgbm47 = new TextBox();
            rybh47 = new TextBox();
            drsjk = new DataGridView();
            H28d = new Button();
            ybzdxz = new Button();
            dc = new Button();
            drsjcs = new Button();
            jsxxcx47 = new Button();
            label5 = new Label();
            label6 = new Label();
            kssj = new DateTimePicker();
            jssj = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)drsjk).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 9);
            label1.Name = "label1";
            label1.Size = new Size(69, 20);
            label1.TabIndex = 0;
            label1.Text = "人员编号";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 332);
            label2.Name = "label2";
            label2.Size = new Size(69, 20);
            label2.TabIndex = 1;
            label2.Text = "人员编号";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 292);
            label3.Name = "label3";
            label3.Size = new Size(69, 20);
            label3.TabIndex = 2;
            label3.Text = "机构编码";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(19, 52);
            label4.Name = "label4";
            label4.Size = new Size(114, 20);
            label4.TabIndex = 3;
            label4.Text = "参保所属分中心";
            // 
            // rybh28d
            // 
            rybh28d.Location = new Point(88, 7);
            rybh28d.Name = "rybh28d";
            rybh28d.Size = new Size(272, 27);
            rybh28d.TabIndex = 4;
            // 
            // cbssfzx28d
            // 
            cbssfzx28d.Location = new Point(132, 49);
            cbssfzx28d.Name = "cbssfzx28d";
            cbssfzx28d.Size = new Size(162, 27);
            cbssfzx28d.TabIndex = 5;
            // 
            // jgbm47
            // 
            jgbm47.Location = new Point(103, 293);
            jgbm47.Name = "jgbm47";
            jgbm47.Size = new Size(156, 27);
            jgbm47.TabIndex = 6;
            // 
            // rybh47
            // 
            rybh47.Location = new Point(91, 329);
            rybh47.Name = "rybh47";
            rybh47.Size = new Size(274, 27);
            rybh47.TabIndex = 7;
            // 
            // drsjk
            // 
            drsjk.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            drsjk.Location = new Point(-1, 123);
            drsjk.Name = "drsjk";
            drsjk.RowHeadersWidth = 51;
            drsjk.Size = new Size(798, 161);
            drsjk.TabIndex = 8;
            // 
            // H28d
            // 
            H28d.ForeColor = SystemColors.Highlight;
            H28d.Location = new Point(39, 88);
            H28d.Name = "H28d";
            H28d.Size = new Size(220, 29);
            H28d.TabIndex = 9;
            H28d.Text = "查询有无在院信息（H28d）";
            H28d.UseVisualStyleBackColor = true;
            H28d.Click += H28d_Click;
            // 
            // ybzdxz
            // 
            ybzdxz.ForeColor = SystemColors.Highlight;
            ybzdxz.Location = new Point(652, 7);
            ybzdxz.Name = "ybzdxz";
            ybzdxz.Size = new Size(123, 29);
            ybzdxz.TabIndex = 10;
            ybzdxz.Text = "医保字典下载";
            ybzdxz.UseVisualStyleBackColor = true;
            ybzdxz.Click += ybzdxz_Click;
            // 
            // dc
            // 
            dc.ForeColor = SystemColors.Highlight;
            dc.Location = new Point(665, 88);
            dc.Name = "dc";
            dc.Size = new Size(123, 29);
            dc.TabIndex = 11;
            dc.Text = "导出";
            dc.UseVisualStyleBackColor = true;
            dc.Click += dc_Click;
            // 
            // drsjcs
            // 
            drsjcs.ForeColor = SystemColors.Highlight;
            drsjcs.Location = new Point(536, 88);
            drsjcs.Name = "drsjcs";
            drsjcs.Size = new Size(123, 29);
            drsjcs.TabIndex = 12;
            drsjcs.Text = "导入数据测试";
            drsjcs.UseVisualStyleBackColor = true;
            drsjcs.Click += drsjcs_Click;
            // 
            // jsxxcx47
            // 
            jsxxcx47.ForeColor = SystemColors.Highlight;
            jsxxcx47.Location = new Point(307, 382);
            jsxxcx47.Name = "jsxxcx47";
            jsxxcx47.Size = new Size(185, 29);
            jsxxcx47.TabIndex = 13;
            jsxxcx47.Text = "结算信息查询（47）";
            jsxxcx47.UseVisualStyleBackColor = true;
            jsxxcx47.Click += jsxxcx47_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(434, 333);
            label5.Name = "label5";
            label5.Size = new Size(69, 20);
            label5.TabIndex = 14;
            label5.Text = "结束时间";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(434, 296);
            label6.Name = "label6";
            label6.Size = new Size(69, 20);
            label6.TabIndex = 15;
            label6.Text = "开始时间";
            // 
            // kssj
            // 
            kssj.Location = new Point(527, 292);
            kssj.Name = "kssj";
            kssj.Size = new Size(166, 27);
            kssj.TabIndex = 16;
            // 
            // jssj
            // 
            jssj.Location = new Point(525, 329);
            jssj.Name = "jssj";
            jssj.Size = new Size(168, 27);
            jssj.TabIndex = 17;
            // 
            // Form2
            // 
            AccessibleRole = AccessibleRole.MenuBar;
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(jssj);
            Controls.Add(kssj);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(jsxxcx47);
            Controls.Add(drsjcs);
            Controls.Add(dc);
            Controls.Add(ybzdxz);
            Controls.Add(H28d);
            Controls.Add(drsjk);
            Controls.Add(rybh47);
            Controls.Add(jgbm47);
            Controls.Add(cbssfzx28d);
            Controls.Add(rybh28d);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "医保";
            ((System.ComponentModel.ISupportInitialize)drsjk).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox rybh28d;
        private TextBox cbssfzx28d;
        private TextBox jgbm47;
        private TextBox rybh47;
        private DataGridView drsjk;
        private Button H28d;
        private Button ybzdxz;
        private Button dc;
        private Button drsjcs;
        private Button jsxxcx47;
        private Label label5;
        private Label label6;
        private DateTimePicker kssj;
        private DateTimePicker jssj;
    }
}