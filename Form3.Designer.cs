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
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(148, 190);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(638, 120);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "最新接口更新请致电19967120544";
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
            // Form3qtjk
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(978, 522);
            Controls.Add(hqybzxsj);
            Controls.Add(richTextBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form3qtjk";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "医保";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
        private Button hqybzxsj;
    }
}