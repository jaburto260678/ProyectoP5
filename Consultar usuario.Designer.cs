namespace ProyectoP5.Presentacion
{
    partial class Consultar_usuario
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
            textBox1 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            dgvBuscar = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvBuscar).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 73);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(678, 23);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // button1
            // 
            button1.Location = new Point(696, 72);
            button1.Name = "button1";
            button1.Size = new Size(92, 23);
            button1.TabIndex = 2;
            button1.Text = "Buscar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(12, 12);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 3;
            button2.Text = "Volver ";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.Font = new Font("Arial", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(217, 12);
            label1.Name = "label1";
            label1.Size = new Size(349, 51);
            label1.TabIndex = 6;
            label1.Text = "Buscar usuario";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dgvBuscar
            // 
            dgvBuscar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBuscar.Location = new Point(12, 116);
            dgvBuscar.Name = "dgvBuscar";
            dgvBuscar.Size = new Size(776, 332);
            dgvBuscar.TabIndex = 7;
            dgvBuscar.CellContentClick += dgvBuscar_CellContentClick;
            // 
            // Consultar_usuario
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvBuscar);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Name = "Consultar_usuario";
            Text = "Consultar usuario";
            Load += Consultar_usuario_Load;
            ((System.ComponentModel.ISupportInitialize)dgvBuscar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBox1;
        private Button button1;
        private Button button2;
        private Label label1;
        private DataGridView dgvBuscar;
    }
}