using ProyectoP5.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProyectoP5.Presentacion
{
    public partial class Actualizar_usuario : Form
    {
        public Actualizar_usuario()
        {
            InitializeComponent();
        }
        public Cliente? cliente_seleccionado { get; set; }
        private void button3_Click(object sender, EventArgs e)
        {
            Gestiondeusuarios gestiondeusuarios = new Gestiondeusuarios();
            gestiondeusuarios.Show();
            this.Hide();
        }

        private void Actualizar_usuario_Load(object sender, EventArgs e)
        {
            dgvbuscar.DataSource = Gestion_usuarios.Obtenertodos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                dgvbuscar.DataSource = Gestion_usuarios.Obtenertodos();
            else
                dgvbuscar.DataSource = Gestion_usuarios.Buscar(textBox1.Text);

        }

        private void dgvbuscar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cliente_seleccionado == null)
            {
                MessageBox.Show("Seleccione un cliente.");
                return;
            }

            Cliente cliente = new Cliente();

            cliente.Nombre = txtNombre.Text.Trim();
            cliente.Telefono = txtTelefono.Text.Trim();
            cliente.Correo = txtCorreo.Text.Trim();
            cliente.Id = cliente_seleccionado.Id;

            if (Gestion_usuarios.Actualizar(cliente)> 0) {

                MessageBox.Show("Datos del cliente actualizados correctamente","Actualización de cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);
           }
            else
            {
                MessageBox.Show("No se puedo actualizar el cliente", "Error al actualizar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
        }

        private void dgvbuscar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow fila = dgvbuscar.Rows[e.RowIndex];

            int id = Convert.ToInt32(fila.Cells[0].Value);
            cliente_seleccionado = Gestion_usuarios.ObtenerCliente(id);

                txtID.Text = cliente_seleccionado.Id.ToString();
                txtNombre.Text = cliente_seleccionado.Nombre;
                txtTelefono.Text = cliente_seleccionado.Telefono;
                txtCorreo.Text = cliente_seleccionado.Correo;
            
        }
    }
}
