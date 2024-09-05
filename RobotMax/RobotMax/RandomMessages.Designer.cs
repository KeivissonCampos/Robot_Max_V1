namespace RobotMax
{
    partial class Random_Messages
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Random_Messages));
            this.label1 = new System.Windows.Forms.Label();
            this.messagesTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxVoice = new System.Windows.Forms.ComboBox();
            this.Activate = new System.Windows.Forms.Button();
            this.Disable = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.randomEye = new System.Windows.Forms.CheckBox();
            this.randomNeck = new System.Windows.Forms.CheckBox();
            this.Falar = new System.Windows.Forms.Button();
            this.Movimentos = new System.Windows.Forms.Button();
            this.checkBox_teste = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Text Messages:";
            // 
            // messagesTextBox
            // 
            this.messagesTextBox.Location = new System.Drawing.Point(12, 27);
            this.messagesTextBox.Multiline = true;
            this.messagesTextBox.Name = "messagesTextBox";
            this.messagesTextBox.Size = new System.Drawing.Size(387, 237);
            this.messagesTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 275);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Use Voice";
            // 
            // comboBoxVoice
            // 
            this.comboBoxVoice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVoice.FormattingEnabled = true;
            this.comboBoxVoice.Location = new System.Drawing.Point(74, 272);
            this.comboBoxVoice.Name = "comboBoxVoice";
            this.comboBoxVoice.Size = new System.Drawing.Size(325, 21);
            this.comboBoxVoice.TabIndex = 3;
            this.comboBoxVoice.SelectedIndexChanged += new System.EventHandler(this.comboBoxVoice_SelectedIndexChanged);
            // 
            // Activate
            // 
            this.Activate.Location = new System.Drawing.Point(406, 270);
            this.Activate.Name = "Activate";
            this.Activate.Size = new System.Drawing.Size(75, 23);
            this.Activate.TabIndex = 8;
            this.Activate.Text = "Activate";
            this.Activate.UseVisualStyleBackColor = true;
            this.Activate.Click += new System.EventHandler(this.Activate_Click);
            // 
            // Disable
            // 
            this.Disable.Enabled = false;
            this.Disable.Location = new System.Drawing.Point(487, 270);
            this.Disable.Name = "Disable";
            this.Disable.Size = new System.Drawing.Size(75, 23);
            this.Disable.TabIndex = 9;
            this.Disable.Text = "Disable";
            this.Disable.UseVisualStyleBackColor = true;
            this.Disable.Click += new System.EventHandler(this.Disable_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(568, 270);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 10;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.button3_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // randomEye
            // 
            this.randomEye.AutoSize = true;
            this.randomEye.Location = new System.Drawing.Point(417, 108);
            this.randomEye.Name = "randomEye";
            this.randomEye.Size = new System.Drawing.Size(145, 17);
            this.randomEye.TabIndex = 74;
            this.randomEye.Text = "Random Eye Movements";
            this.randomEye.UseVisualStyleBackColor = true;
            // 
            // randomNeck
            // 
            this.randomNeck.AutoSize = true;
            this.randomNeck.Location = new System.Drawing.Point(417, 131);
            this.randomNeck.Name = "randomNeck";
            this.randomNeck.Size = new System.Drawing.Size(153, 17);
            this.randomNeck.TabIndex = 75;
            this.randomNeck.Text = "Random Neck Movements";
            this.randomNeck.UseVisualStyleBackColor = true;
            // 
            // Falar
            // 
            this.Falar.Location = new System.Drawing.Point(406, 25);
            this.Falar.Name = "Falar";
            this.Falar.Size = new System.Drawing.Size(235, 23);
            this.Falar.TabIndex = 77;
            this.Falar.Text = "Falar texto";
            this.Falar.UseVisualStyleBackColor = true;
            this.Falar.Visible = false;
            this.Falar.Click += new System.EventHandler(this.Falar_Click);
            // 
            // Movimentos
            // 
            this.Movimentos.Location = new System.Drawing.Point(406, 55);
            this.Movimentos.Name = "Movimentos";
            this.Movimentos.Size = new System.Drawing.Size(235, 23);
            this.Movimentos.TabIndex = 78;
            this.Movimentos.Text = "Movimento Aleatório";
            this.Movimentos.UseVisualStyleBackColor = true;
            this.Movimentos.Click += new System.EventHandler(this.Movimentos_Click);
            // 
            // checkBox_teste
            // 
            this.checkBox_teste.AutoSize = true;
            this.checkBox_teste.Location = new System.Drawing.Point(417, 155);
            this.checkBox_teste.Name = "checkBox_teste";
            this.checkBox_teste.Size = new System.Drawing.Size(148, 17);
            this.checkBox_teste.TabIndex = 79;
            this.checkBox_teste.Text = "Testar Fala e Movimentos";
            this.checkBox_teste.UseVisualStyleBackColor = true;
            this.checkBox_teste.CheckedChanged += new System.EventHandler(this.checkBox_teste_CheckedChanged);
            // 
            // Random_Messages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 305);
            this.Controls.Add(this.checkBox_teste);
            this.Controls.Add(this.Movimentos);
            this.Controls.Add(this.Falar);
            this.Controls.Add(this.randomNeck);
            this.Controls.Add(this.randomEye);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Disable);
            this.Controls.Add(this.Activate);
            this.Controls.Add(this.comboBoxVoice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.messagesTextBox);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Random_Messages";
            this.Text = "Random Messages";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Random_Messages_FormClosing);
            this.Load += new System.EventHandler(this.Random_Messages_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox messagesTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxVoice;
        private System.Windows.Forms.Button Activate;
        private System.Windows.Forms.Button Disable;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox randomEye;
        private System.Windows.Forms.CheckBox randomNeck;
        private System.Windows.Forms.Button Falar;
        private System.Windows.Forms.Button Movimentos;
        private System.Windows.Forms.CheckBox checkBox_teste;
    }
}