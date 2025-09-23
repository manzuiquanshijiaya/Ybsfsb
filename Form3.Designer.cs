namespace Ybsfsb
{
    partial class Form3qtjk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3qtjk));
            richTextBox1 = new RichTextBox();
            hqybzxsj = new Button();
            hsbmxxxz = new Button();
            xzxxsjk = new DataGridView();
            button1 = new Button();
            ysxxxz = new Button();
            yjryxxxz = new Button();
            yjyfryxxxz = new Button();
            ybmlxj = new Button();
            ((System.ComponentModel.ISupportInitialize)xzxxsjk).BeginInit();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(161, 390);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(638, 120);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "最新接口更新请致电19967120544  或 1874859723";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // hqybzxsj
            // 
            hqybzxsj.ForeColor = SystemColors.Highlight;
            hqybzxsj.Location = new Point(17, 12);
            hqybzxsj.Name = "hqybzxsj";
            hqybzxsj.Size = new Size(197, 29);
            hqybzxsj.TabIndex = 1;
            hqybzxsj.Text = "获取医保中心时间（52）";
            hqybzxsj.UseVisualStyleBackColor = true;
            hqybzxsj.Click += hqybzxsj_Click;
            // 
            // hsbmxxxz
            // 
            hsbmxxxz.ForeColor = SystemColors.Highlight;
            hsbmxxxz.Location = new Point(245, 13);
            hsbmxxxz.Name = "hsbmxxxz";
            hsbmxxxz.Size = new Size(232, 29);
            hsbmxxxz.TabIndex = 2;
            hsbmxxxz.Text = "护士编码信息下载（GB002）";
            hsbmxxxz.UseVisualStyleBackColor = true;
            hsbmxxxz.Click += button1_Click;
            // 
            // xzxxsjk
            // 
            xzxxsjk.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            xzxxsjk.Location = new Point(6, 150);
            xzxxsjk.Name = "xzxxsjk";
            xzxxsjk.RowHeadersWidth = 51;
            xzxxsjk.Size = new Size(969, 188);
            xzxxsjk.TabIndex = 3;
            // 
            // button1
            // 
            button1.ForeColor = SystemColors.Highlight;
            button1.Location = new Point(896, 115);
            button1.Name = "button1";
            button1.Size = new Size(70, 29);
            button1.TabIndex = 4;
            button1.Text = "导出";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // ysxxxz
            // 
            ysxxxz.ForeColor = SystemColors.Highlight;
            ysxxxz.Location = new Point(495, 14);
            ysxxxz.Name = "ysxxxz";
            ysxxxz.Size = new Size(223, 29);
            ysxxxz.TabIndex = 5;
            ysxxxz.Text = "医生编码信息下载(GB001)";
            ysxxxz.UseVisualStyleBackColor = true;
            ysxxxz.Click += ysxxxz_Click;
            // 
            // yjryxxxz
            // 
            yjryxxxz.Font = new Font("Microsoft YaHei UI", 7F);
            yjryxxxz.ForeColor = SystemColors.Highlight;
            yjryxxxz.Location = new Point(724, 15);
            yjryxxxz.Name = "yjryxxxz";
            yjryxxxz.Size = new Size(226, 29);
            yjryxxxz.TabIndex = 6;
            yjryxxxz.Text = "医技人员检验超声信息下载（GB004）";
            yjryxxxz.UseVisualStyleBackColor = true;
            yjryxxxz.Click += yjryxxxz_Click;
            // 
            // yjyfryxxxz
            // 
            yjyfryxxxz.Font = new Font("Microsoft YaHei UI", 7F);
            yjyfryxxxz.ForeColor = SystemColors.Highlight;
            yjyfryxxxz.Location = new Point(17, 60);
            yjyfryxxxz.Name = "yjyfryxxxz";
            yjyfryxxxz.Size = new Size(191, 29);
            yjyfryxxxz.TabIndex = 7;
            yjyfryxxxz.Text = "医技药房人员信息（GB005）";
            yjyfryxxxz.UseVisualStyleBackColor = true;
            yjyfryxxxz.Click += yjyfryxxxz_Click;
            // 
            // ybmlxj
            // 
            ybmlxj.ForeColor = SystemColors.Highlight;
            ybmlxj.Location = new Point(250, 66);
            ybmlxj.Name = "ybmlxj";
            ybmlxj.Size = new Size(227, 29);
            ybmlxj.TabIndex = 8;
            ybmlxj.Text = "医保目录信息(91ANew)";
            ybmlxj.UseVisualStyleBackColor = true;
            ybmlxj.Click += ybmlxj_Click;
            // 
            // Form3qtjk
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(978, 522);
            Controls.Add(ybmlxj);
            Controls.Add(yjyfryxxxz);
            Controls.Add(yjryxxxz);
            Controls.Add(ysxxxz);
            Controls.Add(button1);
            Controls.Add(xzxxsjk);
            Controls.Add(hsbmxxxz);
            Controls.Add(hqybzxsj);
            Controls.Add(richTextBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form3qtjk";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "医保";
            ((System.ComponentModel.ISupportInitialize)xzxxsjk).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
        private Button hqybzxsj;
        private Button hsbmxxxz;
        private DataGridView xzxxsjk;
        private Button button1;
        private Button ysxxxz;
        private Button yjryxxxz;
        private Button yjyfryxxxz;
        private Button ybmlxj;
    }
}