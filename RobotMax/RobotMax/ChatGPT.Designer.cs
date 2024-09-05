namespace RobotMax
{
    partial class ChatGPT
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
            this.components = new System.ComponentModel.Container();
            this.randomEye = new System.Windows.Forms.CheckBox();
            this.Prompt = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Prompt.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // randomEye
            // 
            this.randomEye.AutoSize = true;
            this.randomEye.Location = new System.Drawing.Point(6, 27);
            this.randomEye.Name = "randomEye";
            this.randomEye.Size = new System.Drawing.Size(121, 17);
            this.randomEye.TabIndex = 4;
            this.randomEye.Text = "Aleatóriedade Olhos";
            this.randomEye.UseVisualStyleBackColor = true;
            // 
            // Prompt
            // 
            this.Prompt.Controls.Add(this.textBox1);
            this.Prompt.Location = new System.Drawing.Point(12, 12);
            this.Prompt.Name = "Prompt";
            this.Prompt.Size = new System.Drawing.Size(331, 426);
            this.Prompt.TabIndex = 7;
            this.Prompt.TabStop = false;
            this.Prompt.Text = "Prompt";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 27);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(318, 393);
            this.textBox1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.randomEye);
            this.groupBox1.Location = new System.Drawing.Point(349, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(170, 224);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuração";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // ChatGPT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Prompt);
            this.Name = "ChatGPT";
            this.Text = "ChatGPT";
            this.Load += new System.EventHandler(this.ChatGPT_Load);
            this.Prompt.ResumeLayout(false);
            this.Prompt.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox randomEye;
        private System.Windows.Forms.GroupBox Prompt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer timer1;
    }
}