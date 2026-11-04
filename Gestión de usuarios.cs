using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoP5.Presentacion
{
    public partial class Gestiondeusuarios : Form
    {
        public Gestiondeusuarios()
        {
            InitializeComponent();
        }

        private void Gestiondeusuarios_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Registrodeusuarios form2 = new Registrodeusuarios();
            form2.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Consultar_usuario consultar_usuario = new Consultar_usuario();
            consultar_usuario.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Actualizar_usuario actualizar_usuario = new Actualizar_usuario();
            actualizar_usuario.Show();
            this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Eliminar_usuarios eliminar_usuarios = new Eliminar_usuarios();
            eliminar_usuarios.Show();
            this.Hide();    
        }
    }
}
