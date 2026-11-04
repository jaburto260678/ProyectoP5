using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoP5.Entidades;

namespace ProyectoP5.Presentacion
{
    public partial class Registrodeusuarios : Form
    {
        public Registrodeusuarios()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Gestiondeusuarios Gestiondeusuarios = new Gestiondeusuarios();
            Gestiondeusuarios.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Completa el nombre, el teléfono y el correo.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cliente cliente = new Cliente(
                    textBox2.Text.Trim(),
                    textBox3.Text.Trim(),
                    textBox4.Text.Trim());

                Gestion_usuarios.Agregar(cliente);

                MessageBox.Show("Cliente registrado correctamente.", "Registro completado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo registrar el cliente:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
