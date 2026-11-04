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
    public partial class Consultar_usuario : Form
    {
        public Consultar_usuario()
        {
            InitializeComponent();
        }

        private void Consultar_usuario_Load(object sender, EventArgs e)
        {
            dgvBuscar.DataSource = Gestion_usuarios.Obtenertodos();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Gestiondeusuarios gestiondeusuarios = new Gestiondeusuarios();
            gestiondeusuarios.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                dgvBuscar.DataSource = Gestion_usuarios.Obtenertodos();
            else
                dgvBuscar.DataSource = Gestion_usuarios.Buscar(textBox1.Text);
        }
        private void dgvBuscar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
