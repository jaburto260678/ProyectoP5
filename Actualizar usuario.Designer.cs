namespace ProyectoP5.Presentacion
{
    partial class Actualizar_usuario
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            textBox1 = new TextBox();
            txtID = new TextBox();
            txtNombre = new TextBox();
            txtTelefono = new TextBox();
            txtCorreo = new TextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            dgvbuscar = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvbuscar).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new Font("Arial", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(237, 9);
            label1.Name = "label1";
            label1.Size = new Size(341, 50);
            label1.TabIndex = 0;
            label1.Text = "Actualización de usuario";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(28, 351);
            label2.Name = "label2";
            label2.Size = new Size(17, 15);
            label2.TabIndex = 1;
            label2.Text = "Id";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(162, 351);
            label3.Name = "label3";
            label3.Size = new Size(51, 15);
            label3.TabIndex = 2;
            label3.Text = "Nombre";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(293, 351);
            label4.Name = "label4";
            label4.Size = new Size(53, 15);
            label4.TabIndex = 3;
            label4.Text = "Teléfono";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(398, 351);
            label5.Name = "label5";
            label5.Size = new Size(105, 15);
            label5.TabIndex = 4;
            label5.Text = "Correo electrónico";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(16, 60);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(691, 23);
            textBox1.TabIndex = 6;
            // 
            // txtID
            // 
            txtID.Location = new Point(17, 369);
            txtID.Name = "txtID";
            txtID.Size = new Size(75, 23);
            txtID.TabIndex = 7;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(143, 369);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(75, 23);
            txtNombre.TabIndex = 8;
            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(281, 369);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(75, 23);
            txtTelefono.TabIndex = 9;
            // 
            // txtCorreo
            // 
            txtCorreo.Location = new Point(415, 369);
            txtCorreo.Name = "txtCorreo";
            txtCorreo.Size = new Size(75, 23);
            txtCorreo.TabIndex = 10;
            // 
            // button1
            // 
            button1.Location = new Point(713, 60);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 11;
            button1.Text = "Buscar ";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(649, 369);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 12;
            button2.Text = "Actualizar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(546, 369);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 13;
            button3.Text = "Regresar";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // dgvbuscar
            // 
            dgvbuscar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvbuscar.Location = new Point(17, 94);
            dgvbuscar.Name = "dgvbuscar";
            dgvbuscar.Size = new Size(707, 254);
            dgvbuscar.TabIndex = 14;
            dgvbuscar.CellClick += dgvbuscar_CellClick;
            dgvbuscar.CellContentClick += dgvbuscar_CellContentClick;
            // 
            // Actualizar_usuario
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvbuscar);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(txtCorreo);
            Controls.Add(txtTelefono);
            Controls.Add(txtNombre);
            Controls.Add(txtID);
            Controls.Add(textBox1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Actualizar_usuario";
            Text = "Actualizar_usuario";
            Load += Actualizar_usuario_Load;
            ((System.ComponentModel.ISupportInitialize)dgvbuscar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox textBox1;
        private TextBox txtID;
        private TextBox txtNombre;
        private TextBox txtTelefono;
        private TextBox txtCorreo;
        private Button button1;
        private Button button2;
        private Button button3;
        private DataGridView dgvbuscar;
    }
}