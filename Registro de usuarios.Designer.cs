namespace ProyectoP5.Presentacion
{
    partial class Registrodeusuarios
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
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            label5 = new Label();
            SuspendLayout();
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(91, 75);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 0;
            label2.Text = "Nombre ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(92, 111);
            label3.Name = "label3";
            label3.Size = new Size(53, 15);
            label3.TabIndex = 1;
            label3.Text = "Teléfono";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(92, 150);
            label4.Name = "label4";
            label4.Size = new Size(108, 15);
            label4.TabIndex = 2;
            label4.Text = "Correo electrónico ";
            // textBox2
            // 
            textBox2.Location = new Point(91, 93);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 3;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(92, 129);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(100, 23);
            textBox3.TabIndex = 4;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(91, 168);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(100, 23);
            textBox4.TabIndex = 5;
            // 
            // button1
            // 
            button1.Location = new Point(249, 284);
            button1.Name = "button1";
            button1.Size = new Size(126, 48);
            button1.TabIndex = 6;
            button1.Text = "Regresar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(425, 284);
            button2.Name = "button2";
            button2.Size = new Size(126, 48);
            button2.TabIndex = 7;
            button2.Text = "Registrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label5
            // 
            label5.Font = new Font("Arial", 20.25F);
            label5.Location = new Point(180, 9);
            label5.Name = "label5";
            label5.Size = new Size(429, 67);
            label5.TabIndex = 8;
            label5.Text = "REGISTRO DE USUARIO";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Registrodeusuarios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label5);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Name = "Registrodeusuarios";
            Text = "REGISTRO DE USUARIOS ";
            Load += Form2_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private Button button1;
        private Button button2;
        private Label label5;
    }
}