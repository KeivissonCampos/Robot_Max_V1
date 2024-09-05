namespace RobotMax
{
    partial class Cena
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
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.button_fechar = new System.Windows.Forms.Button();
            this.button_parar = new System.Windows.Forms.Button();
            this.button_abrir = new System.Windows.Forms.Button();
            this.button_ligar = new System.Windows.Forms.Button();
            this.button_desligar = new System.Windows.Forms.Button();
            this.button_iniciar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // button_fechar
            // 
            this.button_fechar.Location = new System.Drawing.Point(12, 12);
            this.button_fechar.Name = "button_fechar";
            this.button_fechar.Size = new System.Drawing.Size(75, 23);
            this.button_fechar.TabIndex = 1;
            this.button_fechar.Text = "FECHAR";
            this.button_fechar.UseVisualStyleBackColor = true;
            this.button_fechar.Click += new System.EventHandler(this.button_fechar_Click);
            // 
            // button_parar
            // 
            this.button_parar.Location = new System.Drawing.Point(93, 12);
            this.button_parar.Name = "button_parar";
            this.button_parar.Size = new System.Drawing.Size(75, 23);
            this.button_parar.TabIndex = 2;
            this.button_parar.Text = "PARAR";
            this.button_parar.UseVisualStyleBackColor = true;
            this.button_parar.Click += new System.EventHandler(this.button_parar_Click);
            // 
            // button_abrir
            // 
            this.button_abrir.Location = new System.Drawing.Point(174, 12);
            this.button_abrir.Name = "button_abrir";
            this.button_abrir.Size = new System.Drawing.Size(75, 23);
            this.button_abrir.TabIndex = 3;
            this.button_abrir.Text = "ABRIR";
            this.button_abrir.UseVisualStyleBackColor = true;
            this.button_abrir.Click += new System.EventHandler(this.button_abrir_Click);
            // 
            // button_ligar
            // 
            this.button_ligar.Location = new System.Drawing.Point(15, 54);
            this.button_ligar.Name = "button_ligar";
            this.button_ligar.Size = new System.Drawing.Size(75, 23);
            this.button_ligar.TabIndex = 4;
            this.button_ligar.Text = "LIGAR";
            this.button_ligar.UseVisualStyleBackColor = true;
            this.button_ligar.Click += new System.EventHandler(this.button_ligar_Click);
            // 
            // button_desligar
            // 
            this.button_desligar.Location = new System.Drawing.Point(96, 54);
            this.button_desligar.Name = "button_desligar";
            this.button_desligar.Size = new System.Drawing.Size(75, 23);
            this.button_desligar.TabIndex = 5;
            this.button_desligar.Text = "DESLIGAR";
            this.button_desligar.UseVisualStyleBackColor = true;
            this.button_desligar.Click += new System.EventHandler(this.button_desligar_Click);
            // 
            // button_iniciar
            // 
            this.button_iniciar.Location = new System.Drawing.Point(338, 12);
            this.button_iniciar.Name = "button_iniciar";
            this.button_iniciar.Size = new System.Drawing.Size(75, 23);
            this.button_iniciar.TabIndex = 6;
            this.button_iniciar.Text = "INICIAR";
            this.button_iniciar.UseVisualStyleBackColor = true;
            this.button_iniciar.Click += new System.EventHandler(this.button_iniciar_Click);
            // 
            // Cena
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 162);
            this.Controls.Add(this.button_iniciar);
            this.Controls.Add(this.button_desligar);
            this.Controls.Add(this.button_ligar);
            this.Controls.Add(this.button_abrir);
            this.Controls.Add(this.button_parar);
            this.Controls.Add(this.button_fechar);
            this.Controls.Add(this.label1);
            this.Name = "Cena";
            this.Text = "Cena";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Cena_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button button_fechar;
        private System.Windows.Forms.Button button_parar;
        private System.Windows.Forms.Button button_abrir;
        private System.Windows.Forms.Button button_ligar;
        private System.Windows.Forms.Button button_desligar;
        private System.Windows.Forms.Button button_iniciar;
    }
}